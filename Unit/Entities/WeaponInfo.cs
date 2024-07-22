using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Unit.Entities
{
    [System.Serializable]
    public class WeaponInfo
    {
        public float DamageMultiply = 1f;
        public float AttackFrequency = 1;
        public float SlashDistance = 0.8f;
        public float InaccuracyAngle = 10f;
        public float Range = 10;

        public bool IsRange;
        [PreviewField] public Object AttackObject;
    }
}
