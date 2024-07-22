using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.Visual.Components
{
    public class VCameraSwitcher : MonoBehaviour
    {
        public GameObject[] Cameras;
        public float Interval = 0.1f;

        private IEnumerator Start()
        {
            while (true)
            {
                foreach (var a in Cameras)
                    a.SetActive(!a.activeSelf);

                yield return new WaitForSeconds(Interval);
            }
        }
    }
}
