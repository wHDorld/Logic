using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using AssemblyCSharp.Assets.Logic.InputSystem.Components;

namespace AssemblyCSharp.Assets.Logic.UI.SubComponents
{
    public class sUI_FlyingButtons : MonoBehaviour
    {
        RectTransform me;
        public Vector2 originalPos;
        Vector2 additionalVector;

        bool isInitialized = false;
        float maxDist = 80;
        float coef = 0.5f;
        private IEnumerator Start()
        {
            me = GetComponent<RectTransform>();
            maxDist += Random.Range(0f, 30f);
            coef += Random.Range(0f, 1f);
            yield return null;
            isInitialized = true;
            originalPos = me.anchoredPosition;
        }

        private void LateUpdate()
        {
            if (!isInitialized)
                return;
            additionalVector = Vector2.Lerp(additionalVector, Vector2.zero, 0.5f * Time.deltaTime);
            var mousePos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(me, InputComponent.PlayerInput.CursorPosition, Camera.main, out mousePos);
            //mousePos -= originalPos;
            float dist = mousePos.magnitude;
            dist = dist > maxDist ? maxDist : dist;
            dist /= maxDist;

            additionalVector += new Vector2(
                InputComponent.PlayerInput.CursorMovement.x,
                InputComponent.PlayerInput.CursorMovement.y
                ) * (1f - dist) * coef;
            me.anchoredPosition = originalPos + additionalVector;
        }
    }
}
