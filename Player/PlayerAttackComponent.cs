using AssemblyCSharp.Assets.Logic.Player.Abstract;
using AssemblyCSharp.Assets.Logic.Unit.Abstract;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AssemblyCSharp.Assets.Logic.Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(IMovement))]
    [RequireComponent(typeof(IRotatement))]
    public class PlayerAttackComponent : APlayerAttack
    {
        private IMovement movement;
        private IRotatement rotatement;
        private UnitMonoBehaviour playerUnit;

        private void Start()
        {
            base.Start();
            playerController = GetComponent<PlayerController>();
            movement = GetComponent<IMovement>();
            rotatement = GetComponent<IRotatement>();
            playerUnit = GetComponent<UnitMonoBehaviour>();
            combatController = GetComponent<UCombatController>();

            StartCoroutine(Shooting());
        }

        IEnumerator Shooting()
        {
            while (true)
            {
                while (playerController.IsInventoryOpen)
                {
                    yield return null;
                }

                while (playerController.IsMoving)
                {
                    CurrentEnemy = getNearestEnemy;
                    yield return null;
                }

                CurrentEnemy = getNearestEnemy;
                while (CurrentEnemy == null)
                {
                    CurrentEnemy = getNearestEnemy;
                    yield return new WaitForSeconds((1f / combatController.weaponInfo.AttackFrequency) * 0.8f);
                }

                StatContainer.Aim = CurrentEnemy.TargetPoint.position;
                rotatement.ForcedLookAt(CurrentEnemy.transform.position);

                while (playerController.IsEvading)
                {
                    yield return null;
                }

                playerUnit.StatContainer.HitTrigger = true;
                yield return new WaitForSeconds((1f / combatController.weaponInfo.AttackFrequency) * 0.8f);
            }
        }
    }
}
