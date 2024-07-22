using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.UI.SubComponents
{
    public class sUI_ButtonWorks : MonoBehaviour
    {
        public TMP_Text Text;
        public LayotAutoRebuilder LARebuilder;
        
        public RectTransform LeftDot;
        public sUI_Lines LeftLine;

        public RectTransform RightDot;
        public sUI_Lines RightLine;

        private void Start()
        {
            
        }

        public void ChangeText(string text)
        {
            Text.text = text;
            LARebuilder.Rebuild();
        }

        public void ReconnectLeftDot(RectTransform to, bool isActive)
        {
            LeftDot.gameObject.SetActive(isActive);
            if (!isActive)
                return;
            LeftLine.To = to;
        }
        public void ReconnectRightDot(RectTransform to, bool isActive)
        {
            RightDot.gameObject.SetActive(isActive);
            if (!isActive)
                return;
            RightLine.To = to;
        }
    }
}
