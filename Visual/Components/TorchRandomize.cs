using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace AssemblyCSharp.Assets.Logic.Visual.Components
{
    [RequireComponent(typeof(Light))]
    public class TorchRandomize : MonoBehaviour
    {
        public Light light;
        public float IntensityRandomizeMultiply = 0.1f;
        public float PositionRandomizeMultiply = 0.1f;
        public float UpdateRandomizeInterval = 100;
        public float LerpSpeed = 15f;
        public float MaxPlayerDistance = 100f;

        private float intensity;
        private Vector3 position;
        private Transform player;

        private float newIntensity;
        private Vector3 newPosition;

        IEnumerator Start()
        {
            light = GetComponent<Light>();
            intensity = light.intensity;
            position = transform.position;
            if (GameObject.FindGameObjectWithTag("Player"))
                player = GameObject.FindGameObjectWithTag("Player").transform;

            while (true)
            {
                newIntensity = intensity + intensity * (Random.value - 0.5f) * IntensityRandomizeMultiply * 2f;
                newPosition = position + new Vector3(
                        (Random.value - 0.5f) * PositionRandomizeMultiply * 2f,
                        (Random.value - 0.5f) * PositionRandomizeMultiply * 2f,
                        (Random.value - 0.5f) * PositionRandomizeMultiply * 2f
                        );
                if (player == null && GameObject.FindGameObjectWithTag("Player"))
                    player = GameObject.FindGameObjectWithTag("Player").transform;
                yield return new WaitForSeconds(Random.value * UpdateRandomizeInterval);
            }
        }

        private void Update()
        {
            light.intensity = Mathf.Lerp(
                light.intensity,
                newIntensity * (MaxPlayerDistance - playerDistance) / MaxPlayerDistance,
                LerpSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(
                transform.position,
                newPosition,
                LerpSpeed * Time.deltaTime);
        }

        float playerDistance
        {
            get
            {
                if (player != null)
                    return Vector3.Distance(player.transform.position, transform.position);
                else
                    return 0;
            }
        }
    }
}
