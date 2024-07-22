using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.Unit.Entities
{
    [System.Serializable]
    public class UnitStatMovement
    {
        public float MoveSpeed = 0.1f;
        public float RunMultiply = 2f;
        public float InertiaAffect = 0;
        public float LerpSpeed = 25f;
    }
}
