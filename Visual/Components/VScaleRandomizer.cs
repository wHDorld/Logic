using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.Visual.Components
{
    public class VScaleRandomizer : MonoBehaviour
    {
        public Vector2 Multiply = new Vector2(0.5f, 2f);
        public float LerpSpeed = 15f;
        public Vector2 Interval = new Vector2(0.1f, 0.4f);

        Vector3 originalScale;
        Vector3 offsetScale;

        private void Start()
        {
            originalScale = transform.localScale;
        }

        float current_time = 0;
        private void Update()
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                offsetScale,
                LerpSpeed * Time.deltaTime
                );

            current_time -= Time.deltaTime;
            if (current_time <= 0)
            {
                current_time = Random.Range(Interval.x, Interval.y);
                offsetScale = originalScale * Random.Range(Multiply.x, Multiply.y);
            }
        }
    }
}
