using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.ArtScenes.Components
{
    public class ArtSceneAction : MonoBehaviour
    {
        public RectTransform Life;
        public Image ActionImage;

        public void Initiate(Sprite image, Vector3 pos, float time)
        {
            ActionImage.sprite = image;
            GetComponent<RectTransform>().anchoredPosition = pos;
            GetComponent<RectTransform>().localScale = Vector3.one;
            GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
            StartCoroutine(life(time));
        }

        IEnumerator life(float time)
        {
            float t = 0;

            while (t < time)
            {
                yield return null;
                t += Time.deltaTime;

                Life.localScale = new Vector3(
                    (time - t) / time,
                    1,
                    1
                    );
            }
            Destroy(gameObject);
        }
    }
}
