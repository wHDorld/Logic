using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.WeaponSystem.Entities
{
    [System.Serializable]
    public class ArrowProperties
    {
        [FoldoutGroup("General")] public int Damage;
        [FoldoutGroup("General")] public float Speed;
        [FoldoutGroup("General")] public float MaxDistance = 10f;

        [FoldoutGroup("Collission")] public int MaxCastCount = 10;
        [FoldoutGroup("Collission")] public bool DestroyAfterCollide = true;
        [FoldoutGroup("Collission")] public bool CollideObstacles = true;


        [FoldoutGroup("Orientation")] public float StartAngle = -5f;
        [FoldoutGroup("Orientation")] public float EndAngle = 45;

        public RandomizeArrowProperties RandomizeProperties;

        public int GetRandomDamage
        {
            get
            {
                return Damage + RandomizeProperties.GetRandomDamage;
            }
        }
        public float GetRandomSpeed
        {
            get
            {
                return Speed + RandomizeProperties.GetRandomSpeed;
            }
        }
        public float GetRandomDistance
        {
            get
            {
                return MaxDistance + RandomizeProperties.GetRandomDistance;
            }
        }
        public float GetRandomStartAngle
        {
            get
            {
                return StartAngle + RandomizeProperties.GetRandomStartAngle;
            }
        }
        public float GetRandomEndAngle
        {
            get
            {
                return EndAngle + RandomizeProperties.GetRandomEndAngle;
            }
        }
    }
}
