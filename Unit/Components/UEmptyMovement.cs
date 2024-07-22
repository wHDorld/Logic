using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using AssemblyCSharp.Assets.Logic.Unit.Abstract;

namespace AssemblyCSharp.Assets.Logic.Unit.Components
{
    public class UEmptyMovement : UnitMonoBehaviour, IMovement
    {
        public void ForcedMove(Vector3 direction, bool isRun)
        {

        }

        public void Move(Vector3 direction, bool isRun)
        {

        }
    }
}
