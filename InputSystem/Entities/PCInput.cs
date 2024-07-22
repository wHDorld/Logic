using AssemblyCSharp.Assets.Logic.InputSystem.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.InputSystem.Entities
{
    public class PCInput : AInputEntity
    {
        public override void VectorsUpdate()
        {
            PlayerMovementVectorUpdate();
            CursorMovementVectorUpdate();
            CursorPositionVectorUpdate();
        }

        private void PlayerMovementVectorUpdate()
        {
            PlayerMovement = new UnityEngine.Vector3(
                GetValueByTag("Forward") - GetValueByTag("Backward"),
                GetValueByTag("Right") - GetValueByTag("Left"),
                0
                );
        }
        private void CursorMovementVectorUpdate()
        {
            CursorMovement = new UnityEngine.Vector3(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
                );
        }
        private void CursorPositionVectorUpdate()
        {
            CursorPosition = new UnityEngine.Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y
                );
        }
    }
}
