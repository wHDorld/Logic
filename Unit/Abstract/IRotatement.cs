using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Unit.Abstract
{
    public interface IRotatement
    {
        public void LookAt(Vector3 position);
        public void ForcedLookAt(Vector3 position);
    }
}
