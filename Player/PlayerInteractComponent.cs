using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using TMPro;
using Sirenix.OdinInspector;
using AssemblyCSharp.Assets.Logic.Player.Entities;
using UnityEngine.UI;
using AssemblyCSharp.Assets.Logic.UI.SubComponents;
using AssemblyCSharp.Assets.Logic.InputSystem.Components;

namespace AssemblyCSharp.Assets.Logic.Player
{
    public class PlayerInteractComponent : UnitMonoBehaviour
    {
        public static List<(InteractEntity, GameObject)> Interactions = new List<(InteractEntity, GameObject)>();
        public RectTransform LayoutRoot;
        public Transform Head;
        public RectTransform PlayerDot;

        static Object _buttonObject;
        static Object ButtonObject
        {
            get
            {
                _buttonObject ??= Resources.Load("UI/UI_BUTTON");
                return _buttonObject;
            }
        }

        [FoldoutGroup("UI")] public GameObject InteractionUIObject;
        [FoldoutGroup("UI")] public TMP_Text InteractionInfoText;
        [FoldoutGroup("UI")] public TMP_Text InteractionButtonText;

        private RectTransform[] CacheSizeFitters;
        private static PlayerInteractComponent me;

        private void Start()
        {
            base.Start();
            me = FindFirstObjectByType<PlayerInteractComponent>();
            Interactions.Clear();
            CacheSizeFitters = InteractionUIObject.GetComponentsInChildren<ContentSizeFitter>().Select(x => x.GetComponent<RectTransform>()).ToArray();
        }


        private void LateUpdate()
        {
            base.LateUpdate();
            InteractionObjectsControl();

            DotPosChange();
        }

        public static void InsertInteraction(InteractEntity interact)
        {
            GameObject g = Instantiate(ButtonObject) as GameObject;
            g.transform.SetParent(me.LayoutRoot, false);
            g.GetComponent<Button>().onClick.AddListener(delegate { interact.Action.Invoke(); });

            g.GetComponent<sUI_ButtonWorks>().ChangeText(interact.InfoText);
            g.GetComponent<sUI_ButtonWorks>().ReconnectRightDot(null, false);
            g.GetComponent<sUI_ButtonWorks>().ReconnectLeftDot(me.PlayerDot, true);

            Interactions.Add((interact, g));

            ChangeButtonPositions();
        }
        public static void RemoveInteraction(GameObject from)
        {
            var rint = Interactions
                .Find(x => x.Item1.From == from);
            GameObject.Destroy(rint.Item2);
            Interactions.Remove(rint);

            ChangeButtonPositions();
        }

        private static void ChangeButtonPositions()
        {
            for (int i = 0; i < Interactions.Count; i++)
            {
                Interactions[i].Item2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -30);
                Interactions[i].Item2.GetComponent<sUI_FlyingButtons>().originalPos = new Vector2(0, i * -30);
            }
        }

        private void DotPosChange()
        {
            Vector3 dotPos = Camera.main.WorldToScreenPoint(Head.position);
            dotPos = new Vector3(
                dotPos.x * InputComponent.PlayerInput.AspectCoef.x,
                dotPos.y * InputComponent.PlayerInput.AspectCoef.y,
                0
                );
            PlayerDot.anchoredPosition = dotPos;
        }
        private void InteractionObjectsControl()
        {
            InteractionUIObject.SetActive(Interactions.Count > 0);
            PlayerDot.gameObject.SetActive(InteractionUIObject.activeSelf);
            foreach (var a in Interactions)
                if (a.Item1.From == null)
                {
                    Interactions.Remove(a);
                    GameObject.Destroy(a.Item2);
                    break;
                }
            
        }
    }
}
