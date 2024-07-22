using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Unit.Entities
{
    public class DamageInfo
    {
        public int Damage;
        public int HealthBefore;
        public int HealthAfter;
        public Vector3 From;

        public DamageInfo(int Damage, Vector3 From)
        {
            this.Damage = Damage;
            this.From = From;
        }
    }
}
