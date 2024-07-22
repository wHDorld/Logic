using AssemblyCSharp.Assets.Logic.Character.Components;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Unit.Entities;

namespace AssemblyCSharp.Assets.Logic.Player
{
    [RequireComponent(typeof(CharacterApplier))]
    public class PlayerCharacterLoad : UnitMonoBehaviour
    {
        private void Start()
        {
            base.Start();

            var slave = SaveSystem.Singletone.SaveSystem.Load_Slave().GetSlaveByFileName(SaveSystem.Singletone.SaveSystem.Load_Preparation().ChoosenSlave);
            GetComponent<CharacterApplier>().SetUp(slave);
        }
    }
}
