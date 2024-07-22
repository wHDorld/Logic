using AssemblyCSharp.Assets.Logic.Inventory.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.InteractSystem.Components
{
    public class AddObjectToInvetory : MonoBehaviour
    {
        public ItemSO[] Items;
        public bool DestroyAfterGet = true;

        public void AddItem()
        {
            foreach (var a in Items)
                SaveSystem.Singletone.SaveSystem.LocalInventory[0, 0] = new Inventory.Entities.Item(a.name);

            if (DestroyAfterGet)
                Destroy(gameObject);
        }
    }
}
