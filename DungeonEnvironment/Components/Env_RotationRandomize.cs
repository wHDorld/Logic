using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.DungeonEnvironment.Components
{
    public class Env_RotationRandomize : MonoBehaviour
    {
        public Vector2 AxisXLimit;
        public Vector2 AxisYLimit;
        public Vector2 AxisZLimit;

        private void Start()
        {
            var rot = transform.localEulerAngles +
                new Vector3(
                    Random.Range(AxisXLimit.x, AxisXLimit.y),
                    Random.Range(AxisYLimit.x, AxisYLimit.y),
                    Random.Range(AxisZLimit.x, AxisZLimit.y)
                    );
            transform.localRotation = Quaternion.Euler(rot);

            Destroy(this);
        }

        private void OnDrawGizmos()
        {
            Mesh mesh;
            if (!GetComponent<MeshFilter>())
                mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
            else
                mesh = GetComponent<MeshFilter>().sharedMesh;

            Gizmos.color = new Color(1, 1, 1, 0.15f);
            Gizmos.DrawMesh(mesh, transform.position, 
                transform.rotation
                * Quaternion.Euler(AxisXLimit.y, AxisYLimit.y, AxisZLimit.y)
                , transform.localScale);

            Gizmos.DrawMesh(mesh, transform.position,
                transform.rotation
                * Quaternion.Euler(AxisXLimit.x, AxisYLimit.x, AxisZLimit.x)
                , transform.localScale);
        }
    }
}
