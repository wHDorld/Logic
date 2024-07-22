using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.SaveSystem.Entities
{
    [System.Serializable]
    public class PreparationData
    {
        public string ChoosenSlave = "Asa_Slave";
        public int CurrentDeep = 1;

        public bool IsInDungeon = false;
        public bool FirstLoad = true;

        public Vector3 PlayerPosition
        {
            get
            {
                if (_playerPosition == null || _playerPosition.Length == 0)
                    _playerPosition = new float[3] { 0, 0, 0 };
                return new Vector3(_playerPosition[0], _playerPosition[1], _playerPosition[2]);
            }
            set
            {
                if (_playerPosition == null || _playerPosition.Length == 0)
                    _playerPosition = new float[3] { 0, 0, 0 };
                _playerPosition[0] = value.x;
                _playerPosition[1] = value.y;
                _playerPosition[2] = value.z;
            }
        }
        public UnityEngine.Random.State RandomSeed;

        private float[] _playerPosition;
    }
}
