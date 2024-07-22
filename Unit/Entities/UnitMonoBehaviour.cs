using AssemblyCSharp.Assets.Logic.Unit.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Unit.Entities
{
    [RequireComponent(typeof(UnitStatContainer))]
    public class UnitMonoBehaviour : MonoBehaviour
    {
        [HideInInspector]
        public UnitStatContainer StatContainer;

        public void Awake()
        {
            
        }
        public void Start()
        {
            StatContainer = GetComponent<UnitStatContainer>();
        }
        public void Update()
        {
            if (StatContainer.IsDead)
                enabled = false;
        }
        public void FixedUpdate()
        {
            if (StatContainer.IsDead)
                enabled = false;
        }
        public void LateUpdate()
        {
            if (StatContainer.IsDead)
                enabled = false;
        }
    }
}
