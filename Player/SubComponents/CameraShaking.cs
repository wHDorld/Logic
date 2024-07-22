using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Player.SubComponents
{
    public class CameraShaking : MonoBehaviour
    {
        private Cinemachine.CinemachineImpulseSource noise;

        private void Start()
        {
            noise = GetComponent<Cinemachine.CinemachineImpulseSource>();
        }

        public void Shake(DamageInfo info)
        {
            noise.GenerateImpulse((info.From - transform.position).normalized * info.Damage / 50f);
        }
    }
}
