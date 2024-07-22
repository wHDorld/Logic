using AssemblyCSharp.Assets.Logic.Unit.Abstract;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Unit.Components
{
    [RequireComponent(typeof(CharacterController))]
    public class UCharacterControllerRotatement : UnitMonoBehaviour, IRotatement
    {
        private CharacterController _characterController;

        private void Start()
        {
            base.Start();
            _characterController = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            base.FixedUpdate();
            LookAtUpdate();
        }

        public void LookAt(Vector3 position)
        {
            forced = false;
            lastPosition = position;
            wasCalled = true;
        }
        public void ForcedLookAt(Vector3 position)
        {
            forced = true;
            lastPosition = position;
            wasCalled = true;
        }

        bool forced = false;
        bool wasCalled = false;
        Vector3 lastPosition;
        void LookAtUpdate()
        {
            if (!wasCalled)
                return;
            if (StatContainer.IsHit && !forced)
                return;
            wasCalled = false;

            transform.transform.rotation = Quaternion.Lerp(
                transform.transform.rotation,
                Quaternion.LookRotation(lastPosition - transform.position),
                StatContainer.rotatement.LerpSpeed * Time.fixedDeltaTime
                );
            transform.transform.rotation = Quaternion.Euler(
                StatContainer.rotatement.LockX ? 0 : transform.transform.eulerAngles.x,
                StatContainer.rotatement.LockY ? 0 : transform.transform.eulerAngles.y,
                StatContainer.rotatement.LockZ ? 0 : transform.transform.eulerAngles.z
                );
        }
    }
}
