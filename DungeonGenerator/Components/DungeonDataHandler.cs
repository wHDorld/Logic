using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities;
using Sirenix.OdinInspector;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    public class DungeonDataHandler : MonoBehaviour
    {
        public DungeonPreset dungeonPreset;
        public LevelPresets levelPresets;
        public DungeonVisualPreset visualPreset;

        public static DungeonProperties _dungeonProperties;
        public DungeonProperties DungeonProperties {
            get
            {
                _dungeonProperties ??= new DungeonProperties(dungeonPreset, levelPresets, visualPreset);
                return _dungeonProperties;
            }
        }

        private void Start()
        {

        }
    }
}
