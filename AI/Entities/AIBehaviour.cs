using AssemblyCSharp.Assets.Logic.Minimap.Components;
using AssemblyCSharp.Assets.Logic.Player;
using AssemblyCSharp.Assets.Logic.Player.Abstract;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.AI.Entities
{
    [RequireComponent(typeof(ULife))]
    public class AIBehaviour : UnitMonoBehaviour
    {
        public Transform Player;
        public UnitStatContainer PlayerStatContainer;
        public bool AutoTarget = true;

        private bool IsPlayerDead;

        public void Start()
        {
            base.Start();
            FindPlayer();
        }

        public void FindPlayer()
        {
            if (IsPlayerDead)
                return;

            var plGo = GameObject.FindGameObjectsWithTag("Player")
                .Where(x => x.GetComponent<UnitStatContainer>() != null)
                .FirstOrDefault();
            if (plGo == null)
                return;

            Player = plGo.transform;
            PlayerStatContainer = Player.GetComponent<UnitStatContainer>();

            Player.GetComponent<ULife>().OnDied += AIBehaviour_OnPlayerDied;
            GetComponent<ULife>().OnDied += AIBehaviour_OnDied;

            if (AutoTarget)
                APlayerAttack.Enemies.Add(StatContainer);
            MinimapComponent.AddEnemy(StatContainer);
        }

        private void AIBehaviour_OnDied(DamageInfo info)
        {
            if (AutoTarget)
                APlayerAttack.Enemies.Remove(StatContainer);
            MinimapComponent.RemoveEnemy(StatContainer);
        }
        private void AIBehaviour_OnPlayerDied(DamageInfo info)
        {
            Player = null;
            IsPlayerDead = true;
        }

        public void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsPlayerDead)
                return;
            if (Player == null)
            {
                FindPlayer();
                return;
            }
            StatContainer.Aim = PlayerStatContainer.TargetPoint.position;
        }

        public float PlayerDistance
        {
            get
            {
                return Vector3.Distance(Player.position, transform.position);
            }
        }

        public delegate void AIDelegate();

        public event AIDelegate OnAwareAttack;
        public void CallAwareAttack()
        {
            OnAwareAttack?.Invoke();
        }
        public event AIDelegate OnAttack;
        public void CallAttack()
        {
            OnAttack?.Invoke();
        }
        public event AIDelegate OnAttackBreak;
        public void CallAttackBreak()
        {
            OnAttackBreak?.Invoke();
        }
    }
}
