using AssemblyCSharp.Assets.Logic.Unit.Entities;
using AssemblyCSharp.Assets.Logic.Unit.Enums;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Unit.Components
{
    public class UnitStatContainer : MonoBehaviour
    {
        #region STATE
        private Vector3 _velocity;
        private Vector3 _lastPosition;
        public Vector3 Velocity { get { return _velocity; } }

        private Vector3 _currentAimPoint;
        public Vector3 Aim { get { return _currentAimPoint; } set { _currentAimPoint = value; } }

        private bool isOnGround;
        public bool IsOnGround { get { return isOnGround || !IsOnGroundChecking; } }

        public bool IsHit;
        public bool IsDead;
        #endregion

        #region TRIGGERS
        public bool HitTrigger;

        #endregion

        #region Objects
        public Transform WeaponHandler;
        public Transform TargetPoint;
        #endregion

        public UnitStatMovement movement;
        public UnitStatRotatement rotatement;
        public float GravityAffect = 1;

        [FoldoutGroup("Updates")] public bool IsOnGroundChecking = true;
        [FoldoutGroup("Updates")] public bool IsVelocityUpdate = true;

        [FoldoutGroup("Ground")] public float Weight = 1;
        [FoldoutGroup("Ground")] public RaycastType RaycastType;
        [FoldoutGroup("Ground")] public float GroundRayLength = 0.1f;
        [FoldoutGroup("Ground")] public int GroundRayDensity = 3;
        [FoldoutGroup("Ground")] public float GroundRayfieldRadius = 1;
        [FoldoutGroup("Ground")] public float GroundRayYOffset = -0.5f;

        [FoldoutGroup("Objects")] public string WeaponHandlerName = "Handle";

        new Transform transform;

        public void Start()
        {
            transform = gameObject.transform;
            _lastPosition = transform.position;

            ObjectsFind();
        }

        private void Update()
        {
        }
        private void FixedUpdate()
        {
            VelocityUpdate();
            OnGroundUpdate();
        }

        private void VelocityUpdate()
        {
            if (!IsVelocityUpdate)
                return;
            _velocity = transform.position - _lastPosition;
            _lastPosition = transform.position;
        }

        #region IS ON GROUND
        private void OnGroundUpdate()
        {
            if (!IsOnGroundChecking) return;
            switch (RaycastType)
            {
                case RaycastType.Ray: RaycastingGroundCheck(); break;
                case RaycastType.Sphere: SphereGroundCheck(); break;
            }
        }
        private void RaycastingGroundCheck()
        {
            RaycastHit hit;
            Ray ray;
            for (float y = -GroundRayfieldRadius; y <= GroundRayfieldRadius; y += (GroundRayfieldRadius * 2) / (GroundRayDensity - 1))
            {
                for (float x = -GroundRayfieldRadius; x <= GroundRayfieldRadius; x += (GroundRayfieldRadius * 2) / (GroundRayDensity - 1))
                {
                    ray = new Ray(transform.TransformPoint(new Vector3(x, GroundRayYOffset, y)), -transform.up);
                    isOnGround = Physics.Raycast(
                                ray,
                                out hit,
                                GroundRayLength
                                );
                    Debug.DrawRay(ray.origin, ray.direction * GroundRayLength);
                    if (isOnGround) break;
                }
                if (isOnGround) break;
            }
        }
        private void SphereGroundCheck()
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position + new Vector3(0, GroundRayYOffset, 0), -transform.up);
            isOnGround = Physics.SphereCast(
                        ray,
                        GroundRayfieldRadius,
                        out hit,
                        GroundRayLength,
                        LayerMask.GetMask("Objects")
                        );
            Debug.DrawRay(ray.origin, ray.direction * GroundRayfieldRadius);
        }
        #endregion

        #region EVENTS
        public delegate void HitDelegate();
        public event HitDelegate OnHit;
        public void Call_OnHit()
        {
            OnHit?.Invoke();
        }

        #endregion

        #region OBJECTS
        private void ObjectsFind()
        {
            WeaponHandler = transform
                .GetComponentsInChildren<Transform>()
                .Where(x => x.name == WeaponHandlerName)
                .FirstOrDefault();
        }
        #endregion
    }
}
