using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using AssemblyCSharp.Assets.Logic.Character.Components;
using TMPro;
using AssemblyCSharp.Assets.Logic.UI.SubComponents;
using AssemblyCSharp.Assets.Logic.ArtScenes.Components;
using System.Collections;
using AssemblyCSharp.Assets.Logic.SaveSystem.Entities;

namespace AssemblyCSharp.Assets.Logic.UI.Components
{
    public class UI_SlaveInteractions : MonoBehaviour
    {
        public Object SlaveInteractElement;
        public Object ArtSceneButton;

        List<(GameObject, RectTransform, SlaveSaveElement)> slaveObjects = new List<(GameObject, RectTransform, SlaveSaveElement)>();
        Vector2 referenceResolution;
        private void Start()
        {
            foreach (var a in GameObject.FindGameObjectsWithTag("Slave"))
            {
                GameObject g = Instantiate(SlaveInteractElement) as GameObject;
                g.transform.SetParent(transform, false);
                slaveObjects.Add(
                    (
                    a, 
                    g.GetComponent<RectTransform>(), 
                    a.GetComponent<CharacterApplier>().Preset
                    )
                );
            }
            referenceResolution = GetComponent<CanvasScaler>().referenceResolution;
            FindButtons();
        }

        private void LateUpdate()
        {
            foreach (var a in slaveObjects)
            {
                var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, a.Item1.transform.position);
                screenPos = new Vector2(
                    screenPos.x * (referenceResolution.x / (float)Camera.main.pixelWidth),
                    screenPos.y * (referenceResolution.y / (float)Camera.main.pixelHeight)
                    );
                a.Item2.anchoredPosition = screenPos;
            }
        }

        private void FindButtons()
        {
            for (int i = 0; i < slaveObjects.Count; i++)
            {
                slaveObjects[i].Item2.gameObject
                   .GetComponentsInChildren<RectTransform>()
                   .Where(x => x.name == "Name")
                   .FirstOrDefault()
                   .GetComponent<TMP_Text>().text = slaveObjects[i].Item3.File.Name;


                var buttons = slaveObjects[i].Item2.gameObject.GetComponentsInChildren<Button>();
                foreach (var b in buttons)
                {
                    int u = i + 0;
                    switch (b.name)
                    {
                        case "Sex":
                            b.onClick.AddListener(delegate { ShowAllScenes(u + 0); });
                            break;
                        case "UseB":
                            b.onClick.AddListener(delegate { SwitchMainButtons(u + 0); });
                            SwitchMainButtons(u + 0);
                            break;
                        case "Pick":
                            b.onClick.AddListener(delegate { PickSlave(u + 0); });
                            break;
                    }
                }
            }
            RecheckAllPickedSlaves();
        }

        List<GameObject> currentSceneButtons = new List<GameObject>();
        int lastSceneShowed;
        public void ShowAllScenes(int slave)
        {
            foreach (var a in currentSceneButtons)
                Destroy(a);
            currentSceneButtons.Clear();

            if (lastSceneShowed == slave)
            {
                lastSceneShowed = -1;
                return;
            }
            lastSceneShowed = slave;

            var spawnPoint = slaveObjects[slave].Item2.gameObject
                .GetComponentsInChildren<RectTransform>()
                .Where(x => x.name == "SpawnPoint")
                .FirstOrDefault();

            var scenes = slaveObjects[slave].Item3.File.ArtScenes;

            int cnt = -1;
            for (int i = 0; i < scenes.Length; i++)
            {
                if (slaveObjects[slave].Item3.SceneAvaliable[i] == 0)
                    continue;
                cnt++;

                GameObject g = Instantiate(ArtSceneButton) as GameObject;
                g.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                g.transform.SetParent(slaveObjects[slave].Item2, false);
                g.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    spawnPoint.anchoredPosition.x,
                    spawnPoint.anchoredPosition.y - 40 * cnt
                    );
                g.GetComponent<sUI_FlyingButtons>().originalPos = g.GetComponent<RectTransform>().anchoredPosition;
                g.GetComponent<sUI_ButtonWorks>().ChangeText(scenes[i].Name);

                g.GetComponent<sUI_ButtonWorks>().ReconnectRightDot(slaveObjects[slave].Item2.gameObject.GetComponentsInChildren<RectTransform>()
                                                                .Where(x => x.name == "du_scenes")
                                                                .FirstOrDefault(), true);
                g.GetComponent<sUI_ButtonWorks>().ReconnectLeftDot(null, false);

                int u1 = slave + 0;
                int u2 = i + 0;
                g.GetComponent<Button>().onClick.AddListener(delegate { PlayScene(u1, u2); });

                currentSceneButtons.Add(g);
            }
        }
        
        public void SwitchMainButtons(int slave)
        {
            if (lastSceneShowed == slave)
                ShowAllScenes(slave);

            var bTree = slaveObjects[slave].Item2.gameObject
                            .GetComponentsInChildren<RectTransform>(true)
                            .Where(x => x.name == "BTree")
                            .FirstOrDefault();
            bTree.gameObject.SetActive(!bTree.gameObject.activeSelf);

            for (int i = 0; i < slaveObjects.Count; i++)
            {
                if (i == slave) continue;
                slaveObjects[i].Item2.gameObject.SetActive(!bTree.gameObject.activeSelf);
            }
        }

        public void PickSlave(int slave)
        {
            SaveSystem.Singletone.SaveSystem.Load_Preparation().ChoosenSlave = slaveObjects[slave].Item3.ItemFileName;
            SaveSystem.Singletone.SaveSystem.Save_Preparation(false);

            RecheckAllPickedSlaves();
        }
        private void RecheckAllPickedSlaves()
        {
            foreach (var x in slaveObjects)
            {
                var picked = x.Item2.GetComponentsInChildren<RectTransform>(true)
                    .Where(y => y.name == "picked")
                    .FirstOrDefault();
                bool chosen = x.Item3.ItemFileName == SaveSystem.Singletone.SaveSystem.Load_Preparation().ChoosenSlave;
                picked.gameObject.SetActive(chosen);
                picked.gameObject.GetComponentInParent<sUI_ButtonWorks>(true).ChangeText(
                    chosen ? "" : "PICK"
                    );
            }
        }

        Coroutine currentScenePlaying;
        public void PlayScene(int slave, int scene)
        {
            var c_scene = slaveObjects[slave].Item3.File.ArtScenes[scene];
            currentScenePlaying = FindFirstObjectByType<PlayerArtSceneController>().StartScene(c_scene);
            StartCoroutine(awaitForScene());
        }
        IEnumerator awaitForScene()
        {
            if (lastSceneShowed != -1)
                ShowAllScenes(lastSceneShowed);
            for (int i = 0; i < slaveObjects.Count; i++)
            {
                slaveObjects[i].Item2.gameObject.SetActive(false);
            }

            yield return currentScenePlaying;
            currentScenePlaying = null;

            for (int i = 0; i < slaveObjects.Count; i++)
            {
                slaveObjects[i].Item2.gameObject.SetActive(true);
            }
        }
    }
}
