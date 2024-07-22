using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.ArtScenes.ScriptableObjects;
using Sirenix.OdinInspector;

namespace AssemblyCSharp.Assets.Logic.ArtScenes.Components
{
    public class PlayerArtSceneController : MonoBehaviour
    {
        public FrameSequence FrameSequence;
        public ArtSceneCompSO CurrentScene;
        
        [FoldoutGroup("ASAction")] public Object ASAction;
        [FoldoutGroup("ASAction")] public Sprite NextImage;
        [FoldoutGroup("ASAction")] public Sprite PreviousImage;
        [FoldoutGroup("ASAction")] public Sprite SexSceneStartImage;
        [FoldoutGroup("ASAction")] public Sprite X1Image;
        [FoldoutGroup("ASAction")] public Sprite X2Image;
        [FoldoutGroup("ASAction")] public Sprite X3Image;

        public bool IsDebugging;

        private void Start()
        {
            if (IsDebugging)
                StartScene(CurrentScene);
        }

        public Coroutine StartScene(ArtSceneCompSO scene)
        {
            CurrentScene = scene;
            callEnding = false;
            frameNumber = -1;
            repeatId = 0;
            repeatDeep = -1;

            StartCoroutine(Controller());
            return StartCoroutine(ScenePlay());
        }

        private IEnumerator Controller()
        {
            while (IsScenePlaying)
            {
                yield return new WaitForFixedUpdate();

                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.RightArrow))
                {
                    NextFrame();
                    if (isRepeating)
                    {
                        repeatDeep++;
                        repeatDeep = repeatDeep >= 4 ? 3 : repeatDeep;
                        ASActionRepeatDeep();
                    }
                    else
                        ASActionSpawn(NextImage, 1);
                    yield return new WaitForSeconds(1);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    PreviousFrame();
                    if (isRepeating)
                    {
                        repeatDeep--;
                        repeatDeep = repeatDeep <= 1 ? 1 : repeatDeep;
                        ASActionRepeatDeep();
                    }
                    yield return new WaitForSeconds(1);
                }
                if (repeatDeep >= 3 && frameNumber == CurrentScene.RepeatRange(repeatId)[1])
                {
                    isRepeating = false;
                    repeatId++;
                    yield return new WaitForSeconds(0.5f);
                    NextFrame();
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }
        private IEnumerator ScenePlay()
        {
            NextFrame();
            while (IsScenePlaying)
            {
                yield return new WaitForFixedUpdate();

                if (isRepeating)
                {
                    yield return new WaitForSeconds(1.01f - repeatDeep / 3f);
                    NextFrame();
                }
            }
            FrameSequence.ShutDown();
            CurrentScene = null;
        }

        private void NextFrame()
        {
            frameNumber += 1;
            callEnding = frameNumber >= CurrentScene.FrameObjects.Count;
            frameNumber = frameNumber >= CurrentScene.FrameObjects.Count 
                ? CurrentScene.FrameObjects.Count - 1 
                : frameNumber;
            isRepeating |= CurrentScene.IsRepeated(frameNumber, repeatId);

            if (isRepeating)
            {
                var range = CurrentScene.RepeatRange(repeatId);
                frameNumber = frameNumber > range[1] ? range[0] : frameNumber;
            }
            else
                repeatDeep = -1;

            FrameSequence.ChangeFrame(CurrentScene.FrameObjects[frameNumber].Image);
        }
        private void PreviousFrame()
        {
            if (isRepeating)
                return;

            ASActionSpawn(PreviousImage, 1);
            var range = CurrentScene.SequenceRange(repeatId);
            frameNumber -= 1;
            frameNumber =
                frameNumber < range[0]
                ? range[0]
                : (frameNumber > range[1]
                   ? range[1]
                   : frameNumber);

            FrameSequence.ChangeFrame(CurrentScene.FrameObjects[frameNumber].Image);
        }

        private void ASActionRepeatDeep()
        {
            if (repeatDeep == 0)
                ASActionSpawn(SexSceneStartImage, 1);
            if (repeatDeep == 1)
                ASActionSpawn(X1Image, 1);
            if (repeatDeep == 2)
                ASActionSpawn(X2Image, 1);
            if (repeatDeep == 3)
                ASActionSpawn(X3Image, 1);
        }
        private void ASActionSpawn(Sprite sprite, float time)
        {
            GameObject g = Instantiate(ASAction) as GameObject;
            g.transform.SetParent(transform, false);

            g.GetComponent<ArtSceneAction>().Initiate(
                sprite,
                new Vector3(1643, -98, 0),
                time
                );
        }

        private bool isRepeating;
        private int repeatId = 0;
        private int repeatDeep = -1;
        private int frameNumber = 0;
        private bool callEnding = false;
        public bool IsScenePlaying
        {
            get
            {
                return CurrentScene != null
                    && frameNumber <= CurrentScene.FrameObjects.Count
                    && !callEnding;
            }
        }
    }
}
