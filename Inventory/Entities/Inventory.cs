using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

namespace AssemblyCSharp.Assets.Logic.Inventory.Entities
{
    [Serializable]
    public class Inventory
    {
        private int width;
        private int height;

        [ShowInInspector]
        [TableMatrix(DrawElementMethod = "DrawElement", IsReadOnly = true, ResizableColumns = false)]
        private InventoryElement[,] _current;
        [ShowInInspector]
        private List<Item> _items;
        public List<Item> Items
        {
            get
            {
                return _items;
            }
        }

        public Inventory(int width, int height)
        {
            this.width = width;
            this.height = height;

            _current = new InventoryElement[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    _current[x, y] = new InventoryElement();

            _items = new List<Item>();
        }
        public Inventory()
        {
            this.width = 16;
            this.height = 16;

            _current = new InventoryElement[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    _current[x, y] = new InventoryElement();

            _items = new List<Item>();
        }

        public Item this [int x, int y]
        {
            get
            {
                if (x >= width || x < 0)
                    return null;
                if (y >= height || y < 0)
                    return null;

                int refer = _current[x, y].ItemReference;
                if (refer >= _items.Count || refer < 0)
                    return null;

                return _items[refer];
            }
            set
            {
                var rect = ReplaceItemPosition(value.transform);
                if (rect.x == -1)
                    return;

                value.transform = rect;
                setUpElementsByItem(value.transform, true);
                _items.Add(value);
                OnContentChange?.Invoke();
            }
        }

        public void RemoveItem(Item item)
        {
            setUpElementsByItem(item.transform, false);
            _items.Remove(item);
            OnContentChange?.Invoke();
        }
        public void RemoveItem(Vector2Int pos)
        {
            var dItem = _items.Where(x => x.transform.position == pos).FirstOrDefault();
            if (dItem == null)
                return;

            setUpElementsByItem(dItem.transform, false);
            _items.Remove(dItem);
            OnContentChange?.Invoke();
        }
        public void RemoveItem(int num)
        {
            setUpElementsByItem(_items[num].transform, false);
            _items.RemoveAt(num);
            OnContentChange?.Invoke();
        }
        public bool IsPlaceAvaliable(RectInt pos)
        {
            for (int y = pos.y; y < pos.y + pos.height; y++)
            {
                if (y >= height || y < 0)
                    return false;

                for (int x = pos.x; x < pos.x + pos.width; x++)
                {
                    if (x >= width || x < 0)
                        return false;

                    if (_current[x, y].IsOccupied)
                        return false;
                }
            }

            return true;
        }
        public int GetWidth()
        {
            return width;
        }
        public int GetLastHeight()
        {
            return height;
        }
        public Item[] GetItems()
        {
            return _items.ToArray();
        }

        public RectInt ReplaceItemPosition(RectInt pos)
        {
            if (pos.x >= width || pos.x < 0)
                pos = new RectInt(0, pos.y, pos.width, pos.height);
            if (pos.y >= height || pos.y < 0)
                pos = new RectInt(pos.x, 0, pos.width, pos.height);

            bool[,] mask = new bool[width, height];
            bool[,] passedMask = new bool[width, height];
            mask[pos.x, pos.y] = true;

            Predicate<Vector2Int> isPassedNearby = (Vector2Int p) =>
            {
                if (p.x + 1 < width)
                    if (passedMask[p.x + 1, p.y]) 
                        return true;
                if (p.x - 1 >= 0)
                    if (passedMask[p.x - 1, p.y])
                        return true;

                if (p.y + 1 < height)
                    if (passedMask[p.x, p.y + 1])
                        return true;
                if (p.y - 1 >= 0)
                    if (passedMask[p.x, p.y - 1])
                        return true;

                return false;
            };
            Action overlapMasks = () =>
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (!mask[x, y])
                            continue;
                        passedMask[x, y] = true;
                    }
                }
            };

            for (int i = 0; i < height * width; i++)
            {
                overlapMasks();
                bool[,] bMask = new bool[width, height];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (!mask[x, y])
                        {
                            if (isPassedNearby(new Vector2Int(x, y)))
                                bMask[x, y] = true;
                            continue;
                        }

                        var ret = new RectInt(x, y, pos.width, pos.height);
                        if (IsPlaceAvaliable(ret))
                        {
                            return ret;
                        }
                    }
                }

                mask = bMask;
            }

            expandInventory(pos);

            return ReplaceItemPosition(pos);
        }

        private void setUpElementsByItem(RectInt pos, bool put)
        {
            for (int y = pos.y; y < pos.y + pos.height; y++)
            {
                for (int x = pos.x; x < pos.x + pos.width; x++)
                {
                    _current[x, y].IsOccupied = put;
                    _current[x, y].ItemReference = put ? _items.Count : -1;
                }
            }
        }
        private void expandInventory(RectInt pos)
        {
            InventoryElement[,] bArray = new InventoryElement[width, height + pos.height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bArray[x, y] = _current[x, y];
                }
            }
            for (int y = 0; y < pos.height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bArray[x, y + height] = new InventoryElement();
                }
            }

            height = bArray.GetLength(1);
            _current = bArray;
        }

        #if UNITY_EDITOR
        private InventoryElement DrawElement(Rect rect, InventoryElement value)
        {
            UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value.IsOccupied ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));

            return value;
        }
        #endif

        public delegate void InventoryDelegate();
        [field: NonSerialized]
        public event InventoryDelegate OnContentChange;
    }
}
