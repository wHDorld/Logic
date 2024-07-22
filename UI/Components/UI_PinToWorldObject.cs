using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.UI.Components
{
    public class UI_PinToWorldObject : MonoBehaviour
    {
        public Transform WorldObject;

        RectTransform rect;
        private void Start()
        {
            rect = GetComponent<RectTransform>();
            if (GetComponent<Canvas>())
                GetComponent<Canvas>().worldCamera ??= Camera.main;
        }

        private void LateUpdate()
        {
            rect.anchoredPosition = Camera.main.WorldToScreenPoint(WorldObject.position);
            //rect.position = WorldObject.position;
        }
    }
}
