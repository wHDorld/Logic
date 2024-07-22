using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Inventory.Entities;

namespace AssemblyCSharp.Assets.Logic.Inventory.Singletone
{
    public static class SPropertiesWork
    {
        public static void ManageProperties(Item item, Inventory.Entities.Inventory inventory)
        {
            equipped(item, inventory);
            destroyAfterUse(item, inventory);
        }

        private static void equipped(Item item, Inventory.Entities.Inventory inventory)
        {
            foreach (var a in inventory.Items
                .Where(x => x.HasProperty(item.File.Type.ToString())))
                a.RemoveProperty("Equipped");
            item.AddProperty("Equipped");
        }
        private static void destroyAfterUse(Item item, Inventory.Entities.Inventory inventory)
        {
            if (item.HasProperty("DestroyAfterUse"))
                inventory.RemoveItem(item.transform.position);
        }
    }
}
