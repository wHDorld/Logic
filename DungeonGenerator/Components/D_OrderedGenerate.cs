using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Sirenix.OdinInspector;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    public class D_OrderedGenerate : MonoBehaviour
    {
        public List<DungeonBehaviour> Components;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame(); 
            Components = GetComponents<DungeonBehaviour>()
                .Where(x => x.isActiveAndEnabled)
                .OrderBy(x => x.Order)
                .ToList();

            foreach (var a in Components)
                yield return a.Generate();

            GetComponent<DungeonDataHandler>().DungeonProperties.CallOnGenerated();
        }

        [Button]
        public void FormOrder()
        {
            Components = GetComponents<DungeonBehaviour>()
                .Where(x => x.isActiveAndEnabled)
                .OrderBy(x => x.Order)
                .ToList();
        }
    }
}
