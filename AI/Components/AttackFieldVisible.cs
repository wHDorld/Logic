using AssemblyCSharp.Assets.Logic.AI.Entities;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.AI.Components
{
    [RequireComponent(typeof(AIBehaviour))]
    [RequireComponent(typeof(UCombatController))]
    public class AttackFieldVisible : MonoBehaviour
    {
        AIBehaviour myAI;
        UCombatController myCC;

        private void Start()
        {
            myAI = GetComponent<AIBehaviour>();
            myCC = GetComponent<UCombatController>();

            myAI.OnAwareAttack += MyAI_OnAwareAttack;
            myAI.OnAttackBreak += MyAI_OnAttackBreak;
            myAI.OnAttack += MyAI_OnAttackBreak;
        }

        private void MyAI_OnAttackBreak()
        {
            Destroy(currentAttackObject);
        }

        GameObject currentAttackObject;
        private void MyAI_OnAwareAttack()
        {
            MyAI_OnAttackBreak();
            currentAttackObject = Instantiate(myCC.weaponInfo.AttackObject) as GameObject;
            currentAttackObject.transform.position = transform.position;
            currentAttackObject.transform.rotation = transform.rotation;
            currentAttackObject.transform.SetParent(gameObject.transform, true);
        }
    }
}
