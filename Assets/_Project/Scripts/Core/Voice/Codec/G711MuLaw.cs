using System;

namespace PickleP2P.Core.Voice.Codec
{
    /// <summary>
    /// G.711 mu-law encoder/decoder for 16-bit PCM mono.
    /// </summary>
    public static class G711MuLaw
    {
        private const int Bias = 0x84;
        private const int Clip = 32635;

        public static byte[] Encode(short[] pcm16, int length)
        {
            byte[] mu = new byte[length];
            for (int i = 0; i < length; i++) mu[i] = LinearToMuLawSample(pcm16[i]);
            return mu;
        }

        public static void Encode(short[] pcm16, int length, byte[] output)
        {
            for (int i = 0; i < length; i++) output[i] = LinearToMuLawSample(pcm16[i]);
        }

        public static void Decode(byte[] muLaw, int length, short[] output)
        {
            for (int i = 0; i < length; i++) output[i] = MuLawToLinearSample(muLaw[i]);
        }

        private static byte LinearToMuLawSample(short sample)
        {
            int sign = (sample >> 8) & 0x80;
            if (sign != 0) sample = (short)-sample;
            if (sample > Clip) sample = Clip;
            sample = (short)(sample + Bias);
            int exponent = s_exponentTable[(sample >> 7) & 0xFF];
            int mantissa = (sample >> (exponent + 3)) & 0x0F;
            byte muLaw = (byte)~(sign | (exponent << 4) | mantissa);
            return muLaw;
        }

        private static short MuLawToLinearSample(byte muLaw)
        {
            muLaw = (byte)~muLaw;
            int sign = (muLaw & 0x80);
            int exponent = (muLaw >> 4) & 0x07;
            int mantissa = muLaw & 0x0F;
            int sample = ((mantissa << 3) + Bias) << exponent;
            sample -= Bias;
            return (short)(sign == 0 ? sample : -sample);
        }

        private static readonly byte[] s_exponentTable = new byte[256]
        {
            0,0,1,1,2,2,2,2,3,3,3,3,3,3,3,3,
            4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,
            5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,
            5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,
            6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,
            6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,
            6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,
            6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,
            7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,
            7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,
            7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,
            7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,
            7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,
            7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,
            7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,
            7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7
        };
    }
}

