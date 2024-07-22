using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using AssemblyCSharp.Assets.Logic.DungeonEnvironment.ScriptableObjects;
using Sirenix.OdinInspector;
using System.Collections;
using AssemblyCSharp.Assets.Logic.DungeonEnvironment.Enums;
using AssemblyCSharp.Assets.Logic.DungeonEnvironment.Entities;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    [RequireComponent(typeof(D_CorridorPlacer))]
    public class D_EnvironmentPlacer : DungeonBehaviour
    {
        public EnvironmentProperties[] Environments;
        [DetailedInfoBox("Chances type", "" +
            "Previous - 0" +
            "\n\rMain - 1" +
            "\n\rPast - 2" +
            "\n\rAI - 3" +
            "\n\rUncollideble - 4" +
            "\r\nRenderLoad - 5")]
        public float[] SpawnChances = new float[6]
            {
                0.2f,
                0.2f,
                0.2f,
                0.05f,
                0.2f,
                0.1f
            };

        private D_CorridorPlacer _CorridorPlacer;

        public override IEnumerator Generate()
        {
            _CorridorPlacer = GetComponent<D_CorridorPlacer>();

            yield return GenerateEnvironment();
        }

        private IEnumerator GenerateEnvironment()
        {
            List<EnvironmentProperties> chosenEnvironments = new List<EnvironmentProperties>();
            chosenEnvironments.Add(Environments
                .Where(x => x.IsAvailable && x.Type == Enums.EEnvironventCollectionType.Default)
                .OrderBy(x => x.Deep)
                .LastOrDefault());
            chosenEnvironments.Add(Environments
                .Where(x => x.IsAvailable && x.Type == Enums.EEnvironventCollectionType.AI)
                .OrderBy(x => x.Deep)
                .LastOrDefault());
            chosenEnvironments.AddRange(Environments
                .Where(x => x.IsAvailable && x.Type == Enums.EEnvironventCollectionType.Additional));
            chosenEnvironments.RemoveAll(x => x == null);

            foreach (var env in chosenEnvironments)
            {
                for (int pass = 0; pass < 6; pass++)
                {
                    for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                        for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                        {
                            if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                                continue;
                            if (Random.value > SpawnChances[pass])
                                continue;
                            PlaceEnvironment(x, y, (EnvironmentPass)pass, env);


                            if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                            {
                                dataHandler.DungeonProperties.DebugLog(
                                    "environmentGenerate",
                                    ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)) / 6f + (pass / 6f));
                                yield return null;
                            }
                        }
                }
                dataHandler.DungeonProperties.DebugLog("environmentGenerate", 1);
            }
            yield return null;
        }

        private void PlaceEnvironment(int x, int y, EnvironmentPass pass, EnvironmentProperties currentProperties)
        {
            EnvironmentObject environmentObj = GetRandomEnvironmentObject(x, y, pass, currentProperties);
            if (environmentObj == null)
                return;

            GameObject g = Instantiate(environmentObj.current) as GameObject;
            g.transform.position = dataHandler.DungeonProperties.Dungeon[x, y].PositionWorld * dataHandler.DungeonProperties.Preset.DungeonCellScaleFactor
                + dataHandler.DungeonProperties.Preset.CoridorCenterNoScale;
            _CorridorPlacer.PerlinNoiseAffet(x, y, g);

            CorrectionEnvironment(x, y, g, environmentObj);
        }

        private void CorrectionEnvironment(int x, int y, GameObject envG, EnvironmentObject environmentObject)
        {
            if (environmentObject.environmentCorrections.Contains(EnvironmentCorrection.AlignToWalls)
                && dataHandler.DungeonProperties.Dungeon[x, y].Walls.Count != 0)
            {
                int rotation = dataHandler.DungeonProperties.Dungeon[x, y].Walls[Random.Range(0, dataHandler.DungeonProperties.Dungeon[x, y].Walls.Count)];
                envG.transform.rotation *= Quaternion.Euler(0, rotation * -90f, 0);
            }
            envG.transform.SetParent(transform, true);
        }

        private EnvironmentObject GetRandomEnvironmentObject(int x, int y, EnvironmentPass pass, EnvironmentProperties currentProperties)
        {
            var avaliableObjs = currentProperties.EnvironmentCollection.GetAvaliableObjects(dataHandler.DungeonProperties.Dungeon[x, y], pass)
                .OrderBy(x => x.Chance);

            if (avaliableObjs.Count() == 0)
                return null;

            List<int> chanceComp = new List<int>();
            foreach (var a in avaliableObjs)
                for (int i = 0; i < a.Chance; i++)
                    chanceComp.Add(a.Chance);
            int chosenGroup = chanceComp[Random.Range(0, chanceComp.Count)];

            avaliableObjs = avaliableObjs
                .Where(x => x.Chance == chosenGroup)
                .Select(x => x)
                .OrderBy(x => x.Chance);

            return avaliableObjs.ToList()[Random.Range(0, avaliableObjs.Count())];
        }
    }
}
