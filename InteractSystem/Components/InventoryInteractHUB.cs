using AssemblyCSharp.Assets.Logic.UI.Components;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.InteractSystem.Components
{
    public class InventoryInteractHUB : MonoBehaviour
    {
        public UnitInventory LocalInventory;
        public UnitInventory GlobalInventory;

        public GameObject UIInventoryObject;
        public UI_Inventory LocalUIInventory;
        public UI_Inventory GlobalUIInventory;

        bool isInventoryOpen = false;

        public void InventorySwitch()
        {
            LocalInventory.inventory = SaveSystem.Singletone.SaveSystem.Load_LocalInventory();
            GlobalInventory.inventory = SaveSystem.Singletone.SaveSystem.Load_GlobalInventory();

            isInventoryOpen = !isInventoryOpen;
            UIInventoryObject.SetActive(isInventoryOpen);

            LocalUIInventory.UpdateContent(LocalInventory.inventory);
            GlobalUIInventory.UpdateContent(GlobalInventory.inventory);

            if (!isInventoryOpen)
            {
                SaveSystem.Singletone.SaveSystem.Save_LocalInventory();
                SaveSystem.Singletone.SaveSystem.Save_GlobalInventory();
            }
        }

        public void InventoryForceClose()
        {
            isInventoryOpen = true;
            InventorySwitch();
        }
    }
}
