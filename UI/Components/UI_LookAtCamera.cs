using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.UI.Components
{
    public class UI_LookAtCamera : MonoBehaviour
    {
        private Transform camera;

        Vector3 offset;
        Transform parent;
        private void Start()
        {
            parent = transform.parent;
            offset = new Vector3(
                transform.localPosition.x * transform.parent.localScale.x,
                transform.localPosition.y * transform.parent.localScale.y,
                transform.localPosition.z * transform.parent.localScale.z
                );

            transform.SetParent(null);
            camera = Camera.main.transform;

            transform.localScale = Vector3.one;
        }

        private void LateUpdate()
        {
            if (parent == null)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 8f * Time.deltaTime);
                if (transform.localScale.magnitude <= 0.1f)
                    Destroy(gameObject);
                return;
            }
            transform.rotation = camera.transform.rotation;
            transform.position = parent.position + offset;
        }
    }
}
