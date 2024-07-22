using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Visual.Components
{
    public class SlowFlowComponent : MonoBehaviour
    {
        public float Speed = 15f;

        Vector3 offset;
        Transform parent;
        private void Start()
        {
            if (transform.parent == null)
                return;
            offset = transform.localPosition;
            parent = transform.parent;
            transform.SetParent(null, true);
        }

        private void Update()
        {
            if (parent == null)
                return;
            transform.position = Vector3.Lerp(
                transform.position,
                parent.TransformPoint(offset),
                Speed * Time.deltaTime
                );
        }
    }
}
