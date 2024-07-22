using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.Dialogue.Components
{
    public class DialogueSystemComponent : MonoBehaviour
    {
        public GameObject MainUI;
        public GameObject DialogueUI;

        static DialogueSystemComponent me;

        private void Start()
        {
            me = GameObject.FindAnyObjectByType<DialogueSystemComponent>();
        }

        public static void StartDialogue()
        {
            me.MainUI.SetActive(false);
            me.DialogueUI.SetActive(true);
        }
        public static void EndDialogue()
        {
            me.MainUI.SetActive(true);
            me.DialogueUI.SetActive(false);
        }
    }
}
