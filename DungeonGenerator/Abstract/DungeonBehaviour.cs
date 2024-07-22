using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Components;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract
{
    [RequireComponent(typeof(DungeonDataHandler))]
    public class DungeonBehaviour : MonoBehaviour
    {
        public int Order = 0;
        public int GeneratorTicks = 200;
        [HideInInspector] public DungeonDataHandler dataHandler;

        private void Start()
        {
            dataHandler = GetComponent<DungeonDataHandler>();
        }

        public virtual IEnumerator Generate()
        {
            yield return null;
        }
    }
}
