using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace AssemblyCSharp.Assets.Logic.Other.Debug
{
    public class SaveManipulation : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);
            slaves();
        }

        void slaves()
        {
            SaveSystem.Singletone.SaveSystem.SlaveContainer.Slaves.Add(
                new SaveSystem.Entities.SlaveSaveElement("Asa_Slave")
                );
            SaveSystem.Singletone.SaveSystem.SlaveContainer.Slaves.Add(
                new SaveSystem.Entities.SlaveSaveElement("NewSlave")
                );
            SaveSystem.Singletone.SaveSystem.Save_Slave();
        }
    }
}
