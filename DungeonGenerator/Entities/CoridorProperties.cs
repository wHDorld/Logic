using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Enums;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities
{
    [System.Serializable]
    public class CoridorProperties
    {
        public Object Coridor;
        public int Deep = -1;
        public ECoridorType Type;

        public bool IsAvailable
        {
            get
            {
                return SaveSystem.Singletone.SaveSystem.Load_Preparation().CurrentDeep >= Deep;
            }
        }
    }
}
