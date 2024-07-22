using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Components;
using AssemblyCSharp.Assets.Logic.Unit.Components;

namespace AssemblyCSharp.Assets.Logic.Minimap.Components
{
    public class MinimapComponent : MonoBehaviour
    {
        public RectTransform MapRect;
        public RectTransform PlayerCursor;
        public Transform Player;
        public Transform MapParent;
        public int MapSize = 500;


        private float worldScale;

        private void Start()
        {
            if (DungeonDataHandler._dungeonProperties == null)
                return;
            worldScale = 
                DungeonDataHandler._dungeonProperties.Preset.DungeonCellScaleFactor 
                * DungeonDataHandler._dungeonProperties.Preset.DungeonSize
                * DungeonDataHandler._dungeonProperties.Preset.DungeonScaleFactor;

            MapRect.GetComponent<Image>().sprite = Sprite.Create(
                D_MapGenerator.Map, 
                new Rect(0, 0, D_MapGenerator.Map.width, D_MapGenerator.Map.height), 
                Vector2.zero
                );
            MapRect.sizeDelta = new Vector2(MapSize, MapSize);
        }

        private void FixedUpdate()
        {
            if (DungeonDataHandler._dungeonProperties == null)
                return;
            SetPlayerPosition();
            SetEnemiesPosition();
        }

        private void SetPlayerPosition()
        {
            MapRect.localPosition = new Vector2(
                -(Player.position.x / worldScale) * MapSize,
                -(Player.position.z / worldScale) * MapSize
                );
            PlayerCursor.localRotation = Quaternion.Euler(0, 0, -Player.transform.eulerAngles.y) * Quaternion.Euler(0, 0, 90);
        }

        private void SetEnemiesPosition()
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                Vector2 mapPos = new Vector2(
                    (Enemies[i].transform.position.x / worldScale) * MapSize,
                    (Enemies[i].transform.position.z / worldScale) * MapSize
                    ) + MapRect.anchoredPosition;
                enemiesCursors[i].anchoredPosition = mapPos;
                enemiesCursors[i].localRotation = Quaternion.Euler(0, 0, -Enemies[i].transform.eulerAngles.y) * Quaternion.Euler(0, 0, 90);
            }
        }

        public static List<UnitStatContainer> Enemies = new List<UnitStatContainer>();
        private static List<RectTransform> enemiesCursors = new List<RectTransform>();
        public static void AddEnemy(UnitStatContainer enemy)
        {
            Enemies.Add(enemy);
            enemiesCursors.Add(getEnemyCursor);
        }
        public static void RemoveEnemy(UnitStatContainer enemy)
        {
            Enemies.Remove(enemy);
            Destroy(enemiesCursors[0].gameObject);
            enemiesCursors.RemoveAt(0);
        }

        private static Object enemyCursor;
        private static RectTransform getEnemyCursor
        {
            get
            {
                enemyCursor ??= Resources.Load("UI/EnemyMinimapCursor");
                GameObject g = Instantiate(enemyCursor) as GameObject;
                g.transform.SetParent(FindAnyObjectByType<MinimapComponent>().MapParent, false);
                return g.GetComponent<RectTransform>();
            }
        }
    }
}
