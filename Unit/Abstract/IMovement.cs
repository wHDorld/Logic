using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Unit.Abstract
{
    public interface IMovement
    {
        public void Move(Vector3 direction, bool isRun);
        public void ForcedMove(Vector3 direction, bool isRun);
    }
}
