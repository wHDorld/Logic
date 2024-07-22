using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Unit.Components
{
    public class UAnimatorEventsHandler : MonoBehaviour
    {
        public void Hit()
        {
            OnHit?.Invoke();
        }
        public void Shoot()
        {
            OnShoot?.Invoke();
        }
        public void FootR()
        {
            OnFootR?.Invoke();
        }
        public void FootL()
        {
            OnFootL?.Invoke();
        }
        public void Land()
        {
            OnLand?.Invoke();
        }
        public void WeaponSwitch()
        {
            OnWeaponSwitch?.Invoke();
        }

        public delegate void UnitAnimationEvent();

        public event UnitAnimationEvent OnHit;
        public event UnitAnimationEvent OnShoot;
        public event UnitAnimationEvent OnFootR;
        public event UnitAnimationEvent OnFootL;
        public event UnitAnimationEvent OnLand;
        public event UnitAnimationEvent OnWeaponSwitch;
    }
}
