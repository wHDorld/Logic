using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Sirenix.OdinInspector;
using AssemblyCSharp.Assets.Logic.Unit.Entities;

namespace AssemblyCSharp.Assets.Logic.WeaponSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DefaultWeapon", menuName = "Create Weapon")]
    public class WeaponSO : ScriptableObject
    {
        public string Name;
        public WeaponInfo WeaponInfo;
        [PreviewField] public Object BowObject;
    }
}
