using AssemblyCSharp.Assets.Logic.Player.Abstract;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Player
{
    public class PlayerViewPointSetter : UnitMonoBehaviour
    {
        public Transform Point;
        public float DistanceMultiply = 25f;
        public float Speed = 2f;

        private Transform me;
        private APlayerAttack attackComponent;
        private void Start()
        {
            base.Start();
            attackComponent = GetComponent<APlayerAttack>();
            me = transform;
            Point.SetParent(null, true);
        }

        private void FixedUpdate()
        {
            base.FixedUpdate();
            Point.position = Vector3.Lerp(
                Point.position,
                Center + StatContainer.Velocity * DistanceMultiply,
                Speed * Time.fixedDeltaTime
                );
        }

        Vector3 Center
        {
            get
            {
                return me.position
                    + EnemyCenterOffset;
            }
        }
        Vector3 EnemyCenterOffset
        {
            get
            {
                if (attackComponent.CurrentEnemy == null)
                    return Vector3.zero;
                return (attackComponent.CurrentEnemy.transform.position - me.position) / 2f;
            }
        }
    }
}
