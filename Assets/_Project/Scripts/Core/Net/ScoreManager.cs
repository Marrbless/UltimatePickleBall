using UnityEngine;
using Mirror;
using PickleP2P.Core.Config;

namespace PickleP2P.Core.Net
{
    /// <summary>
    /// Tracks and syncs score and serve state. Simplified MVP.
    /// </summary>
    public class ScoreManager : NetworkBehaviour
    {
        [SyncVar] public int scoreA;
        [SyncVar] public int scoreB;
        [SyncVar] public bool sideAServing = true;

        // Rally state
        [SyncVar] public bool rallyActive;
        [SyncVar] public bool doubleBounceSatisfied; // after two ground bounces (receiver then server)
        [SyncVar] public int bouncesReceiver;
        [SyncVar] public int bouncesServer;

        public void PointToA()
        {
            scoreA++;
            CheckWin();
        }

        public void PointToB()
        {
            scoreB++;
            CheckWin();
        }

        private void CheckWin()
        {
            int target = GameConfig.Rules.pointsToWin;
            int winBy = GameConfig.Rules.winBy;
            if (scoreA >= target && scoreA - scoreB >= winBy)
            {
                // A wins, reset for now
                scoreA = 0; scoreB = 0; sideAServing = true; ResetRally();
            }
            else if (scoreB >= target && scoreB - scoreA >= winBy)
            {
                scoreA = 0; scoreB = 0; sideAServing = false; ResetRally();
            }
        }

        public void ResetRally()
        {
            rallyActive = false;
            doubleBounceSatisfied = false;
            bouncesReceiver = 0;
            bouncesServer = 0;
            // Reset ball to server side center
            var ball = GameObject.FindObjectOfType<PickleP2P.Gameplay.Ball.BallController>();
            if (ball != null)
            {
                float z = sideAServing ? -GameConfig.Rules.courtLengthM * 0.25f : GameConfig.Rules.courtLengthM * 0.25f;
                ball.transform.position = new Vector3(0, 1.2f, z);
                ball.rb.velocity = Vector3.zero;
                ball.rb.angularVelocity = Vector3.zero;
            }
        }

        public void OnBallGrounded(Vector3 position)
        {
            float z = position.z;
            bool onASide = z > 0f;
            bool serverIsA = sideAServing;
            bool onServerSide = (onASide && serverIsA) || (!onASide && !serverIsA);
            bool onReceiverSide = !onServerSide;

            if (!rallyActive)
            {
                // First bounce must be on receiver side
                if (onReceiverSide)
                {
                    rallyActive = true;
                    bouncesReceiver = 1;
                }
                else
                {
                    // Fault on serve
                    AwardPointToOpponentOfServer();
                }
                return;
            }

            if (!doubleBounceSatisfied)
            {
                if (onReceiverSide)
                {
                    bouncesReceiver++;
                }
                else
                {
                    bouncesServer++;
                }
                if (bouncesReceiver >= 1 && bouncesServer >= 1)
                {
                    doubleBounceSatisfied = true;
                }
                return;
            }

            // After double bounce, normal rally: if ball bounces, it means previous hitter failed -> point to opponent of the side where it bounced? Simplify: If it bounces on a side, that side failed to return.
            if (onASide)
            {
                // Side A failed
                PointToB();
            }
            else
            {
                PointToA();
            }
            sideAServing = !sideAServing; // Side-out
            ResetRally();
        }

        public void OnBallNetHit()
        {
            // Net hit during serve: fault to server; during rally: point to opponent of the last hitter
            AwardPointToOpponentOfServer();
            sideAServing = !sideAServing;
            ResetRally();
        }

        private void AwardPointToOpponentOfServer()
        {
            if (sideAServing) PointToB(); else PointToA();
        }
    }
}

