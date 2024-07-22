using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.WeaponSystem.Components
{
    public class WeaponHitComponent : MonoBehaviour
    {
        public int TicksOfExist = 5;
        ColliderHitHandlerComponents[] allHitHandlers;
        bool isReady = false;

        private void Awake()
        {
            allHitHandlers = GetComponentsInChildren<ColliderHitHandlerComponents>();
            foreach (var a in allHitHandlers)
            {
                a.OnShutDown += Collider_OnShutDown;
            }
        }

        public void Initialize(
            WeaponInfo weaponInfo,
            string enemyTag)
        {
            foreach (var a in allHitHandlers)
            {
                a.Initialize(weaponInfo, enemyTag);
                a.GetComponent<MeshRenderer>().enabled = false;
            }
            isReady = true;
        }
        private void FixedUpdate()
        {
            if (!isReady)
                return;
            if (TicksOfExist <= 0)
            {
                Destroy(gameObject);
            }
            TicksOfExist--;
        }

        int collidersDone = 0;
        private void Collider_OnShutDown()
        {
            collidersDone++;
            if (collidersDone >= allHitHandlers.Length)
                Destroy(gameObject);
        }
    }
}
