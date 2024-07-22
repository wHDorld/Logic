using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Components;

namespace AssemblyCSharp.Assets.Logic.Visual.Components
{
    public class DungeonLoadingScreen : MonoBehaviour
    {
        public DungeonDataHandler dungeonGenerator;
        public RectTransform loadingBar;
        public TMPro.TMP_Text text;

        private void Start()
        {
            dungeonGenerator.DungeonProperties.OnGenerated += DungeonGenerator_OnGenerated;
        }

        private void DungeonGenerator_OnGenerated()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            text.text = dungeonGenerator.DungeonProperties.CurrentGenerationState.ToLower().Replace("generate", "");
            loadingBar.localScale = Vector3.Lerp(
                loadingBar.localScale,
                new Vector3(dungeonGenerator.DungeonProperties.CurrentGenerationProgress, 1, 1),
                15f * Time.deltaTime);
        }
    }
}
