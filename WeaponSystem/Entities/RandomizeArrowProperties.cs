using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.WeaponSystem.Entities
{
    [Serializable]
    public class RandomizeArrowProperties
    {
        [FoldoutGroup("General")] [MinMaxSlider(-20, 20, ShowFields = true)] public Vector2Int Damage = new Vector2Int(-1, 1);
        [FoldoutGroup("General")] [MinMaxSlider(-1f, 1f, ShowFields = true)] public Vector2 Speed = new Vector2(-0.1f, 0.1f);
        [FoldoutGroup("General")] [MinMaxSlider(-25, 25, ShowFields = true)] public Vector2 MaxDistance = new Vector2(-5, 5);

        [FoldoutGroup("Orientation")] [MinMaxSlider(-45, 45, ShowFields = true)] public Vector2 StartAngle = new Vector2(-5f, 5f);
        [FoldoutGroup("Orientation")] [MinMaxSlider(-45, 45, ShowFields = true)] public Vector2 EndAngle = new Vector2(-5f, 5f);

        private bool wasInitialized = false;
        private int RandomDamage;
        private float RandomSpeed;
        private float RandomDistance;
        private float RandomStartAngle;
        private float RandomEndAngle;

        private void Initialize()
        {
            wasInitialized = true;

            RandomDamage = UnityEngine.Random.Range(Damage.x, Damage.y);
            RandomSpeed = UnityEngine.Random.Range(Speed.x, Speed.y);
            RandomDistance = UnityEngine.Random.Range(MaxDistance.x, MaxDistance.y);
            RandomStartAngle = UnityEngine.Random.Range(StartAngle.x, StartAngle.y);
            RandomEndAngle = UnityEngine.Random.Range(EndAngle.x, EndAngle.y);
        }

        public int GetRandomDamage
        {
            get
            {
                if (!wasInitialized)
                    Initialize();

                return RandomDamage;
            }
        }
        public float GetRandomSpeed
        {
            get
            {
                if (!wasInitialized)
                    Initialize();

                return RandomSpeed;
            }
        }
        public float GetRandomDistance
        {
            get
            {
                if (!wasInitialized)
                    Initialize();

                return RandomDistance;
            }
        }
        public float GetRandomStartAngle
        {
            get
            {
                if (!wasInitialized)
                    Initialize();

                return RandomStartAngle;
            }
        }
        public float GetRandomEndAngle
        {
            get
            {
                if (!wasInitialized)
                    Initialize();

                return RandomEndAngle;
            }
        }
    }
}
