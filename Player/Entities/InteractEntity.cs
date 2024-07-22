using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace AssemblyCSharp.Assets.Logic.Player.Entities
{
    [System.Serializable]
    public class InteractEntity
    {
        [HideInInspector] public GameObject From;
        public string InfoText;

        public UnityEvent Action;
        public UnityEvent ForceEndAction;
    }
}
