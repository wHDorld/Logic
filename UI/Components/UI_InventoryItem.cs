using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Inventory.Entities;
using UnityEngine.UI;
using AssemblyCSharp.Assets.Logic.Inventory.Singletone;
using UnityEngine.EventSystems;

namespace AssemblyCSharp.Assets.Logic.UI.Components
{
    public class UI_InventoryItem : MonoBehaviour
    {
        private UI_Inventory inventory;
        public Item item;

        public GameObject selectionFrame;
        public GameObject equippedMark;
        public RectTransform RectTransform;

        public void Initiate(Item item, UI_Inventory uI_Inventory)
        {
            GetComponent<RectTransform>().localScale = new Vector2(
                item.transform.width,
                item.transform.height
                );
            foreach (var a in GetComponentsInChildren<RectTransform>())
            {
                switch (a.name)
                {
                    case "ItemImage":
                        a.GetComponent<Image>().sprite = item.File.Image;
                        RectTransform = a;
                        break;
                    case "Select":
                        selectionFrame = a.gameObject;
                        selectionFrame.SetActive(false);
                        break;
                    case "Equipped":
                        equippedMark = a.gameObject;
                        equippedMark.SetActive(false);
                        break;
                }
            }

            this.item = item;
            inventory = uI_Inventory;
        }

        public void OnClick()
        {
            inventory.DescribeItem(item);
        }
    }
}
