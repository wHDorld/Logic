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
    public class D_Sophistication : DungeonBehaviour
    {
        public override IEnumerator Generate()
        {
            yield return nearestGenerate();
            yield return roadsGenerate();
            yield return edgeGenerate();

            yield return roomsGenerate();
            yield return nearestGenerate();
            yield return roadsGenerate();
            yield return edgeGenerate();

            if (dataHandler.DungeonProperties.Preset.ClosingHoles)
            {
                yield return holeClosing();
                yield return nearestGenerate();
                yield return roadsGenerate();
                yield return edgeGenerate();
            }

            yield return roomExitsGenerate();
            yield return spaceSizeGenerate();

            if (dataHandler.DungeonProperties.Preset.ClosingDeadends)
            {
                yield return deadendsClosing();
                yield return nearestGenerate();
                yield return roadsGenerate();
                yield return edgeGenerate();
            }

            yield return wallsGenerate();
            yield return doorDirectionsGenerate();

            yield return null;
        }

        IEnumerator nearestGenerate()
        {
            Action<int, int, int, int> cell_checking = delegate (int x, int y, int xAdd, int yAdd)
            {
                if (x + xAdd >= 0 && x + xAdd < dataHandler.DungeonProperties.Size)
                    if (dataHandler.DungeonProperties.Dungeon[x + xAdd, y] != null)
                        dataHandler.DungeonProperties.Dungeon[x, y].NearCells.Add(dataHandler.DungeonProperties.Dungeon[x + xAdd, y]);
                if (y + yAdd >= 0 && y + yAdd < dataHandler.DungeonProperties.Size)
                    if (dataHandler.DungeonProperties.Dungeon[x, y + yAdd] != null)
                        dataHandler.DungeonProperties.Dungeon[x, y].NearCells.Add(dataHandler.DungeonProperties.Dungeon[x, y + yAdd]);
            };

            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                        continue;
                    dataHandler.DungeonProperties.Dungeon[x, y].NearCells.Clear();

                    cell_checking(x, y, 1, 1);
                    cell_checking(x, y, -1, -1);

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("nearestGenerate", (x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size));
                        yield return null;
                    }
                }
            dataHandler.DungeonProperties.DebugLog("nearestGenerate", 1);
            yield return null;
        }
        IEnumerator edgeGenerate()
        {
            Func<int, int, int, int, int> cell_checking = delegate (int x, int y, int xAdd, int yAdd)
            {
                if (x + xAdd >= 0 && x + xAdd < dataHandler.DungeonProperties.Size)
                    if (y + yAdd >= 0 && y + yAdd < dataHandler.DungeonProperties.Size)
                        if (dataHandler.DungeonProperties.Dungeon[x + xAdd, y + yAdd] == null
                        || dataHandler.DungeonProperties.Dungeon[x + xAdd, y + yAdd].IsLongCorridor
                        || dataHandler.DungeonProperties.Dungeon[x + xAdd, y + yAdd].IsCross)
                            return 1;
                return 0;
            };

            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                        continue;
                    if (dataHandler.DungeonProperties.Dungeon[x, y].IsLongCorridor)
                        continue;
                    if (dataHandler.DungeonProperties.Dungeon[x, y].IsCross)
                        continue;

                    int nearestsCount = 0;
                    for (int j = -1; j <= 1; j++)
                        for (int i = -1; i <= 1; i++)
                            nearestsCount += cell_checking(x, y, i, j);
                    dataHandler.DungeonProperties.Dungeon[x, y].IsEdge = nearestsCount > 0 && nearestsCount <= 6;


                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("edgeGenerate", (x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size));
                        yield return null;
                    }
                }
            dataHandler.DungeonProperties.DebugLog("edgeGenerate", 1);
            yield return null;
        }

        IEnumerator roadsGenerate()
        {
            Func<int, int, int, int, bool, int> cell_checking = delegate (int x, int y, int xAdd, int yAdd, bool passNearestCount)
            {
                if (x + xAdd >= 0 && x + xAdd < dataHandler.DungeonProperties.Size)
                    if (y + yAdd >= 0 && y + yAdd < dataHandler.DungeonProperties.Size)
                        if (dataHandler.DungeonProperties.Dungeon[x + xAdd, y + yAdd] != null
                            && (dataHandler.DungeonProperties.Dungeon[x + xAdd, y + yAdd].NearCells.Count > 2 || passNearestCount))
                            return 1;
                return 0;
            };

            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                        continue;

                    int nearestsCount = 0;
                    int clearNearestsCount = 0;
                    for (int j = -1; j <= 1; j += 2)
                        for (int i = -1; i <= 1; i += 2)
                        {
                            nearestsCount += cell_checking(x, y, i, j, false);
                            clearNearestsCount += cell_checking(x, y, i, j, true);
                        }

                    dataHandler.DungeonProperties.Dungeon[x, y].IsCross =
                        dataHandler.DungeonProperties.Dungeon[x, y].NearCells.Count >= 3
                        && nearestsCount == 0
                        && clearNearestsCount <= 2;

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("roadsGenerate", ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)) / 2f);
                        yield return null;
                    }
                }
            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                        continue;
                    dataHandler.DungeonProperties.Dungeon[x, y].IsLongCorridor =
                        dataHandler.DungeonProperties.Dungeon[x, y].NearCells.Count == 2
                        && dataHandler.DungeonProperties.Dungeon[x, y].NearCells.Any(x => x.NearCells.Count == 2 || x.IsCross || x.IsDeadend)

                        || dataHandler.DungeonProperties.Dungeon[x, y].NearCells.Any(x => x.IsCross);
                    dataHandler.DungeonProperties.Dungeon[x, y].IsDeadend = dataHandler.DungeonProperties.Dungeon[x, y].NearCells.Count == 1;

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("roadsGenerate", ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)) / 2f + 0.5f);
                        yield return null;
                    }
                }
            dataHandler.DungeonProperties.DebugLog("roadsGenerate", 1);
            yield return null;
        }

        IEnumerator roomsGenerate()
        {
            for (int y = 1; y < dataHandler.DungeonProperties.Size - 1; y++)
                for (int x = 1; x < dataHandler.DungeonProperties.Size - 1; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                        continue;
                    if (!dataHandler.DungeonProperties.Dungeon[x, y].IsLongCorridor)
                        continue;
                    if (UnityEngine.Random.value > dataHandler.DungeonProperties.Preset.RoomSpawnChance)
                        continue;

                    UnityEngine.Vector2Int norm = new UnityEngine.Vector2Int(
                        dataHandler.DungeonProperties.Dungeon[x + 1, y] == null ? 1 : 0,
                        dataHandler.DungeonProperties.Dungeon[x, y + 1] == null ? 1 : 0
                        );
                    norm *= UnityEngine.Random.value >= 0.5f ? -1 : 1;

                    if (norm.x * norm.y != 0) continue;

                    initiateRoom(norm, initiateHallway(x, y, norm));

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("roomsGenerate", ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)));
                        yield return null;
                    }
                }
            dataHandler.DungeonProperties.DebugLog("roomsGenerate", 1);
            yield return null;
        }
        UnityEngine.Vector2Int initiateHallway(int x, int y, UnityEngine.Vector2Int norm)
        {
            int addX = x;
            int addY = y;
            int i = 1;
            int rndHallwayLong = UnityEngine.Random.Range(0, dataHandler.DungeonProperties.Preset.HallwayLong);
            for (i = 1; i <= rndHallwayLong; i++)
            {
                addX = x + norm.x * i;
                addY = y + norm.y * i;

                if (addX < 0 || addX >= dataHandler.DungeonProperties.Size)
                    break;
                if (addY < 0 || addY >= dataHandler.DungeonProperties.Size)
                    break;
                if (dataHandler.DungeonProperties.Dungeon[addX, addY] != null)
                    break;

                dataHandler.DungeonProperties.Dungeon[addX, addY] = new DungeonMapCell(addX, addY);
            }

            if (i > 1)
                return new UnityEngine.Vector2Int(
                    addX + norm.x,
                    addY + norm.y);
            else
                return new UnityEngine.Vector2Int(
                    -1,
                    -1);
        }
        void initiateRoom(UnityEngine.Vector2Int norm, UnityEngine.Vector2Int start)
        {
            if (start.x < 0 || start.x >= dataHandler.DungeonProperties.Size) return;
            if (start.y < 0 || start.y >= dataHandler.DungeonProperties.Size) return;

            int width = 0;
            int roomSize = UnityEngine.Random.Range(2, dataHandler.DungeonProperties.Preset.RoomMaxSize + 1);

            if (norm.x != 0)
                for (int x = 1; x <= roomSize; x++)
                    if (x * norm.x + start.x >= 0 && x * norm.x + start.x < dataHandler.DungeonProperties.Size
                        &&
                        dataHandler.DungeonProperties.Dungeon[x * norm.x + start.x, start.y] == null)
                        width++;
                    else
                        break;
            if (norm.y != 0)
                for (int y = 1; y <= roomSize; y++)
                    if (y * norm.y + start.y >= 0 && y * norm.y + start.y < dataHandler.DungeonProperties.Size
                        &&
                        dataHandler.DungeonProperties.Dungeon[start.x, y * norm.y + start.y] == null)
                        width++;
                    else
                        break;

            width = width % 2 == 0 ? (width - 1) : width;

            if (width <= 1) return;
            int centerX = start.x + norm.x * width / 2;
            int centerY = start.y + norm.y * width / 2;

            for (int y = centerY - width / 2; y <= centerY + width / 2; y++)
            {
                if (y < 0 || y >= dataHandler.DungeonProperties.Size) continue;

                for (int x = centerX - width / 2; x <= centerX + width / 2; x++)
                {
                    if (x < 0 || x >= dataHandler.DungeonProperties.Size) continue;

                    dataHandler.DungeonProperties.Dungeon[x, y] = dataHandler.DungeonProperties.Dungeon[x, y] ?? new DungeonMapCell(x, y);
                    dataHandler.DungeonProperties.Dungeon[x, y].IsRoom = true;
                }
            }
        }

        IEnumerator spaceSizeGenerate()
        {
            Func<int, int, int, int, int> cell_checking = delegate (int x, int y, int xAdd, int yAdd)
            {
                if (x + xAdd >= 0 && x + xAdd < dataHandler.DungeonProperties.Size)
                    if (y + yAdd >= 0 && y + yAdd < dataHandler.DungeonProperties.Size)
                        if (dataHandler.DungeonProperties.Dungeon[x + xAdd, y + yAdd] != null)
                            return 1;
                return 0;
            };

            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                        continue;

                    for (int s = 1; s < dataHandler.DungeonProperties.Size; s++)
                    {
                        int nearestsCount = 0;
                        for (int j = -s; j <= s; j++)
                            for (int i = -s; i <= s; i++)
                                nearestsCount += cell_checking(x, y, i, j);

                        if (nearestsCount % UnityEngine.Mathf.Pow(s * 2 + 1, 2) == 0)
                            dataHandler.DungeonProperties.Dungeon[x, y].SpaceSize += 1;
                        else
                            break;
                    }

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("spaceSizeGenerate", ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)));
                        yield return null;
                    }
                }
            dataHandler.DungeonProperties.DebugLog("spaceSizeGenerate", 1);
            yield return null;
        }

        IEnumerator holeClosing()
        {
            bool[,] holes = new bool[dataHandler.DungeonProperties.Size, dataHandler.DungeonProperties.Size];
            Func<int, int, int, int, int> cell_checking = delegate (int x, int y, int xAdd, int yAdd)
            {
                if (x + xAdd >= 0 && x + xAdd < dataHandler.DungeonProperties.Size)
                    if (y + yAdd >= 0 && y + yAdd < dataHandler.DungeonProperties.Size)
                        if (dataHandler.DungeonProperties.Dungeon[x + xAdd, y + yAdd] != null
                            || holes[x + xAdd, y + yAdd])
                            return 1;
                return 0;
            };

            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] != null)
                        continue;
                    int nearestsCount = 0;
                    for (int j = -1; j <= 1; j++)
                        for (int i = -1; i <= 1; i++)
                            nearestsCount += cell_checking(x, y, i, j);

                    if (nearestsCount > 5)
                        holes[x, y] = true;

                    if ((x + y * dataHandler.DungeonProperties.Size) % (GeneratorTicks * 100) == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("holeClosing", ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)) / 2f);
                        yield return null;
                    }
                }
            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] != null)
                        continue;
                    if (!holes[x, y])
                        continue;
                    dataHandler.DungeonProperties.Dungeon[x, y] = new DungeonMapCell(x, y);

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("holeClosing", ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)) / 2f + 0.5f);
                        yield return null;
                    }
                }
            dataHandler.DungeonProperties.DebugLog("holeClosing", 1);
            yield return null;
        }

        IEnumerator roomExitsGenerate()
        {
            Func<int, int, int, int, int> cell_checking = delegate (int x, int y, int xAdd, int yAdd)
            {
                if (x + xAdd >= 0 && x + xAdd < dataHandler.DungeonProperties.Size)
                    if (y + yAdd >= 0 && y + yAdd < dataHandler.DungeonProperties.Size)
                        if (dataHandler.DungeonProperties.Dungeon[x + xAdd, y + yAdd] != null
                            && !dataHandler.DungeonProperties.Dungeon[x + xAdd, y + yAdd].IsRoom)
                            return 1;
                return 0;
            };

            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                        continue;
                    if (!dataHandler.DungeonProperties.Dungeon[x, y].IsRoom)
                        continue;
                    int nearestsCount = 0;

                    nearestsCount += cell_checking(x, y, 1, 0);
                    nearestsCount += cell_checking(x, y, -1, 0);
                    nearestsCount += cell_checking(x, y, 0, 1);
                    nearestsCount += cell_checking(x, y, 0, -1);

                    dataHandler.DungeonProperties.Dungeon[x, y].IsRoomExit = nearestsCount >= 1;

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("roomExitsGenerate", ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)));
                        yield return null;
                    }
                }
            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                        continue;
                    if (!dataHandler.DungeonProperties.Dungeon[x, y].IsRoomExit)
                        continue;

                    dataHandler.DungeonProperties.Dungeon[x, y].IsDoorhole = !dataHandler.DungeonProperties.Dungeon[x, y].NearCells.Any(x => x.IsRoomExit);

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("doorHolesGenerate", ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)));
                        yield return null;
                    }
                }
            dataHandler.DungeonProperties.DebugLog("roomExitsGenerate", 1);
            yield return null;
        }

        IEnumerator deadendsClosing()
        {
            bool isDeadendsExists = true;
            int deadendsClosed = 0;
            while (isDeadendsExists)
            {
                isDeadendsExists = false;
                for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                    for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                    {
                        if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                            continue;
                        if (!dataHandler.DungeonProperties.Dungeon[x, y].IsDeadend)
                            continue;

                        deadendsClosed += 1;
                        isDeadendsExists = true;
                        DungeonMapCell lastCell = dataHandler.DungeonProperties.Dungeon[x, y];
                        UnityEngine.Vector2Int lastCoords = lastCell.Position;

                        while (
                            lastCell != null
                            && (lastCell.IsLongCorridor || lastCell.IsDeadend)
                            && !lastCell.IsCross)
                        {
                            dataHandler.DungeonProperties.Dungeon[lastCell.PosX, lastCell.PosY] = null;
                            var bufferCell = lastCell
                                .NearCells.Where(x => x != null && x.Position != lastCoords)
                                .FirstOrDefault();
                            lastCoords = lastCell.Position;
                            lastCell = bufferCell;
                        }
                        if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                        {
                            dataHandler.DungeonProperties.DebugLog("deadendsClosed (" + deadendsClosed + ")", ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)));
                            yield return null;
                        }
                    }
            }
            dataHandler.DungeonProperties.DebugLog("deadendsClosing", 1);
            yield return null;
        }

        IEnumerator wallsGenerate()
        {
            Action<int, int, int, int> cell_checking = delegate (int x, int y, int xAdd, int yAdd)
            {
                if (x + xAdd >= 0 && x + xAdd < dataHandler.DungeonProperties.Size)
                    if (dataHandler.DungeonProperties.Dungeon[x + xAdd, y] == null)
                        dataHandler.DungeonProperties.Dungeon[x, y].Walls.Add(xAdd > 0 ? 0 : 2);
                if (y + yAdd >= 0 && y + yAdd < dataHandler.DungeonProperties.Size)
                    if (dataHandler.DungeonProperties.Dungeon[x, y + yAdd] == null)
                        dataHandler.DungeonProperties.Dungeon[x, y].Walls.Add(yAdd > 0 ? 1 : 3);

                if (x + xAdd < 0)
                    dataHandler.DungeonProperties.Dungeon[x, y].Walls.Add(2);
                if (x + xAdd >= dataHandler.DungeonProperties.Size)
                    dataHandler.DungeonProperties.Dungeon[x, y].Walls.Add(0);

                if (y + yAdd < 0)
                    dataHandler.DungeonProperties.Dungeon[x, y].Walls.Add(3);
                if (y + yAdd >= dataHandler.DungeonProperties.Size)
                    dataHandler.DungeonProperties.Dungeon[x, y].Walls.Add(1);
            };

            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                        continue;
                    cell_checking(x, y, 1, 1);
                    cell_checking(x, y, -1, -1);

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("wallsGenerate", ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)));
                        yield return null;
                    }
                }

            dataHandler.DungeonProperties.DebugLog("wallsGenerate", 1);
            yield return null;
        }
        IEnumerator doorDirectionsGenerate()
        {
            Action<int, int, int, int> cell_checking = delegate (int x, int y, int xAdd, int yAdd)
            {
                if (x + xAdd >= 0 && x + xAdd < dataHandler.DungeonProperties.Size)
                    if (dataHandler.DungeonProperties.Dungeon[x + xAdd, y] != null)
                        if (!dataHandler.DungeonProperties.Dungeon[x + xAdd, y].IsRoom)
                            dataHandler.DungeonProperties.Dungeon[x, y].DoorDirections.Add(xAdd > 0 ? 0 : 2);
                if (y + yAdd >= 0 && y + yAdd < dataHandler.DungeonProperties.Size)
                    if (dataHandler.DungeonProperties.Dungeon[x, y + yAdd] != null)
                        if (!dataHandler.DungeonProperties.Dungeon[x, y + yAdd].IsRoom)
                            dataHandler.DungeonProperties.Dungeon[x, y].DoorDirections.Add(yAdd > 0 ? 1 : 3);
            };

            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    if (dataHandler.DungeonProperties.Dungeon[x, y] == null)
                        continue;
                    if (!dataHandler.DungeonProperties.Dungeon[x, y].IsDoorhole)
                        continue;
                    cell_checking(x, y, 1, 1);
                    cell_checking(x, y, -1, -1);

                    if ((x + y * dataHandler.DungeonProperties.Size) % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("doorDirectionsGenerate", ((x + y * dataHandler.DungeonProperties.Size) / (float)(dataHandler.DungeonProperties.Size * dataHandler.DungeonProperties.Size)));
                        yield return null;
                    }
                }

            dataHandler.DungeonProperties.DebugLog("doorDirectionsGenerate", 1);
            yield return null;
        }
    }
}
