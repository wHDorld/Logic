using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp.Assets.Logic.ArtScenes.Components
{
    public class FrameSequence : MonoBehaviour
    {
        public Object Frame;
        public Vector3Int InitiatePosition;
        public Vector2Int InitiateSize;
        public float SequenceSpeed = 10;

        GameObject currentImage;
        public void ChangeFrame(Sprite image)
        {
            GameObject g = Instantiate(Frame) as GameObject;
            g.transform.SetParent(transform);
            g.GetComponent<Image>().sprite = image;
            g.GetComponent<Image>().color *= new Color(1, 1, 1, 0);

            g.GetComponent<RectTransform>().localPosition = InitiatePosition;
            g.GetComponent<RectTransform>().sizeDelta = InitiateSize;
            g.GetComponent<RectTransform>().localScale = Vector3.one;
            g.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);

            StartCoroutine(frameChanging(g));
        }
        public void ShutDown()
        {
            StartCoroutine(frameChanging(currentImage));
        }

        IEnumerator frameChanging(GameObject image)
        {
            var img = image.GetComponent<Image>();
            var oldCol = new Color(img.color.r, img.color.g, img.color.b, img.color.a);
            var newCol = new Color(img.color.r, img.color.g, img.color.b, 1 - img.color.a);
            float t = 0;

            while (t <= 1)
            {
                t += Time.deltaTime * SequenceSpeed;
                img.color = Color.Lerp(oldCol, newCol, t);
                yield return null;
            }
            if (currentImage != null)
                Destroy(currentImage);
            currentImage = image;
        }

        public bool IsImageExist
        {
            get
            {
                return currentImage != null;
            }
        }
    }
}
