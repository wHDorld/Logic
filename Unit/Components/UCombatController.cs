using AssemblyCSharp.Assets.Logic.Unit.Abstract;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Sirenix.OdinInspector;
using AssemblyCSharp.Assets.Logic.WeaponSystem.ScriptableObjects;
using AssemblyCSharp.Assets.Logic.WeaponSystem.Components;

namespace AssemblyCSharp.Assets.Logic.Unit.Components
{
    public class UCombatController : UnitMonoBehaviour
    {
        public bool IsPreset = true;
        public WeaponInfo weaponInfo = new WeaponInfo();
        public Object CurrentArrow;
        public WeaponSO CurrentWeapon;

        public string EnemiesTags;
        private IMovement movement;

        private void Start()
        {
            base.Start();

            if (!IsPreset)
                ChangeWeapon(CurrentWeapon);

            if (GetComponent<IMovement>() != null)
                movement = GetComponent<IMovement>();
        }

        private GameObject weaponObject;
        public void ChangeWeapon(WeaponSO weapon)
        {
            CurrentWeapon = weapon;
            if (weaponObject != null)
                Destroy(weaponObject);
            weaponObject = GameObject.Instantiate(weapon.BowObject) as GameObject;
            weaponObject.transform.SetParent(StatContainer.WeaponHandler, true);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localEulerAngles = Vector3.zero;
            weaponObject.transform.localScale = Vector3.one;

            weaponInfo = weapon.WeaponInfo;
        }
        public void ChangeArrow(Object arrow)
        {
            CurrentArrow = arrow;
        }

        GameObject currentAttackObject;
        private void PhysicalHit()
        {
            if (currentAttackObject == null)
                currentAttackObject = Instantiate(weaponInfo.AttackObject) as GameObject;
            currentAttackObject.transform.position = transform.position;
            currentAttackObject.transform.rotation = transform.rotation;
            currentAttackObject.GetComponent<WeaponHitComponent>().Initialize(weaponInfo, EnemiesTags);
            currentAttackObject = null;
        }
        private void RangeHit()
        {
            GameObject arrow = Instantiate(CurrentArrow) as GameObject;

            arrow.transform.position = transform.position + new Vector3(0, 1f, 0);
            
            arrow.transform.rotation = Quaternion.LookRotation(transform.forward);
            arrow.transform.rotation = Quaternion.LookRotation(
                arrow.transform.forward * Vector3.Distance(arrow.transform.position, StatContainer.Aim)
                + new Vector3(0, StatContainer.Aim.y - arrow.transform.position.y, 0)
                );
            arrow.transform.rotation *= Quaternion.Euler(
                Random.Range(-weaponInfo.InaccuracyAngle, weaponInfo.InaccuracyAngle),
                Random.Range(-weaponInfo.InaccuracyAngle, weaponInfo.InaccuracyAngle),
                Random.Range(-weaponInfo.InaccuracyAngle, weaponInfo.InaccuracyAngle)
                );
            arrow.GetComponent<ArrowComponent>().Initialize(weaponInfo, EnemiesTags);
        }

        private void Update()
        {
            base.Update();
            HitUpdate();
        }

        public void OnHitTrigger()
        {
            movement?.ForcedMove(transform.forward * weaponInfo.SlashDistance, false);
            if (weaponInfo.IsRange)
                RangeHit();
            else
                PhysicalHit();
        }

        float hitSaveTimer = 1;
        private void HitUpdate()
        {
            if (hitSaveTimer <= 0)
            {
                StatContainer.IsHit = false;
            }
            else
            {
                hitSaveTimer -= Time.deltaTime;
                return;
            }

            if (!StatContainer.HitTrigger)
                return;

            hitSaveTimer = 1f / weaponInfo.AttackFrequency;
            OnHitEvent();
        }

        private void OnHitEvent()
        {
            StatContainer.IsHit = StatContainer.HitTrigger;
            StatContainer.HitTrigger = false;
            OnHitTrigger();
            StatContainer.Call_OnHit();
        }
    }
}
