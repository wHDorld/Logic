using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using AssemblyCSharp.Assets.Logic.Unit.Abstract;

namespace AssemblyCSharp.Assets.Logic.Unit.Components
{
    public class UEmptyRotatement : UnitMonoBehaviour, IRotatement
    {
        public void ForcedLookAt(Vector3 position)
        {

        }

        public void LookAt(Vector3 position)
        {

        }
    }
}
