using AssemblyCSharp.Assets.Logic.DungeonEnvironment.Enums;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities
{
    public class DungeonMapCell
    {
        public int PosX;
        public int PosY;

        public int SpaceSize;

        public List<DungeonMapCell> NearCells;
        public List<int> Walls;
        public List<int> DoorDirections;
        public List<RoomType> Types;

        public DungeonMapCell(int x, int y)
        {
            PosX = x;
            PosY = y;
            NearCells = new List<DungeonMapCell>();
            Walls = new List<int>();
            Types = new List<RoomType>();
            DoorDirections = new List<int>();
        }

        public bool IsMess
        {
            get
            {
                return !IsLongCorridor
                    && !IsRoom
                    && !IsCross
                    && !IsDeadend;
            }
        }
        public bool IsLongCorridor
        {
            set
            {
                if (value && !Types.Contains(RoomType.IsLongCorridor))
                    Types.Add(RoomType.IsLongCorridor);
                if (!value && Types.Contains(RoomType.IsLongCorridor))
                    Types.Remove(RoomType.IsLongCorridor);
            }
            get
            {
                return Types.Contains(RoomType.IsLongCorridor);
            }
        }
        public bool IsRoom
        {
            set
            {
                if (value && !Types.Contains(RoomType.IsRoom))
                    Types.Add(RoomType.IsRoom);
                if (!value && Types.Contains(RoomType.IsRoom))
                    Types.Remove(RoomType.IsRoom);
            }
            get
            {
                return Types.Contains(RoomType.IsRoom);
            }
        }
        public bool IsCross
        {
            set
            {
                if (value && !Types.Contains(RoomType.IsCross))
                    Types.Add(RoomType.IsCross);
                if (!value && Types.Contains(RoomType.IsCross))
                    Types.Remove(RoomType.IsCross);
            }
            get
            {
                return Types.Contains(RoomType.IsCross);
            }
        }
        public bool IsDeadend
        {
            set
            {
                if (value && !Types.Contains(RoomType.IsDeadend))
                    Types.Add(RoomType.IsDeadend);
                if (!value && Types.Contains(RoomType.IsDeadend))
                    Types.Remove(RoomType.IsDeadend);
            }
            get
            {
                return Types.Contains(RoomType.IsDeadend);
            }
        }
        public bool IsRoomExit
        {
            set
            {
                if (value && !Types.Contains(RoomType.IsRoomExit))
                    Types.Add(RoomType.IsRoomExit);
                if (!value && Types.Contains(RoomType.IsRoomExit))
                    Types.Remove(RoomType.IsRoomExit);
            }
            get
            {
                return Types.Contains(RoomType.IsRoomExit);
            }
        }
        public bool IsDoorhole
        {
            set
            {
                if (value && !Types.Contains(RoomType.IsDoorhole))
                    Types.Add(RoomType.IsDoorhole);
                if (!value && Types.Contains(RoomType.IsDoorhole))
                    Types.Remove(RoomType.IsDoorhole);
            }
            get
            {
                return Types.Contains(RoomType.IsDoorhole);
            }
        }
        public bool IsEdge
        {
            set
            {
                if (value && !Types.Contains(RoomType.IsEdge))
                    Types.Add(RoomType.IsEdge);
                if (!value && Types.Contains(RoomType.IsEdge))
                    Types.Remove(RoomType.IsEdge);
            }
            get
            {
                return Types.Contains(RoomType.IsEdge);
            }
        }

        public List<string> ConditionNames
        {
            get
            {
                List<string> ret = new List<string>();

                if (IsLongCorridor) ret.Add("LongCoridor");
                if (IsDoorhole) ret.Add("Doorhole");
                if (IsRoomExit) ret.Add("RoomExit");
                if (IsRoom) ret.Add("Room");
                if (IsCross) ret.Add("Cross");
                if (IsDeadend) ret.Add("Deadend");
                if (IsEdge) ret.Add("Edge");

                return ret;
            }
        }
        public UnityEngine.Vector2Int Position
        {
            get
            {
                return new UnityEngine.Vector2Int(PosX, PosY);
            }
        }
        public UnityEngine.Vector3 PositionWorld
        {
            get
            {
                return new UnityEngine.Vector3(PosX, 0, PosY);
            }
        }
    
        public bool IsCellCorrect(RoomType[] types)
        {
            int count = 0;
            foreach (var a in types)
                switch (a)
                {
                    case RoomType.IsLongCorridor:
                        if (IsLongCorridor) count++;
                        break;
                    case RoomType.IsRoom:
                        if (IsRoom) count++;
                        break;
                    case RoomType.IsCross:
                        if (IsCross) count++;
                        break;
                    case RoomType.IsDeadend:
                        if (IsDeadend) count++;
                        break;
                    case RoomType.IsRoomExit:
                        if (IsRoomExit) count++;
                        break;
                    case RoomType.IsDoorhole:
                        if (IsDoorhole) count++;
                        break;
                    case RoomType.IsEdge:
                        if (IsEdge) count++;
                        break;
                    case RoomType.IsMess:
                        if (IsMess) count++;
                        break;
                }

            return count > 0;
        }
        public bool IsCellCompliesRequirements(EnvironmentRequirement[] types)
        {
            foreach (var a in types)
                switch (a)
                {
                    case EnvironmentRequirement.Walls:
                        if (Walls.Count == 0) return false;
                        break;
                    case EnvironmentRequirement.VisibleWalls:
                        if (!Walls.Any(x => x == 1) && !Walls.Any(x => x == 1)) return false;
                        break;
                }

            return true;
        }
    }
}
