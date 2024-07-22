using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.Visual.Components
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerFollow : MonoBehaviour
    {
        public float MaxDistance = 7;
        public float RandomOffsetDistance;
        public Vector2 RandomTiming;
        public float FloatSpeed;
        public AnimationCurve FloatingCurve;

        Vector3 offset;
        Vector3 add_offset;
        Transform parent;
        CharacterController controller;

        private void Start()
        {
            currentRandomTime = Random.Range(RandomTiming.x, RandomTiming.y);
            parent = transform.parent;
            offset = transform.localPosition;
            transform.SetParent(null, true);
            controller = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            CheckDistance();
            Move();
            ForcedTeleport();
            RandomingOffset();
        }

        private void Move()
        {
            controller.Move((parent.position + offset + add_offset - transform.position) * _distance / 20f);
        }

        private void ForcedTeleport()
        {
            if (_distance < MaxDistance)
                return;
            controller.enabled = false;
            transform.position = parent.position + offset + add_offset;
            controller.enabled = true;
        }

        float _distance;
        private void CheckDistance()
        {
            _distance = Vector3.Distance(transform.position, parent.position + offset + add_offset);
        }

        float currentRandomTime;
        float floatingTime;
        Vector3 flow_add_offset;
        private void RandomingOffset()
        {
            floatingTime = floatingTime >= 1f ? 0 : (floatingTime + Time.fixedDeltaTime * FloatSpeed);
            add_offset = Vector3.Lerp(add_offset, flow_add_offset, 12f * Time.fixedDeltaTime);
            flow_add_offset = new Vector3(
                flow_add_offset.x,
                FloatingCurve.Evaluate(floatingTime),
                flow_add_offset.z
                );
            if (currentRandomTime > 0)
            {
                currentRandomTime -= Time.fixedDeltaTime;
                return;
            }
            currentRandomTime = Random.Range(RandomTiming.x, RandomTiming.y);
            flow_add_offset = new Vector3(
                Random.Range(-RandomOffsetDistance, RandomOffsetDistance),
                flow_add_offset.y,
                Random.Range(-RandomOffsetDistance, RandomOffsetDistance)
                );
            flow_add_offset = flow_add_offset.magnitude > RandomOffsetDistance ? flow_add_offset.normalized * RandomOffsetDistance : flow_add_offset;
        }
    }
}
