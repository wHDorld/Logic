using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using AssemblyCSharp.Assets.Logic.Unit.Components;

namespace AssemblyCSharp.Assets.Logic.WeaponSystem.Components
{
    public class ColliderHitHandlerComponents : MonoBehaviour
    {
        public int MaxTicksExists = 5;
        public float DamageMultiply = 1f;
        public bool OnlyOriginalCollisions = true;

        bool isReady = false;
        WeaponInfo weaponInfo;
        string EnemiesTags;
        WeaponHitComponent myHitComponent;

        public void Initialize(
            WeaponInfo weaponInfo,
            string enemyTag)
        {
            myHitComponent = GetComponentInParent<WeaponHitComponent>();
            EnemiesTags = enemyTag;
            this.weaponInfo = weaponInfo;
            isReady = true;
        }

        public void FixedUpdate()
        {
            if (!isReady)
                return;
            if (currentTicks >= MaxTicksExists)
                OnShutDown?.Invoke();
            currentTicks++;
        }

        List<ULife> already_collided = new List<ULife>();
        int currentTicks = 0;
        private void OnTriggerStay(Collider other)
        {
            if (!isReady)
                return;
            if (other.tag != EnemiesTags)
                return;
            if (!other.GetComponentInParent<ULife>())
                return;

            var current = other.GetComponentInParent<ULife>();
            if (already_collided.Contains(current))
                return;

            already_collided.Add(current);
            current.GetDamage(new DamageInfo(
                Mathf.RoundToInt(weaponInfo.DamageMultiply * DamageMultiply), 
                myHitComponent.transform.position)
                );
        }

        public delegate void ColliderHitDelegate();
        public event ColliderHitDelegate OnShutDown;
    }
}
