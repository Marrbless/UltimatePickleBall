using System;
using System.Collections.Generic;

namespace PickleP2P.Core.Mods
{
    [Serializable]
    public class Manifest
    {
        public AssetMap assets;
        public Dictionary<string, string> textures;
        public Dictionary<string, string> audio;
        public string rules;
        public string physics;
        public string camera;

        [Serializable]
        public class AssetMap
        {
            public AssetEntry paddle;
            public AssetEntry ball;
            public AssetEntry net;
        }

        [Serializable]
        public class AssetEntry
        {
            public string model;
            public string mtl;
            public float[] scale;
            public float[] rotateEuler;
            public float[] offset;
        }
    }
}

