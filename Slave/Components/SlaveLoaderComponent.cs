using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Slave.Entities;
using AssemblyCSharp.Assets.Logic.SaveSystem.Entities;
using System;
using AssemblyCSharp.Assets.Logic.Character.Components;

namespace AssemblyCSharp.Assets.Logic.Slave.Components
{
    public class SlaveLoaderComponent : MonoBehaviour
    {
        public JailcellElement[] Jailcells;

        private SlaveContainer slaveContainer;

        private void Start()
        {
            slaveContainer = SaveSystem.Singletone.SaveSystem.Load_Slave();
            SpawnSlaves();
        }

        void SpawnSlaves()
        {
            int currentCell = 0;
            Action<GameObject> putInJail = (g) =>
            {
                if (Jailcells[currentCell].CurrentSlaveCount >= Jailcells[currentCell].MaxSlaveCount)
                    currentCell++;
                Jailcells[currentCell].CurrentSlaveCount++;

                g.GetComponent<CharacterController>().enabled = false;

                g.transform.position = Jailcells[currentCell].SpawnPoint.position;

                g.GetComponent<CharacterController>().enabled = true;
            };

            for (int i = 0; i < slaveContainer.Slaves.Count; i++)
            {
                GameObject g = Instantiate(slaveContainer.Slaves[i].File.SlavePreset) as GameObject;
                putInJail(g);
                g.GetComponent<CharacterApplier>().SetUp(slaveContainer.Slaves[i]);
            }
        }
    }
}
