using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using System.Collections;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities;
using System;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    public class D_CorridorPlacer : DungeonBehaviour
    {
        private DungeonMapCell[,] cells;
        private float _objectsScale;
        private Transform _parent;
        private DungeonVisualPreset _visualPreset;

        public override IEnumerator Generate()
        {
            _visualPreset = dataHandler.DungeonProperties.visualPreset;
            _objectsScale = dataHandler.DungeonProperties.Preset.DungeonCellScaleFactor;
            _parent = transform;

            yield return DungeonVizualize();
            yield return null;
        }

        public IEnumerator DungeonVizualize()
        {
            cells = dataHandler.DungeonProperties.Dungeon;
            DestroyObjects();
            yield return instantiateObjects(dataHandler.DungeonProperties.levelPresets);
            yield return setUpObjects();
            yield return null;
        }

        public void DestroyObjects()
        {
            for (int y = 0; y < dataHandler.DungeonProperties.DungeonMapCellGameObjects.GetLength(1); y++)
                for (int x = 0; x < dataHandler.DungeonProperties.DungeonMapCellGameObjects.GetLength(0); x++)
                    UnityEngine.Object.Destroy(dataHandler.DungeonProperties.DungeonMapCellGameObjects[x, y]);
        }

        IEnumerator instantiateObjects(
            LevelPresets presets
            )
        {
            List<CoridorProperties> coridors = new List<CoridorProperties>();
            coridors.Add(presets.Coridors
                .Where(x => x.IsAvailable && x.Type == Enums.ECoridorType.Default)
                .OrderBy(x => x.Deep)
                .LastOrDefault());
            coridors.AddRange(presets.Coridors
                .Where(x => x.IsAvailable && x.Type == Enums.ECoridorType.Additional));
            coridors.RemoveAll(x => x == null);

            var currentCoridor = coridors[UnityEngine.Random.Range(0, coridors.Count)];
            for (int y = 0; y < cells.GetLength(1); y++)
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    if (cells[x, y] == null)
                        continue;

                    GameObject g = UnityEngine.Object.Instantiate(currentCoridor.Coridor) as GameObject;
                    g.transform.SetParent(_parent);
                    g.transform.position = new Vector3(x, 0, y) * _objectsScale;

                    PerlinNoiseAffet(x, y, g);

                    dataHandler.DungeonProperties.DungeonMapCellGameObjects[x, y] = g;

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("corridorsGenerate", (x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size));
                        yield return null;
                    }
                }
            dataHandler.DungeonProperties.DebugLog("corridorsGenerate", 1);
        }
        public void PerlinNoiseAffet(int x, int y, GameObject g)
        {
            float pNoise = Mathf.PerlinNoise(x / _visualPreset.PerlinNoiseScale, y / _visualPreset.PerlinNoiseScale);

            g.transform.position = g.transform.position
                + new Vector3(0, pNoise, 0) * (_objectsScale * _visualPreset.NoiseHeightImpact)
                + new Vector3(pNoise, 0, pNoise) * (_objectsScale * _visualPreset.NoiseOffsetImpact);
            g.transform.localScale *= 1f + pNoise * _visualPreset.NoiseScaleImpact;
            g.transform.rotation *= Quaternion.Euler(0, (pNoise - 0.5f) * _visualPreset.NoiseYRotationImpact, 0);
            g.transform.rotation *= Quaternion.Euler(new Vector3((pNoise - 0.5f), 0, (pNoise - 0.5f)) * _visualPreset.NoiseXZRotationImpact);
        }
        IEnumerator setUpObjects()
        {
            Action<int, IEnumerable<Transform>, int, int> wall_activate = delegate (int i, IEnumerable<Transform> walls, int x, int y)
            {
                if (!cells[x, y].Walls.Contains(i))
                    return;
                var walls_c = walls.Where(x => x.name.Contains("r" + i)).ToArray();
                GameObject g = walls_c[UnityEngine.Random.Range(0, walls_c.Count())].gameObject;
                g.SetActive(!g.activeSelf);
            };

            for (int y = 0; y < cells.GetLength(1); y++)
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    if (cells[x, y] == null)
                        continue;

                    wall_working(x, y, "walls");

                    wall_working(x, y, "blackSpaces");

                    floors_working(x, y);

                    doors_working(x, y);

                    stairs_working(x, y);

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("corridorSettingsGenerate", (x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size));
                        yield return null;
                    }
                }
            dataHandler.DungeonProperties.DebugLog("corridorSettingsGenerate", 1);
        }

        void wall_working(int x, int y, string key)
        {
            Action<int, IEnumerable<Transform>, int, int> wall_activate = delegate (int i, IEnumerable<Transform> walls, int x, int y)
            {
                if (!cells[x, y].Walls.Contains(i))
                    return;
                var walls_c = walls.Where(x => x.name.Contains("r" + i)).ToArray();
                GameObject g = walls_c[UnityEngine.Random.Range(0, walls_c.Count())].gameObject;
                g.SetActive(!g.activeSelf);

                if (i > 1)
                {
                    //g.transform.localScale = new Vector3(g.transform.localScale.x, g.transform.localScale.y * 0.2f, g.transform.localScale.z);
                    if (g.GetComponent<Renderer>())
                        g.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                    foreach (var a in g.GetComponentsInChildren<Renderer>())
                        a.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            };

            var walls = findAvaliableObjects(
                        dataHandler.DungeonProperties.DungeonMapCellGameObjects[x, y].GetComponentsInChildren<Transform>(true),
                        cells[x, y],
                        key);

            wall_activate(0, walls, x, y);
            wall_activate(1, walls, x, y);
            wall_activate(2, walls, x, y);
            wall_activate(3, walls, x, y);
        }
        void floors_working(int x, int y)
        {
            var floors = findAvaliableObjects(
                dataHandler.DungeonProperties.DungeonMapCellGameObjects[x, y].GetComponentsInChildren<Transform>(true),
                cells[x, y],
                "floors")
                .Where(x => x.name.Contains("Floor"))
                .ToArray();
            var currentFloor = floors[UnityEngine.Random.Range(0, floors.Length)];
            currentFloor.gameObject.SetActive(true);
            //currentFloor.rotation *= Quaternion.Euler(0, Mathf.RoundToInt(UnityEngine.Random.Range(0, 4)) * 90f, 0);
        }
        void doors_working(int x, int y)
        {
            if (!cells[x, y].IsDoorhole)
                return;
            Action<int, IEnumerable<Transform>, int, int> door_activate = delegate (int i, IEnumerable<Transform> doors, int x, int y)
            {
                if (!cells[x, y].DoorDirections.Contains(i))
                    return;
                var door_c = doors.Where(x => x.name.Contains("r" + i)).ToArray();
                GameObject g = door_c[UnityEngine.Random.Range(0, door_c.Count())].gameObject;
                g.SetActive(!g.activeSelf);
            };

            var doors = findAvaliableObjects(
                        dataHandler.DungeonProperties.DungeonMapCellGameObjects[x, y].GetComponentsInChildren<Transform>(true),
                        cells[x, y],
                        "doors");

            door_activate(0, doors, x, y);
            door_activate(1, doors, x, y);
            door_activate(2, doors, x, y);
            door_activate(3, doors, x, y);
        }
        void stairs_working(int x, int y)
        {
            Action<int, IEnumerable<Transform>, int, int> stairs_activate = delegate (int i, IEnumerable<Transform> stairs, int x, int y)
            {
                if (cells[x, y].Walls.Contains(i))
                    return;
                var stairs_c = stairs.Where(x => x.name.Contains("r" + i)).ToArray();
                GameObject g = stairs_c[UnityEngine.Random.Range(0, stairs_c.Count())].gameObject;

                int corner = (i - 1);
                corner = corner < 0 ? 3 : corner;
                if (cells[x, y].Walls.Contains(corner))
                {
                    g.GetComponentsInChildren<Transform>(true)
                        .Where(x => x.name == "corner")
                        .FirstOrDefault()
                        .gameObject.SetActive(false);
                }

                g.SetActive(true);
            };

            var stairs = findAvaliableObjects(
                        dataHandler.DungeonProperties.DungeonMapCellGameObjects[x, y].GetComponentsInChildren<Transform>(true),
                        cells[x, y],
                        "stairs");

            stairs_activate(0, stairs, x, y);
            stairs_activate(1, stairs, x, y);
            stairs_activate(2, stairs, x, y);
            stairs_activate(3, stairs, x, y);
        }

        IEnumerable<Transform> findAvaliableObjects(Transform[] child, DungeonMapCell cell, string code)
        {
            var conditionalNames = cell.ConditionNames;
            var objects = child
                .Where(x => x.name.Contains(code))
                .FirstOrDefault()
                .GetComponentsInChildren<Transform>(true) as IEnumerable<Transform>;

            var mess = objects
                .Where(x => x.name.Contains("Mess"))
                .FirstOrDefault()
                .GetComponentsInChildren<Transform>(true);

            if (cell.IsMess)
                return mess;

            objects = objects
                .Where(x => conditionalNames.Contains(x.name));

            if (objects.Count() == 0)
                return mess;

            return objects
                .FirstOrDefault()
                .GetComponentsInChildren<Transform>(true);
        }

    }
}
