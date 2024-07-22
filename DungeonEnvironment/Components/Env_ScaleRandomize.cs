using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

namespace AssemblyCSharp.Assets.Logic.DungeonEnvironment.Components
{
    public class Env_ScaleRandomize : MonoBehaviour
    {
        [MinMaxSlider(-0.5f, 0.5f)] public Vector2 AxisXLimit;
        [DisableIf("ConstrainedProportion")] [MinMaxSlider(-0.5f, 0.5f)] public Vector2 AxisYLimit;
        [DisableIf("ConstrainedProportion")] [MinMaxSlider(-0.5f, 0.5f)] public Vector2 AxisZLimit;
        public bool ConstrainedProportion = true;

        private void Start()
        {
            if (!ConstrainedProportion)
                transform.localScale = transform.localScale +
                    new Vector3(
                        Random.Range(AxisXLimit.x, AxisXLimit.y),
                        Random.Range(AxisYLimit.x, AxisYLimit.y),
                        Random.Range(AxisZLimit.x, AxisZLimit.y)
                        );
            else
            {
                float newScale = Random.Range(AxisXLimit.x, AxisXLimit.y);
                transform.localScale = transform.localScale + Vector3.one * newScale;
            }

            Destroy(this);
        }

        private void OnDrawGizmos()
        {
            Mesh mesh;
            if (!GetComponent<MeshFilter>())
                mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
            else
                mesh = GetComponent<MeshFilter>().sharedMesh;

            Vector3 newScaleMax = ConstrainedProportion
                ? new Vector3(AxisXLimit.y, AxisXLimit.y, AxisXLimit.y)
                : new Vector3(AxisXLimit.y, AxisYLimit.y, AxisZLimit.y);
            Vector3 newScaleMin = ConstrainedProportion
                ? new Vector3(AxisXLimit.x, AxisXLimit.x, AxisXLimit.x)
                : new Vector3(AxisXLimit.x, AxisYLimit.x, AxisZLimit.x);

            Gizmos.color =  new Color(0, 0, 1, 0.1f);
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation,
                transform.lossyScale
                + newScaleMax);

            Gizmos.DrawMesh(mesh, transform.position, transform.rotation,
                transform.lossyScale
                + newScaleMin);
        }
    }
}
