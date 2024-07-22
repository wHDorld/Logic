using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.ArtScenes.Enums;
using Sirenix.OdinInspector;

namespace AssemblyCSharp.Assets.Logic.ArtScenes.Entities
{
    [System.Serializable]
    public class FrameObject
    {
        [TableColumnWidth(60, Resizable = false)]
        [HorizontalGroup("Image")]
        [HideLabel]
        [PreviewField(Alignment = ObjectFieldAlignment.Left, FilterMode = FilterMode.Point, Height = 50)]
        public Sprite Image;

        [TableColumnWidth(150, Resizable = false)]
        [HorizontalGroup("Type")]
        [EnumPaging]
        [HideLabel]
        public FrameType FrameType;

        [DisableIf("FrameType", FrameType.Default)]
        [TableColumnWidth(60, Resizable = false)]
        [HorizontalGroup("Id")]
        [EnumPaging]
        [HideLabel]
        public int id;
    }
}
