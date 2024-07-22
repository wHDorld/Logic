using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities
{
    [Serializable]
    public class DungeonVisualPreset
    {
        public float PerlinNoiseScale = 3f;
        public float NoiseHeightImpact = 0.5f;
        public float NoiseOffsetImpact = 0.2f;
        public float NoiseScaleImpact = 0.15f;
        public float NoiseYRotationImpact = 16f;
        public float NoiseXZRotationImpact = 16f;
    }
}
