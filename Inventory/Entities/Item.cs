using AssemblyCSharp.Assets.Logic.Inventory.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Inventory.Entities
{
    [System.Serializable]
    public class Item
    {
        private int[] sTransform;
        private List<string> properties;

        public string ItemFileName;
        [System.NonSerialized] private ItemSO _file;
        public ItemSO File
        {
            get
            {
                if (_file == null)
                    Load();
                return _file;
            }
        }

        public Item(string FileName)
        {
            ItemFileName = FileName;
            Load();
            transform = new RectInt(
                -1,
                -1,
                _file.Width,
                _file.Height
                );
        }
        public void Load()
        {
            _file = Resources.Load("Items/" + ItemFileName) as ItemSO;
            AddProperty(_file.Type.ToString());
            AddProperty(_file.DefaultProperties.ToArray());
        }

        public RectInt transform
        {
            get
            {
                if (sTransform == null || sTransform.Length == 0)
                    sTransform = new int[4];

                return new RectInt(sTransform[0], sTransform[1], sTransform[2], sTransform[3]);
            }
            set
            {
                if (sTransform == null || sTransform.Length == 0)
                    sTransform = new int[4];

                sTransform[0] = value.x;
                sTransform[1] = value.y;
                sTransform[2] = value.width;
                sTransform[3] = value.height;
            }
        }

        public bool HasProperty(params string[] args)
        {
            properties ??= new List<string>();

            foreach (var a in args)
                if (!properties.Any(x => x == a))
                    return false;

            return true;
        }
        public void RemoveProperty(params string[] args)
        {
            properties ??= new List<string>();

            foreach (var a in args)
                properties.RemoveAll(x => x == a);
        }
        public void AddProperty(params string[] args)
        {
            properties ??= new List<string>();

            properties.AddRange(args);
            properties = properties.Distinct().ToList();
        }
    }
}
