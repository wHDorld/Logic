using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using AssemblyCSharp.Assets.Logic.Inventory.Entities;
using TMPro;
using AssemblyCSharp.Assets.Logic.Inventory.Singletone;
using AssemblyCSharp.Assets.Logic.Inventory.Enums;

namespace AssemblyCSharp.Assets.Logic.UI.Components
{
    public class UI_Inventory : MonoBehaviour
    {
        public Vector2 Spacing = new Vector2(7, 7);
        public Vector2 InventorySize = new Vector2(700, 700);

        public Object itemCell;
        public Object itemElement;
        public RectTransform Content;
        public UnitInventory unitInventory;

        public GameObject DescriptionObject;
        public Image PreviewImage;
        public TMP_Text NameText;
        public TMP_Text DescriptionText;

        private void Start()
        {
            unitInventory.inventory.OnContentChange += Inventory_OnContentChange;
        }

        private void Inventory_OnContentChange()
        {
            UpdateContent();
        }

        Vector2 cellSize;
        List<GameObject> currentItemCells = new List<GameObject>();
        List<UI_InventoryItem> currentItemElements = new List<UI_InventoryItem>();
        public void UpdateContent()
        {
            UpdateContent(unitInventory.inventory);
        }
        public void UpdateContent(Inventory.Entities.Inventory inventory)
        {
            InventorySize = new Vector2(Content.sizeDelta.x, Content.sizeDelta.x);
            cellSize =
                InventorySize * (1f / inventory.GetWidth())
                - Spacing;
            Content.GetComponent<GridLayoutGroup>().cellSize = cellSize;

            clearInventoryElements();
            for (int y = 0; y < inventory.GetLastHeight(); y++)
            {
                for (int x = 0; x < inventory.GetWidth(); x++)
                {
                    GameObject g = Instantiate(itemCell) as GameObject;
                    g.transform.SetParent(Content, false);

                    if (!inventory.IsPlaceAvaliable(new RectInt(x, y, 1, 1)))
                        g.GetComponent<Image>().color *= new Color(1, 1, 1, 0.5f);

                    currentItemCells.Add(g);
                }
            }

            updateItems(inventory);
        }

        private void updateItems(Inventory.Entities.Inventory inventory)
        {
            var items = inventory.GetItems();
            foreach (var a in items)
            {
                GameObject g = Instantiate(itemElement) as GameObject;
                g.transform.SetParent(
                    currentItemCells[a.transform.x + a.transform.y * inventory.GetWidth()].GetComponent<RectTransform>(), 
                    false);
                var item = g.GetComponent<UI_InventoryItem>();
                item.Initiate(a, this);
                g.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnItemClick(item); });

                currentItemElements.Add(item);
            }
            equippedMarksUpdate();
        }
        private void clearInventoryElements()
        {
            foreach (var a in currentItemCells)
                Destroy(a);
            currentItemCells.Clear();

            foreach (var a in currentItemElements)
                Destroy(a.gameObject);
            currentItemElements.Clear();
        }
        private void OnItemClick(UI_InventoryItem from)
        {
            selectionItemUpdate(from);
        }
        private void selectionItemUpdate(UI_InventoryItem from)
        {
            foreach (var a in currentItemElements)
            {
                a.selectionFrame.SetActive(a == from);
            }
        }
        private void equippedMarksUpdate()
        {
            foreach (var tag in System.Enum.GetNames(typeof(EItemType)))
                foreach (var a in currentItemElements.Where(x => x.item.HasProperty(tag)))
                {
                    a.equippedMark.SetActive(a.item.HasProperty("Equipped"));
                }
        }

        #region DESCRIPTION BOX
        Item currentDescriptionItem;

        public void DescribeItem(Item item)
        {
            currentDescriptionItem = item;

            DescriptionObject.SetActive(true);
            PreviewImage.sprite = item.File.Image;
            NameText.text = item.File.Name;
            DescriptionText.text = item.File.Description;
        }

        public void UseDescribeItem()
        {
            SItemUse.UseItemByType(
                unitInventory.gameObject, 
                currentDescriptionItem.File.Type, 
                currentDescriptionItem.File.ReferedObject);

            SPropertiesWork.ManageProperties(currentDescriptionItem, unitInventory.inventory);

            equippedMarksUpdate();
        }

        public void SetDescribeItemAsQuickSlot(int slot)
        {

        }
        #endregion

    }
}
