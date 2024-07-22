using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Player;
using AssemblyCSharp.Assets.Logic.Player.Entities;

namespace AssemblyCSharp.Assets.Logic.InteractSystem.Components
{
    public class InteractObject : MonoBehaviour
    {
        public float MinimumDistance = 2f;
        public InteractEntity Interact;

        Transform player;

        private void Start()
        {
            Interact.From = gameObject;
        }

        bool AlreadyAdded = false;
        private void FixedUpdate()
        {
            if (player == null)
            {
                if (GameObject.FindGameObjectWithTag("Player"))
                    player = GameObject.FindGameObjectWithTag("Player").transform;
                return;
            }

            if (!IsInDistance)
            {
                if (AlreadyAdded)
                {
                    PlayerInteractComponent.RemoveInteraction(gameObject);
                    Interact.ForceEndAction?.Invoke();
                    AlreadyAdded = false;
                }
                return;
            }
            if (!AlreadyAdded)
            {
                PlayerInteractComponent.InsertInteraction(Interact);
                AlreadyAdded = true;
            }
        }

        bool IsInDistance
        {
            get
            {
                return Vector3.Distance(player.position, transform.position) <= MinimumDistance;
            }
        }
    }
}
