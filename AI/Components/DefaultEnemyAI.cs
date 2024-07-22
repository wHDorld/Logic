using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Unit.Abstract;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using AssemblyCSharp.Assets.Logic.Player;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using AssemblyCSharp.Assets.Logic.AI.Entities;

namespace AssemblyCSharp.Assets.Logic.AI.Components
{
    public class DefaultEnemyAI : AIBehaviour
    {
        private IMovement movement;
        private IRotatement rotatement;

        public float AttackDistance = 2;
        public float SleepTime = 2;
        public float AttackAwareDelay = 0.5f;
        public bool AlwaysRotate = false;
        public Vector2 AttackInverval = new(1, 2);
        public Vector2 ChasingXOffset = new(-1, 1);
        public Vector2 ChasingZOffset = new(-1, 1);

        private void Start()
        {
            base.Start();

            movement = GetComponent<IMovement>() ?? null;
            rotatement = GetComponent<IRotatement>() ?? null;
        }

        private void Update()
        {
            if (StatContainer.IsDead)
            {
                CallAttackBreak();
                return;
            }
            base.Update();

            if (Player == null)
                return;

            if (chasing_timer > 0)
                chasing_timer -= Time.deltaTime;

            Rotating();
            if (PlayerDistance > AttackDistance)
            {
                CallAttackBreak();
                Chasing();
            }
            else
                Attack();
        }

        private void Rotating()
        {
            if (!AlwaysRotate)
            {
                if (attack_timer < AttackAwareDelay)
                    return;
                if (chasing_timer > 0)
                    return;
            }
            rotatement.LookAt(Player.transform.position);
        }

        private float chasing_timer = 0;
        private float chasingOffset_timer = 0;
        Vector3 chasingOffset;
        private void Chasing()
        {
            if (chasing_timer > 0)
                return;
            attack_timer = AttackAwareDelay + 0.05f;
            if (chasingOffset_timer > 0)
                chasingOffset_timer -= Time.deltaTime;
            else
            {
                chasingOffset_timer = Random.Range(1f, 3f);
                chasingOffset = new Vector3(
                    Random.Range(ChasingXOffset.x, ChasingXOffset.y),
                    0,
                    Random.Range(ChasingZOffset.x, ChasingZOffset.y)
                    ) * PlayerDistance;
            }
            movement.Move(-transform.position + Player.position + chasingOffset, false);
        }

        private float attack_timer = 0;
        private void Attack()
        {
            if (chasing_timer > 0)
                return;
            if (attack_timer > 0)
            {
                if (attack_timer <= AttackAwareDelay)
                    CallAwareAttack();
                attack_timer -= Time.deltaTime;
                return;
            }
            chasing_timer = SleepTime;
            attack_timer = Random.Range(AttackInverval.x, AttackInverval.y);
            StatContainer.HitTrigger = true;
            CallAttack();
        }
    }
}
