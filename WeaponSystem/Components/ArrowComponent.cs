using AssemblyCSharp.Assets.Logic.WeaponSystem.Entities;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.WeaponSystem.Components
{
    public class ArrowComponent : MonoBehaviour
    {
        public ArrowProperties ArrowProperties;
        public GameObject[] DestroyIfStuck;

        Quaternion originalRotation;
        bool isReady = false;
        WeaponInfo weaponInfo;
        string EnemiesTags;

        public void Initialize( 
            WeaponInfo weaponInfo,
            string enemyTag)
        {
            EnemiesTags = enemyTag;
            this.weaponInfo = weaponInfo;
            originalRotation = transform.rotation;
            isReady = true;
        }

        private void FixedUpdate()
        {
            HitCheck();
            Move();
            Rotate();

            if (DistanceCoef >= 1f)
                Destroy(gameObject);
        }

        float distancePast = 0;
        private void Move()
        {
            if (!isReady)
                return;
            distancePast += ArrowProperties.GetRandomSpeed;
            transform.position += transform.forward * ArrowProperties.GetRandomSpeed;
        }
        private void Rotate()
        {
            if (!isReady)
                return;
            float coef = DistanceCoef;
            transform.rotation = originalRotation * Quaternion.Euler(
                ArrowProperties.GetRandomStartAngle * (1f - coef) + ArrowProperties.GetRandomEndAngle * coef,
                0,
                0
                );
        }

        private void HitCheck()
        {
            if (!isReady)
                return;

            bool collide = false;
            var hits = Physics.RaycastAll(
                transform.position,
                transform.forward,
                ArrowProperties.GetRandomSpeed + 0.05f,
                ~LayerMask.GetMask("PlayerMouse", "Pixel")
                )
                .OrderBy(x => Vector3.Distance(transform.position, x.transform.position));

            int cnt = 0;
            RaycastHit firstHit = new RaycastHit();
            foreach (var a in hits)
            {
                collide |= ArrowProperties.CollideObstacles && a.collider.tag == "Environment";

                if (collide)
                    firstHit = a;

                if (!a.collider.gameObject.CompareTag(EnemiesTags))
                    continue;
                if (!a.collider.gameObject.GetComponentInParent<ULife>())
                    continue;

                a.collider.gameObject.GetComponentInParent<ULife>().GetDamage(new DamageInfo(
                    Mathf.RoundToInt(ArrowProperties.GetRandomDamage * weaponInfo.DamageMultiply),
                    transform.position - transform.forward));
                collide = true;
                firstHit = a;
                cnt++;

                if (cnt >= ArrowProperties.MaxCastCount)
                    break; 
            }
            if (ArrowProperties.DestroyAfterCollide && collide)
            {
                transform.position = firstHit.point;
                transform.SetParent(firstHit.transform, true);
                foreach (var a in DestroyIfStuck)
                    Destroy(a);
                isReady = false;
                Destroy(this);
            }
        }

        public float DistanceCoef
        {
            get
            {
                return distancePast / (float)ArrowProperties.GetRandomDistance;
            }
        }
    }
}
