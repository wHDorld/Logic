using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    public class D_RandomSeed : DungeonBehaviour
    {
        public bool RandomSeedByTime;
        public int SeedPreset;

        public override IEnumerator Generate()
        {
            if (!SaveSystem.Singletone.SaveSystem.Load_Preparation().IsInDungeon)
            {
                if (RandomSeedByTime)
                    dataHandler.DungeonProperties.RandomSeed = (int)System.DateTime.Now.Ticks;
                else
                    dataHandler.DungeonProperties.RandomSeed = SeedPreset;
            }
            else
                Random.state = SaveSystem.Singletone.SaveSystem.Load_Preparation().RandomSeed;

            SaveSystem.Singletone.SaveSystem.Load_Preparation().RandomSeed = Random.state;
            return base.Generate();
        }
    }
}
