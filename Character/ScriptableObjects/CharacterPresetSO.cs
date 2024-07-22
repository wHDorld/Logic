using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Character.Entities;
using AssemblyCSharp.Assets.Logic.ArtScenes.ScriptableObjects;

namespace AssemblyCSharp.Assets.Logic.Character.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Default Character Preset", menuName = "Create Character Preset")]
    public class CharacterPresetSO : ScriptableObject
    {
        public MaterialElement HairMaterial;
        public int HairStyle = 5;
        public MaterialElement BodyMaterial;
        public MaterialElement BreastsMaterial;
        public float BreastsSize = 1;
        public MaterialElement SnoutMaterial;
        public MaterialElement FeetMaterial;
        public MaterialElement ClothMaterial;
        public MaterialElement TailMaterial;
        public MaterialElement EndTailMaterial;
        [Range(0, 8)]
        public int TailLength;
    }
}
