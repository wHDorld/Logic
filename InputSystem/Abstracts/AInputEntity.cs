using AssemblyCSharp.Assets.Logic.InputSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.InputSystem.Abstracts
{
    public class AInputEntity
    {
        public List<InputElement> RegistredInputs = new List<InputElement>();
        public Vector2 AspectCoef;

        public Vector3 PlayerMovement;
        public Vector3 CursorMovement;
        public Vector3 CursorPosition;

        public void Initiate()
        {
            AspectCoef = new Vector2(
                1920f / Camera.main.pixelWidth,
                1080f / Camera.main.pixelHeight
                );
        }
        public void KeysUpdate()
        {
            foreach (var a in RegistredInputs)
                a.Work();
        }
        public virtual void VectorsUpdate()
        {
            PlayerMovement = Vector3.zero;
            CursorMovement = Vector3.zero;
            CursorPosition = Vector3.zero;
        }

        public float GetValue(string tag)
        {
            if (!RegistredInputs.Any(x => x.Tag == tag))
                return 0;
            return RegistredInputs.Find(x => x.Tag == tag).Value;
        }
        public float GetValueByTag(string tag)
        {
            if (!RegistredInputs.Any(x => x.Tag == tag))
                return 0;
            return RegistredInputs.Find(x => x.Tag == tag).Value;
        }

        public InputElement this [string tag]
        {
            get
            {
                if (!RegistredInputs.Any(x => x.Tag == tag))
                    return new InputElement();
                return RegistredInputs.Find(x => x.Tag == tag);
            }
            set
            {
                if (RegistredInputs.Any(x => x.Tag == tag))
                    return;
                RegistredInputs.Add(value);
            }
        }
    }
}
