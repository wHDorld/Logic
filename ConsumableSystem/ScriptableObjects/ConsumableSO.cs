using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.ConsumableSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DefaultConsumable", menuName = "Create Consumable")]
    public class ConsumableSO : ScriptableObject
    {
        public string EffectText;
    }
}
