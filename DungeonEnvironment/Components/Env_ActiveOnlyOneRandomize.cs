using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.DungeonEnvironment.Components
{
    public class Env_ActiveOnlyOneRandomize : MonoBehaviour
    {
        private void Start()
        {
            var child = Random.Range(0, transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(i == child);

            Destroy(this);
        }
    }
}
