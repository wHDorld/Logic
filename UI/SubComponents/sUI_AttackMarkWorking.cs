using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.AI.Components;
using AssemblyCSharp.Assets.Logic.AI.Entities;
using System.Collections;
using UnityEngine.UI;

namespace AssemblyCSharp.Assets.Logic.UI.SubComponents
{
    public class sUI_AttackMarkWorking : MonoBehaviour
    {
        public AIBehaviour MyAI;

        private Image[] myImages;

        private void Awake()
        {
            MyAI = GetComponentInParent<AIBehaviour>();
            myImages = GetComponentsInChildren<Image>();
            MyAI.OnAwareAttack += MyAI_OnAwareAttack;
            MyAI.OnAttack += MyAI_OnAttack;
            MyAI.OnAttackBreak += MyAI_OnAttack;

            ImageSwitch(false);
        }

        private void MyAI_OnAttack()
        {
            ImageSwitch(false);
        }

        private void MyAI_OnAwareAttack()
        {
            ImageSwitch(true);
        }
        private void ImageSwitch(bool mark)
        {
            foreach (var a in myImages)
                a.enabled = mark;
        }
    }
}
