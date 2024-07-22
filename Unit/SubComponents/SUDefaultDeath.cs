using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System.Collections;

namespace AssemblyCSharp.Assets.Logic.Unit.SubComponents
{
    [RequireComponent(typeof(ULife))]
    public class SUDefaultDeath : UnitMonoBehaviour
    {
        private void Start()
        {
            base.Start();

            GetComponent<ULife>().OnDied += SUDefaultDeath_OnDied;
        }

        private void SUDefaultDeath_OnDied(DamageInfo info)
        {
            StatContainer.IsDead = true;
            GetComponent<UAnimatorController>().animator.SetTrigger("Death1Trigger");
            StartCoroutine(drowing());
        }

        IEnumerator drowing()
        {
            yield return new WaitForSeconds(2);
            float t = 8;
            float scaleFactor = transform.localScale.magnitude;

            while (t > 0)
            {
                yield return null;
                t -= Time.deltaTime;
                transform.position -= Vector3.up * Time.deltaTime * 0.05f * scaleFactor;
            }

            Destroy(gameObject);
        }
    }
}
