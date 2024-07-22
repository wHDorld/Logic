using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Character.ScriptableObjects;
using AssemblyCSharp.Assets.Logic.Slave.Entities;
using Sirenix.OdinInspector;
using AssemblyCSharp.Assets.Logic.ArtScenes.ScriptableObjects;

namespace AssemblyCSharp.Assets.Logic.Slave.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Default Slave", menuName = "Create Slave File")]
    public class SlaveSO : ScriptableObject
    {
        public string Name;
        [PreviewField] public Object SlavePreset;
        public CharacterPresetSO CharacterPreset;
        public SlaveProperties Properties;
        public ArtSceneCompSO[] ArtScenes;
        public Sprite[] FullbodyPreview;

        private void OnEnable()
        {
            SlavePreset ??= Resources.Load("Slave");
        }
    }
}
