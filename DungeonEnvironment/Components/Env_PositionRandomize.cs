using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

namespace AssemblyCSharp.Assets.Logic.DungeonEnvironment.Components
{
    public class Env_PositionRandomize : MonoBehaviour
    {
        [MinMaxSlider(-1f, 1f)] public Vector2 AxisXLimit;
        [MinMaxSlider(-1f, 1f)] public Vector2 AxisYLimit;
        [MinMaxSlider(-1f, 1f)] public Vector2 AxisZLimit;

        private void Start()
        {
            transform.localPosition = transform.localPosition +
                new Vector3(
                    Random.Range(AxisXLimit.x, AxisXLimit.y),
                    Random.Range(AxisYLimit.x, AxisYLimit.y),
                    Random.Range(AxisZLimit.x, AxisZLimit.y)
                    );

            Destroy(this);
        }

        private void OnDrawGizmos()
        {
            Mesh mesh;
            if (!GetComponent<MeshFilter>())
                mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
            else
                mesh = GetComponent<MeshFilter>().sharedMesh;

            Gizmos.color = new Color(0, 1, 0, 0.15f);
            Gizmos.DrawMesh(mesh, 
                transform.position
                + new Vector3(AxisXLimit.y, AxisYLimit.y, AxisZLimit.y)
                , transform.rotation
                , transform.localScale);

            Gizmos.DrawMesh(mesh,
                transform.position
                + new Vector3(AxisXLimit.x, AxisYLimit.x, AxisZLimit.x)
                , transform.rotation
                , transform.localScale);
        }
    }
}
