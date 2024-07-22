using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.UI.Components
{
    public class UI_PinPosition : MonoBehaviour
    {
        public Transform Pin;


        private void LateUpdate()
        {
            transform.position = Pin.transform.position;
        }
    }
}
