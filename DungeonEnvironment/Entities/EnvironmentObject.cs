using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Enums;
using AssemblyCSharp.Assets.Logic.DungeonEnvironment.Enums;
using Sirenix.OdinInspector;

namespace AssemblyCSharp.Assets.Logic.DungeonEnvironment.Entities
{
    [System.Serializable]
    public class EnvironmentObject
    {
        public string Name
        {
            get
            {
                if (current == null)
                    return "NUL";
                return current.name;
            }
        }

        [LabelText("$Name")]
        [PreviewField(Alignment = ObjectFieldAlignment.Right, FilterMode = FilterMode.Point, Height = 50)]
        public Object current;

        [FoldoutGroup("Properties")] public RoomType[] roomTypes;
        [FoldoutGroup("Properties")] public EnvironmentRequirement[] environmentRequirements;
        [FoldoutGroup("Properties")] public EnvironmentCorrection[] environmentCorrections;
        [FoldoutGroup("Properties")] [PropertyRange(0, 5)] public int SpaceRequired = 0;
        [FoldoutGroup("Properties")] public EnvironmentPass pass = EnvironmentPass.Main;
        
        [FoldoutGroup("Properties")]
        [PropertyRange(0, 100)]
        public int Chance = 50;
    }
}
