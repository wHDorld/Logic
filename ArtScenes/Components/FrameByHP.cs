using AssemblyCSharp.Assets.Logic.Character.Components;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp.Assets.Logic.ArtScenes.Components
{
    public class FrameByHP : MonoBehaviour
    {
        public Image Image;
        public CharacterApplier characterApplier;
        public ULife uLife;

        private void Start()
        {
            original_pos = Image.rectTransform.anchoredPosition;
            Image.sprite = characterApplier.Preset.File.FullbodyPreview[0];
            uLife.OnDamaged += ULife_OnDamaged;
        }

        int next_image = 1;
        Vector2 original_pos;
        private void ULife_OnDamaged(Unit.Entities.DamageInfo info)
        {
            if (currentChecking != null) 
                return;
            currentChecking = StartCoroutine(hp_checking());
        }

        Coroutine currentChecking = null;
        IEnumerator hp_checking()
        {
            while (characterApplier.Preset.File.FullbodyPreview.Length > next_image)
            {
                yield return null;
                float lowThres = uLife.MaxHealth - next_image * (uLife.MaxHealth / (float)characterApplier.Preset.File.FullbodyPreview.Length);
                if (uLife.Health <= lowThres)
                {
                    Image.sprite = characterApplier.Preset.File.FullbodyPreview[next_image];
                    next_image += 1;

                    if (currentShake != null) StopCoroutine(currentShake);
                    currentShake = StartCoroutine(shake());
                    yield return currentShake;
                }
                else
                    break;
            }
            currentChecking = null;
        }

        Coroutine currentShake;
        IEnumerator shake()
        {
            Func<float, float> get_y = (float x) =>
            {
                return (1f / Mathf.Pow(x, 2)) * Mathf.Sin(Mathf.Pow(x, 3));
            };

            float t = 8f;
            while (t > 0)
            {
                t -= Time.deltaTime * 8f;
                yield return null;

                Image.rectTransform.anchoredPosition = new Vector2(
                    original_pos.x + get_y(8f - t) * 40f,
                    original_pos.y
                    );
            }
            Image.rectTransform.anchoredPosition = original_pos;
            currentShake = null;
        }
    }
}
