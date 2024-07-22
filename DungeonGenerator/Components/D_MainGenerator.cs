using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using System.Collections;
using System;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    public class D_MainGenerator : DungeonBehaviour
    {
        public override IEnumerator Generate()
        {
            yield return arrayGenerate(dataHandler.DungeonProperties.Preset.DungeonDeep);
        }

        IEnumerator arrayGenerate(int deep)
        {
            int dir = 0;
            Func<int, int> validate_value = delegate (int t)
            {
                if (t >= dataHandler.DungeonProperties.Size || t < 0)
                    dir += UnityEngine.Random.value > 0.5f ? 1 : 3;
                t = t >= dataHandler.DungeonProperties.Size ? (dataHandler.DungeonProperties.Size - 1) : (t < 0 ? 0 : t);
                dir = dir > 3 ? dir - 4 : dir;
                return t;
            };

            int x = dataHandler.DungeonProperties.Size / 2;
            int y = dataHandler.DungeonProperties.Size / 2;
            int length = 1;

            for (int i = 0; i < deep; i++)
            {
                dataHandler.DungeonProperties.Dungeon[x, y] = dataHandler.DungeonProperties.Dungeon[x, y] ?? new DungeonMapCell(x, y);
                if (i % length == 0)
                {
                    length = UnityEngine.Random.Range(dataHandler.DungeonProperties.Preset.MinimumRoadLenght, dataHandler.DungeonProperties.Preset.MaximumRoadLenght);
                    dir = UnityEngine.Random.Range(0, 4);
                }

                switch (dir)
                {
                    case 0:
                        x++;
                        x = validate_value(x);
                        break;
                    case 1:
                        y++;
                        y = validate_value(y);
                        break;
                    case 2:
                        x--;
                        x = validate_value(x);
                        break;
                    case 3:
                        y--;
                        y = validate_value(y);
                        break;
                }

                if (i % 5000 == 0)
                {
                    dataHandler.DungeonProperties.DebugLog("arrayGenerate", i / (float)deep);
                    yield return null;
                }
            }
            dataHandler.DungeonProperties.DebugLog("arrayGenerate", 1);
            yield return null;
        }
    }
}
