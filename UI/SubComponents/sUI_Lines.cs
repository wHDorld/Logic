using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.InputSystem.Components;

namespace AssemblyCSharp.Assets.Logic.UI.SubComponents
{
    public class sUI_Lines : MonoBehaviour
    {
        public RectTransform From;
        public RectTransform To;

        RectTransform me;
        float wCoef;
        float hCoef;
        Camera mCam;
        private void Start()
        {
            me = GetComponent<RectTransform>();

            mCam = Camera.main;
            wCoef = InputComponent.PlayerInput.AspectCoef.x;
            hCoef = InputComponent.PlayerInput.AspectCoef.y;
        }

        private void LateUpdate()
        {
            Vector2 toPos = mCam.WorldToScreenPoint(To.position);
            Vector2 fromPos = mCam.WorldToScreenPoint(From.position);
            
            toPos = new Vector3(toPos.x * wCoef, toPos.y * hCoef, 0);
            fromPos = new Vector3(fromPos.x * wCoef, fromPos.y * hCoef, 0);


            me.rotation = Quaternion.LookRotation(To.position - From.position) * Quaternion.Euler(90, 0, 0);
            me.sizeDelta = new Vector2(
                3,
                Vector2.Distance(toPos, fromPos)
                );
        }
    }
}
