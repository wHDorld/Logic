using AssemblyCSharp.Assets.Logic.Unit.Abstract;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.AI.Components
{
    public class NPC_AI : UnitMonoBehaviour
    {
        public bool IsStationary;
        private IMovement movement;
        private IRotatement rotatement;

        private void Start()
        {
            base.Start();

            movement = GetComponent<IMovement>() ?? null;
            rotatement = GetComponent<IRotatement>() ?? null;

            if (!IsStationary)
                StartCoroutine(actionChange());
            else
                GetComponent<UAnimatorController>().animator.SetTrigger("ActionTrigger");
        }

        private void FixedUpdate()
        {
            base.FixedUpdate();

            movement.Move(currentMoveDirection, false);
            if (currentMoveDirection.magnitude > 0.1f)
                rotatement.LookAt(transform.position + currentMoveDirection);
        }

        Vector3 currentMoveDirection;
        IEnumerator actionChange()
        {
            while (true)
            {
                currentMoveDirection = new Vector3(
                    Random.Range(-1, 1f),
                    0,
                    Random.Range(-1f, 1)
                    ).normalized;
                yield return new WaitForSeconds(Random.Range(0.1f, 2.5f));
                currentMoveDirection = Vector3.zero;
                yield return new WaitForSeconds(Random.Range(1.5f, 8f));
            }
        }
    }
}
