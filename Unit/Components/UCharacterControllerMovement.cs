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
    public class UCharacterControllerMovement : UnitMonoBehaviour, IMovement
    {
        public bool HitCheck = true;
        private CharacterController _characterController;

        private void Start()
        {
            base.Start();
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            base.Update();
            if (!isMoved)
            {
                velocity = Vector3.Lerp(velocity, Vector3.zero, (1f - StatContainer.movement.InertiaAffect) * StatContainer.movement.LerpSpeed * Time.deltaTime);
            }
            isMoved = false;

            MoveUpdate();
        }

        private void FixedUpdate()
        {
            base.FixedUpdate();

            Vector3 grav = StatContainer.IsOnGround ? Vector3.zero : Physics.gravity * StatContainer.GravityAffect;
            _characterController.Move(velocity + grav);
        }


        bool wasCalled = false;
        Vector3 lastDirection;
        bool lastRun = false;
        public void Move(Vector3 direction, bool isRun)
        {
            wasCalled = true;
            lastDirection = direction;
            lastRun = isRun;
        }


        bool isMoved = false;
        Vector3 velocity;
        private void MoveUpdate()
        {
            if (!wasCalled)
                return;
            if (HitCheck && StatContainer.IsHit)
                return;
            wasCalled = false;
            AbsoluteMove(lastDirection, lastRun);
        }
        public void ForcedMove(Vector3 direction, bool isRun)
        {
            isMoved = direction.magnitude > 0.05f;
            if (!isMoved)
                return;

            velocity = Vector3.Lerp(velocity, direction, StatContainer.movement.LerpSpeed * Time.deltaTime);
        }

        private void AbsoluteMove(Vector3 direction, bool isRun)
        {
            isMoved = direction.magnitude > 0.05f;
            if (!isMoved)
                return;

            float run = isRun ? StatContainer.movement.RunMultiply : 1;
            direction = direction.normalized;
            direction *= StatContainer.movement.MoveSpeed * run;
            velocity = Vector3.Lerp(velocity, direction, StatContainer.movement.LerpSpeed * Time.deltaTime);
        }
    }
}
