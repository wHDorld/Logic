using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using AssemblyCSharp.Assets.Logic.Inventory.Enums;
using AssemblyCSharp.Assets.Logic.Inventory.Singletone;

namespace AssemblyCSharp.Assets.Logic.Player
{
    [RequireComponent(typeof(UnitInventory))]
    public class PlayerLocalInventoryLoad : UnitMonoBehaviour
    {
        private void Start()
        {
            base.Start();

            GetComponent<UnitInventory>().inventory = SaveSystem.Singletone.SaveSystem.Load_LocalInventory();

            UseItems();
        }

        private void UseItems()
        {
            foreach (var tag in System.Enum.GetValues(typeof(EItemType)))
                foreach (var a in GetComponent<UnitInventory>().inventory.Items.Where(x => x.HasProperty(((EItemType)tag).ToString())))
                {
                    if (!a.HasProperty("Equipped"))
                        continue;
                    SItemUse.UseItemByType(
                        gameObject,
                        (EItemType)tag,
                        a.File.ReferedObject
                        );
                }
        }
    }
}
