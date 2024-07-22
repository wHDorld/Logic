using AssemblyCSharp.Assets.Logic.InputSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.InputSystem.Entities
{
    [Serializable]
    public class InputElement
    {
        public KeyCode Key;
        public string Tag;
        public float Value;
        public bool IsHolded;
        public EPressType PressType;
        public bool IsSmooth;
        public float SmoothSpeed;

        public InputElement()
        {

        }
        public InputElement(KeyCode key, string tag, EPressType pressType, bool isSmooth, float smoothSpeed)
        {
            Key = key;
            Tag = tag;
            PressType = pressType;
            IsSmooth = isSmooth;
            SmoothSpeed = smoothSpeed;
        }

        public void Work()
        {
            if (StopActionEvent != null && StopActionEvent.Invoke())
            {
                ResetValue(0);
                return;
            }
            switch (PressType)
            {
                case EPressType.Up:
                    ResetValue(Input.GetKeyUp(Key) ? 1 : 0);
                    break;
                case EPressType.Hold:
                    ResetValue(Input.GetKey(Key) ? 1 : 0);
                    break;
                case EPressType.Down:
                    ResetValue(Input.GetKeyDown(Key) ? 1 : 0);
                    break;
            }
        }

        private float _holdTimer = 0;
        private void ResetValue(float new_value)
        {
            if (!IsSmooth)
                Value = new_value;
            else
                Value = Mathf.Lerp(Value, new_value, SmoothSpeed * Time.deltaTime);

            IsHolded = _holdTimer < 0;
            if (Value <= 0.01f)
                _holdTimer = 0.1f;
            else
                _holdTimer -= Time.deltaTime;
        }

        public delegate bool StopActionDelegate();
        public event StopActionDelegate StopActionEvent;
    }
}
