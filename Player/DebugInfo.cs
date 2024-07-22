using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Player
{
    public class DebugInfo : MonoBehaviour
    {
        private void OnGUI()
        {
            GUI.Label(
                new Rect(0, 0, 200, 200),
                string.Format("FPS: {0:f2}", 1f / Time.unscaledDeltaTime)
                );
        }
    }
}
