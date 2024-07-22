using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using AssemblyCSharp.Assets.Logic.Inventory.Enums;

namespace AssemblyCSharp.Assets.Logic.Inventory.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Default Item", menuName = "Create Item")]
    public class ItemSO : ScriptableObject
    {
        public string Name;
        [FoldoutGroup("Item")] public EItemType Type;
        [FoldoutGroup("Item")] public Object ReferedObject;

        [FoldoutGroup("Properties")] [MaxValue(7)] [MinValue(1)] public int Width;
        [FoldoutGroup("Properties")] [MinValue(1)] public int Height;

        [PreviewField(100, ObjectFieldAlignment.Left)]
        [FoldoutGroup("Properties")] public Sprite Image;

        public List<string> DefaultProperties = new List<string>() { "Default" };

        [MultiLineProperty()]
        public string Description;
    }
}
