using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using System.Collections;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    public class D_SaveCoroutine : DungeonBehaviour
    {
        public float Interval = 5;

        public override IEnumerator Generate()
        {
            StartCoroutine(SaveC());
            yield return null;
        }

        private IEnumerator SaveC()
        {
            while (true)
            {
                yield return new WaitForSeconds(Interval);

                SaveSystem.Singletone.SaveSystem.Load_Preparation().PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
                SaveSystem.Singletone.SaveSystem.Save_Preparation(true);
            }
        }
    }
}
