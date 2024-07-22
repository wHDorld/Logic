using AssemblyCSharp.Assets.Logic.InputSystem.Abstracts;
using AssemblyCSharp.Assets.Logic.InputSystem.Entities;
using AssemblyCSharp.Assets.Logic.InputSystem.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.InputSystem.Components
{
    public class InputComponent : MonoBehaviour
    {
        public List<PresetElement> Presets = new List<PresetElement>();
        public static AInputEntity PlayerInput;

        private InputPresetSO currentPreset;

        private void Awake()
        {
            //crossplatform
            PlayerInput = new PCInput();
            currentPreset = Presets.Find(x => x.Tag == "PC").Preset;
            foreach (var a in currentPreset.DefaultInputs)
                PlayerInput[a.Tag] = a;

            PlayerInput.Initiate();
        }

        private void Update()
        {
            PlayerInput.KeysUpdate();
            PlayerInput.VectorsUpdate();
        }
    }
}
