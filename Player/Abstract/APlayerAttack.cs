using AssemblyCSharp.Assets.Logic.Unit.Components;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AssemblyCSharp.Assets.Logic.Player.Abstract
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(UCombatController))]
    public class APlayerAttack : UnitMonoBehaviour
    {
        public DecalProjector CircleDecal;
        public DecalProjector EnemyCircleDecal;
        public DecalProjector LineDecal;

        public static List<UnitStatContainer> Enemies = new List<UnitStatContainer>();
        public PlayerController playerController;
        public UCombatController combatController;

        private void FixedUpdate()
        {
            DecalWorking();
        }

        void DecalWorking()
        {
            var cSize = getCurrentCircleSize;

            CircleDecal.size = Vector3.Lerp(
                CircleDecal.size,
                cSize,
                15f * Time.fixedDeltaTime
                );

            if (CurrentEnemy == null)
            {
                EnemyCircleDecal.size = Vector3.zero;

                LineDecal.size = new Vector3(0.2f, CircleDecal.size.y / 2f, 10);
                LineDecal.pivot = new Vector3(0, CircleDecal.size.y / 4f, 0);
            }
            else
            {
                EnemyCircleDecal.size = new Vector3(1, 1, 1);
                EnemyCircleDecal.transform.position = CurrentEnemy.transform.position + new Vector3(0, .1f, 0);

                float dist = Vector3.Distance(transform.position, CurrentEnemy.transform.position);
                LineDecal.size = new Vector3(0.2f, dist, 10);
                LineDecal.pivot = new Vector3(0, dist / 2f, 0);
            }
        }

        public UnitStatContainer CurrentEnemy;
        public UnitStatContainer getNearestEnemy
        {
            get
            {
                int num = -1;
                float distance = getCurrentAttackRange;
                for (int i = 0; i < Enemies.Count; i++)
                {
                    float d = Vector3.Distance(transform.position, Enemies[i].transform.position);
                    if (d > distance)
                        continue;
                    num = i;
                    distance = d;
                }
                if (num == -1)
                    return null;

                return Enemies[num];
            }
        }

        public float getCurrentAttackRange
        {
            get
            {
                return combatController.weaponInfo.Range * (StatContainer.Velocity.magnitude + 1);
            }
        }
        public Vector3 getCurrentCircleSize
        {
            get
            {
                return Vector3.one * getCurrentAttackRange * 2 + new Vector3(0, 0, 9);
            }
        }
    }
}
