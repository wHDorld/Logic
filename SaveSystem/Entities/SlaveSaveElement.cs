using AssemblyCSharp.Assets.Logic.Slave.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.SaveSystem.Entities
{
    [System.Serializable]
    public class SlaveSaveElement
    {
        public string ItemFileName;
        public byte[] SceneAvaliable;

        [System.NonSerialized] private SlaveSO _file;
        public SlaveSO File
        {
            get
            {
                if (_file == null)
                    Load();
                return _file;
            }
        }

        public SlaveSaveElement(string FileName)
        {
            ItemFileName = FileName;
            Load();
        }
        public void Load()
        {
            _file = Resources.Load("Slaves/" + ItemFileName) as SlaveSO;

            if (SceneAvaliable == null || SceneAvaliable.Length == 0)
                SceneAvaliable = new byte[_file.ArtScenes.Length];

            if (SceneAvaliable.Length < _file.ArtScenes.Length)
            {
                List<byte> buffer = new List<byte>();
                buffer.AddRange(SceneAvaliable);
                SceneAvaliable = new byte[_file.ArtScenes.Length];
                for (int i = 0; i < buffer.Count; i++)
                    SceneAvaliable[i] = buffer[i];
            }
        }
    }
}
