using AssemblyCSharp.Assets.Logic.InputSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.InputSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InputSettings", menuName = "New Input Settings")]
    public class InputPresetSO : ScriptableObject
    {
        public List<InputElement> DefaultInputs = new List<InputElement>();
    }
}
