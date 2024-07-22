using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonEnvironment.Entities;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities;
using AssemblyCSharp.Assets.Logic.DungeonEnvironment.Enums;

namespace AssemblyCSharp.Assets.Logic.DungeonEnvironment.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Default Environment Collection", menuName = "Create Environment Collection", order = 0)]
    public class EnvironmentCollectionSO : ScriptableObject
    {
        public EnvironmentObject[] environmentObjects; 

        public EnvironmentObject[] GetAvaliableObjects(DungeonMapCell mapCell, EnvironmentPass pass)
        {
            var avaliable = environmentObjects
                .Where(x => x.pass == pass)
                .Where(x => x.SpaceRequired <= mapCell.SpaceSize)
                .Where(x => mapCell.IsCellCorrect(x.roomTypes))
                .Where(x => mapCell.IsCellCompliesRequirements(x.environmentRequirements));

            return avaliable.ToArray();
        }
    }
}
