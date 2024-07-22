using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.DungeonEnvironment.Components
{
    public class Env_ActiveRandomize : MonoBehaviour
    {
        public float DeactiveChance = 0.5f;

        private void Start()
        {
            if (Random.value <= DeactiveChance)
                gameObject.SetActive(false);

            Destroy(this);
        }
    }
}
