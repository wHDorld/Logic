using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AssemblyCSharp.Assets.Logic.Unit.Components
{
    public class UAnimatorController : UnitMonoBehaviour
    {
        public Animator animator;
        public float DefaultAnimationSpeed = 1f;

        [FoldoutGroup("Velocity")] public float VelocityMax = 6;
        [FoldoutGroup("Velocity")] public float VelocityScale = 1;
        [FoldoutGroup("Velocity")] public string VelocityX = "Velocity X";
        [FoldoutGroup("Velocity")] public string VelocityZ = "Velocity Z";

        [FoldoutGroup("Weapon")] public int Weapon = -1;
        [FoldoutGroup("Weapon")] public int AttackMaxNum = 10;

        [FoldoutGroup("Triggers")] public string InstantSwitchTrigger = "InstantSwitchTrigger";

        private void Start()
        {
            base.Start();
            animator ??= GetComponentInChildren<Animator>();
            animator.SetInteger("Weapon", Weapon);
            animator.SetTrigger(InstantSwitchTrigger);
            animator.speed = DefaultAnimationSpeed;


            StatContainer.OnHit += OnStatHit;
        }

        private void FixedUpdate()
        {
            base.FixedUpdate();
            VelocityUpdate();
        }

        Vector3 localDir;
        private void VelocityUpdate()
        {
            if (attackTiming > 0)
                attackTiming -= Time.deltaTime;
            else
                currentAttackNum = 1;

            Vector3 dir = StatContainer.Velocity * VelocityMax * VelocityScale;
            dir = transform.InverseTransformDirection(dir);
            localDir = Vector3.Lerp(localDir, dir, 15f * Time.deltaTime);

            animator.SetFloat(VelocityX, localDir.x);
            animator.SetFloat(VelocityZ, localDir.z);
            //animator.SetBool("Sprint", dir.magnitude > VelocityMax * 1.5f);
            animator.SetBool("Moving", localDir.magnitude > 0.05f);
        }

        #region EVENTS
        int currentAttackNum = 1;
        float attackTiming = 1;
        private void OnStatHit()
        {
            animator.SetBool("Moving", false);
            animator.SetInteger("Action", currentAttackNum);
            animator.SetTrigger("AttackTrigger");
            attackTiming = 1;
            currentAttackNum += 1;
            currentAttackNum = currentAttackNum > AttackMaxNum ? 1 : currentAttackNum;
        }
        #endregion
    }
}
