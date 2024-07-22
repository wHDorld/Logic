using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp.Assets.Logic.UI.SubComponents
{
    public class LayotAutoRebuilder : MonoBehaviour
    {
        private void Start()
        {
            Rebuild();
        }

        public void Rebuild()
        {
            foreach (var a in GetComponentsInChildren<RectTransform>())
                LayoutRebuilder.ForceRebuildLayoutImmediate(a);
            foreach (var a in GetComponentsInChildren<RectTransform>())
                LayoutRebuilder.ForceRebuildLayoutImmediate(a);

            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}
