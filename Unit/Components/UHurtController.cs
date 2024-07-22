using AssemblyCSharp.Assets.Logic.Unit.Entities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Unit.Abstract;

namespace AssemblyCSharp.Assets.Logic.Unit.Components
{
    [RequireComponent(typeof(ULife))]
    public class UHurtController : UnitMonoBehaviour
    {
        public float PushForce;
        public Object[] BloodSplatter;

        private IMovement movement;

        private void Start()
        {
            base.Start();

            if (GetComponent<IMovement>() != null)
                movement = GetComponent<IMovement>();

            GetComponent<ULife>().OnDamaged += UHurtController_OnDamaged;
        }

        private void UHurtController_OnDamaged(DamageInfo info)
        {
            movement?.ForcedMove((transform.position - info.From) * PushForce * info.Damage / 30f, false);
            BloodSpawn(info);
        }

        private void BloodSpawn(DamageInfo info)
        {
            Vector3 dir = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(info.From.x, 0, info.From.z);
            GameObject g = Instantiate(BloodSplatter[Random.Range(0, BloodSplatter.Length)]) as GameObject;
            g.transform.position = transform.position + new Vector3(0, 1);
            g.transform.rotation = Quaternion.LookRotation(dir);
            Destroy(g, 25);
        }
    }
}
