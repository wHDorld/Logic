using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities
{
    [System.Serializable]
    public class DungeonProperties
    {
        private DungeonPreset _preset;
        private int _size;
        private DungeonMapCell[,] _dungeon;
        private GameObject[,] _dungeonMapCellGameObjects;
        private LevelPresets _levelPresets;
        private DungeonVisualPreset _dungeonVisualPreset;

        public int RandomSeed = 2281337;
        public int Size { get { return _size; } }
        public DungeonMapCell[,] Dungeon { get { return _dungeon; } }
        public GameObject[,] DungeonMapCellGameObjects { get { return _dungeonMapCellGameObjects; } }
        public DungeonPreset Preset { get { return _preset; } }
        public LevelPresets levelPresets { get { return _levelPresets; } }
        public DungeonVisualPreset visualPreset { get { return _dungeonVisualPreset; } }

        public string CurrentGenerationState;
        public float CurrentGenerationProgress;

        public void DebugLog(string msg, float coef)
        {
            CurrentGenerationProgress = coef;

            CurrentGenerationState = msg;/* + "\r\n[";
            for (int i = 0; i < coef * 10; i++)
                CurrentGenerationState += "#";
            for (float i = coef * 10; i < 10; i++)
                CurrentGenerationState += "   ";
            CurrentGenerationState += "]";*/

#if UNITY_EDITOR
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
            UnityEngine.Debug.Log(CurrentGenerationState);
#endif
        }

        public DungeonProperties(
            DungeonPreset preset,
            LevelPresets levelPresets,
            DungeonVisualPreset visualPreset)
        {
            _preset = preset;
            _levelPresets = levelPresets;
            _dungeonVisualPreset = visualPreset;
            _size = preset.DungeonSize;
            _dungeon = new DungeonMapCell[_size, _size];
            _dungeonMapCellGameObjects = new GameObject[_size, _size];
        }

        public void CallOnGenerated()
        {
            OnGenerated?.Invoke();
        }
        public delegate void DungeonGeneratorDelegate();
        public event DungeonGeneratorDelegate OnGenerated;
    }
}
