using AssemblyCSharp.Assets.Logic.Unit.Entities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.Unit.Components
{
    [RequireComponent(typeof(ULife))]
    public class UOnHitColorChange : UnitMonoBehaviour
    {
        public string PropertyTag = "_Glow";
        public float FadingSpeed = 5f;
        public Renderer[] Renderers;

        ULife life;
        Material[] mats;
        float current_power = 0;
        private void Start()
        {
            base.Start();

            life = GetComponent<ULife>();
            mats = new Material[Renderers.Length];
            for (int i = 0; i < mats.Length; i++)
                mats[i] = Renderers[i].materials[0];

            life.OnDamaged += Life_OnDamaged;
        }

        private void Update()
        {
            if (StatContainer.IsDead)
            {
                current_power = 0;
                MaterialsUpdate();
                return;
            }
            base.Update();

            if (current_power > 0)
                current_power -= Time.deltaTime * FadingSpeed;
            MaterialsUpdate();
        }

        private void MaterialsUpdate()
        {
            foreach (var a in mats)
                a.SetFloat(PropertyTag, current_power);
        }

        private void Life_OnDamaged(DamageInfo info)
        {
            current_power = 1f;
        }
    }
}
