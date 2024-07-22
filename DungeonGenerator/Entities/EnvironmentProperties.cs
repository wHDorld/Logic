using AssemblyCSharp.Assets.Logic.DungeonEnvironment.ScriptableObjects;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities
{
    [System.Serializable]
    public class EnvironmentProperties
    {
        public EnvironmentCollectionSO EnvironmentCollection;
        public int Deep = -1;
        public EEnvironventCollectionType Type;

        public bool IsAvailable
        {
            get
            {
                return SaveSystem.Singletone.SaveSystem.Load_Preparation().CurrentDeep >= Deep;
            }
        }
    }
}
