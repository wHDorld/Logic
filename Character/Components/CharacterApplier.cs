using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Character.ScriptableObjects;
using AssemblyCSharp.Assets.Logic.Character.Entities;
using Sirenix.OdinInspector;
using AssemblyCSharp.Assets.Logic.Slave.ScriptableObjects;
using AssemblyCSharp.Assets.Logic.SaveSystem.Entities;

namespace AssemblyCSharp.Assets.Logic.Character.Components
{
    public class CharacterApplier : MonoBehaviour
    {
        public bool Preload = false;
        public SlaveSaveElement Preset;
        public Transform Character;

        private void Start()
        {
            if (Preload)
                ApplyCharacter();
        }

        public void SetUp(SlaveSaveElement Preset)
        {
            this.Preset = Preset;
            ApplyCharacter();
        }
        CharacterPresetSO cPreset;
        [Button]
        public void ApplyCharacter()
        {
            cPreset = Preset.File.CharacterPreset;
            var child = Character.GetComponentsInChildren<Renderer>(true);
            foreach (var a in child)
            {
                switch (a.name)
                {
                    case "Body":
                        MaterialApply(a, cPreset.BodyMaterial);
                        break;
                    case "Peasant_Belt":
                        MaterialApply(a, cPreset.ClothMaterial);
                        break;
                    case "Peasant_Jacket":
                        MaterialApply(a, cPreset.ClothMaterial);
                        break;
                    case "furryFace":
                        MaterialApply(a, cPreset.SnoutMaterial);
                        break;
                    case "_LeftBoob":
                        MaterialApply(a, cPreset.BreastsMaterial);
                        break;
                    case "_RightBoob":
                        MaterialApply(a, cPreset.BreastsMaterial);
                        break;

                    case "LFeet01":
                        MaterialApply(a, cPreset.FeetMaterial);
                        break;
                    case "LFeet02":
                        MaterialApply(a, cPreset.FeetMaterial);
                        break;
                    case "LFeet03":
                        MaterialApply(a, cPreset.FeetMaterial);
                        break;
                    case "RFeet01":
                        MaterialApply(a, cPreset.FeetMaterial);
                        break;
                    case "RFeet02":
                        MaterialApply(a, cPreset.FeetMaterial);
                        break;
                    case "RFeet03":
                        MaterialApply(a, cPreset.FeetMaterial);
                        break;
                }
                if (a.name.Contains("Hair")
                    && int.Parse(System.Text.RegularExpressions.Regex.Match(a.name, @"\d+").Value) == cPreset.HairStyle)
                {
                    a.gameObject.SetActive(true);
                    MaterialApply(a, cPreset.HairMaterial);
                }
                if (a.name.Contains("TailElement"))
                    MaterialApply(a, cPreset.TailMaterial);
                if (a.name.Contains("TailElement_End"))
                    MaterialApply(a, cPreset.EndTailMaterial);
            }

            var tailBones = Character.GetComponentsInChildren<Transform>(true)
                .Where(x => x.name.Contains("Bone.00"));
            foreach (var a in tailBones)
                if (int.Parse(System.Text.RegularExpressions.Regex.Match(a.name, @"\d+").Value) > cPreset.TailLength)
                    a.gameObject.SetActive(false);
            Character.GetComponentsInChildren<Transform>(true).
                Where(x => x.name == "RightBoob")
                .FirstOrDefault()
                .transform.localScale += (Vector3.one * (cPreset.BreastsSize - 8f)) * 0.12f;
            Character.GetComponentsInChildren<Transform>(true).
                Where(x => x.name == "LeftBoob")
                .FirstOrDefault()
                .transform.localScale += (Vector3.one * (cPreset.BreastsSize - 8f)) * 0.12f;
        }

        private void MaterialApply(Renderer g, MaterialElement material)
        {
            g.materials[0].SetTexture("_MainTexture", material.MainTexture);
            g.materials[0].SetColor("_MainColor", material.MainColor);
        }
    }
}
