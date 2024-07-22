using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Inventory.Entities;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Unit.Components
{
    public class UnitInventory : UnitMonoBehaviour
    {
        public int Width = 7;
        public int Height = 10;
        public Inventory.Entities.Inventory inventory;

        public void Awake()
        {
            base.Awake();
            inventory = new Inventory.Entities.Inventory(Width, Height);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                var items = Resources.LoadAll("Items", typeof(Inventory.ScriptableObjects.ItemSO));
                inventory[0, 0] = new Item(items[Random.Range(0, items.Length)].name);
                SaveSystem.Singletone.SaveSystem.Save_LocalInventory();
            }
        }
    }
}
