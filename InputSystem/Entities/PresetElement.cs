using AssemblyCSharp.Assets.Logic.InputSystem.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.InputSystem.Entities
{
    [Serializable]
    public class PresetElement
    {
        public InputPresetSO Preset;
        public string Tag;
    }
}
