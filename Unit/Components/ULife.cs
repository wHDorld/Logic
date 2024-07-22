using AssemblyCSharp.Assets.Logic.Unit.Entities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace AssemblyCSharp.Assets.Logic.Unit.Components
{
    public class ULife : UnitMonoBehaviour
    {
        public int MaxHealth;
        private int _currentHealth;

        private bool alreadyDead;

        public UnityEvent<DamageInfo> PreloadEventsOnDamaged;
        public UnityEvent<DamageInfo> PreloadEventsOnDied;

        [HideLabel, ShowInInspector]
        [ProgressBar(0, "MaxHealth")]
        public int Health
        {
            get
            {
                if (_currentHealth >= 0)
                    return _currentHealth;
                else
                    return 0;
            }
            set
            {
                if (_currentHealth <= 0)
                    return;

                _currentHealth = value;
            }
        }

        private void Start()
        {
            base.Start();

            _currentHealth = MaxHealth;
            OnDamaged += PreloadOnDamaged;
            OnDied += PreloadOnDied;
        }

        private void PreloadOnDamaged(DamageInfo info)
        {
            PreloadEventsOnDamaged?.Invoke(info);
        }
        private void PreloadOnDied(DamageInfo info)
        {
            PreloadEventsOnDied?.Invoke(info);
        }

        public void GetDamage(DamageInfo info)
        {
            if (alreadyDead)
                return;

            info.HealthBefore = Health;
            Health -= info.Damage;
            info.HealthAfter = Health;

            if (_currentHealth <= 0)
            {
                OnDied?.Invoke(info);
                alreadyDead = true;
            }
            OnDamaged?.Invoke(info);
        }

        public delegate void LifeDelegate(DamageInfo info);
        public event LifeDelegate OnDamaged;
        public event LifeDelegate OnDied;
    }
}
