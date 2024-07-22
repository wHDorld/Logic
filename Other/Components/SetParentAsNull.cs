using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Other.Components
{
    public class SetParentAsNull : MonoBehaviour
    {
        private void Start()
        {
            transform.SetParent(null, true);
        }
    }
}
