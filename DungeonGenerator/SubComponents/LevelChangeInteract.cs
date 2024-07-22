using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.SubComponents
{
    public class LevelChangeInteract : MonoBehaviour
    {
        public bool ToHub;

        public void NextLevel()
        {
            SaveSystem.Singletone.SaveSystem.Save_LocalInventory();
            SaveSystem.Singletone.SaveSystem.Save_Preparation(false);

            if (ToHub)
                SceneManager.LoadScene("Hub", LoadSceneMode.Single);
            else
                SceneManager.LoadScene("Dungeon", LoadSceneMode.Single);
        }
    }
}
