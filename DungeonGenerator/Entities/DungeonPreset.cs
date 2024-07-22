using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities
{
    [Serializable]
    public class DungeonPreset
    {
        public int DungeonSize = 100;
        public int DungeonDeep = 1000;
        public float DungeonCellScaleFactor = 5;
        public float DungeonScaleFactor = 1;
        public bool ClosingHoles = false;
        public bool ClosingDeadends = false;

        public float RoomSpawnChance = 0.1f;
        public int RoomMaxSize = 5;
        public int HallwayLong = 2;
        public int MaximumRoadLenght = 5;
        public int MinimumRoadLenght = 3;


        public float DungeonCellScale 
        {
            get
            {
                return DungeonScaleFactor * DungeonCellScaleFactor;
            }
        }
        public Vector3 CoridorCenter
        {
            get
            {
                return (new Vector3(-DungeonCellScaleFactor, 0, DungeonCellScaleFactor) / 2f) * DungeonScaleFactor;
            }
        }
        public Vector3 CoridorCenterNoScale
        {
            get
            {
                return (new Vector3(-DungeonCellScaleFactor, 0, DungeonCellScaleFactor) / 2f);
            }
        }
    }
}
