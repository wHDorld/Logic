using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Slave.Entities
{
    [System.Serializable]
    public class JailcellElement
    {
        public Transform SpawnPoint;
        public int MaxSlaveCount;
        [HideInInspector] public int CurrentSlaveCount;
    }
}
