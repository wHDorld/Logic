using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using System.Collections;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    public class D_RescaleDungeon : DungeonBehaviour
    {
        public override IEnumerator Generate()
        {
            transform.localScale = Vector3.one * dataHandler.DungeonProperties.Preset.DungeonScaleFactor;
            yield return null;
        }
    }
}
