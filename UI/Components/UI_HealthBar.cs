using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using AssemblyCSharp.Assets.Logic.Unit.Entities;

namespace AssemblyCSharp.Assets.Logic.UI.Components
{
    public class UI_HealthBar : MonoBehaviour
    {
        public RectTransform HP_main;
        public RectTransform HP_flow;

        public ULife uLife;
        Vector2 originalHpSize;
        Vector2 originalHpFlowSize;
        private void Start()
        {
            uLife.OnDamaged += ULife_OnDamaged;
            HP_main.localScale = Vector3.one;
            HP_flow.localScale = Vector3.one;
            originalHpSize = HP_main.sizeDelta;
            originalHpFlowSize = HP_flow.sizeDelta;
        }

        private void ULife_OnDamaged(DamageInfo info)
        {
            flow_timer = 1;
        }

        float flow_timer = 1;
        private void FixedUpdate()
        {
            HP_main.sizeDelta = new Vector2(
                uLife.Health / (float)uLife.MaxHealth * originalHpSize.x,
                originalHpSize.y
                );

            if (flow_timer <= 0)
                HP_flow.sizeDelta = Vector2.Lerp(HP_flow.sizeDelta, HP_main.sizeDelta, 8f * Time.deltaTime);
            else
                flow_timer -= Time.deltaTime;
        }
    }
}
