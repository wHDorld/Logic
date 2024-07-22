using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.Unit.Entities
{
    [System.Serializable]
    public class UnitStatRotatement
    {
        public float LerpSpeed = 25;
        public bool LockX = true;
        public bool LockY = false;
        public bool LockZ = true;
    }
}
