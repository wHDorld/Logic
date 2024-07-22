using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using System.Collections;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    public class D_PlayerSet : DungeonBehaviour
    {
        public Object PlayerObject;

        public override IEnumerator Generate()
        {
            GameObject player = Instantiate(PlayerObject) as GameObject;
            List<DungeonMapCell> rooms = new List<DungeonMapCell>();
            foreach (var a in dataHandler.DungeonProperties.Dungeon)
                if (a != null && a.IsRoom)
                    rooms.Add(a);
            if (rooms.Count == 0)
                foreach (var a in dataHandler.DungeonProperties.Dungeon)
                    if (a != null)
                        rooms.Add(a);
            var spawn_room = rooms[Random.Range(0, rooms.Count)];

            player.GetComponent<CharacterController>().enabled = false;
            if (!SaveSystem.Singletone.SaveSystem.Load_Preparation().IsInDungeon)
            {
                player.transform.position =
                    dataHandler.DungeonProperties.DungeonMapCellGameObjects[spawn_room.PosX, spawn_room.PosY].transform.position
                    + dataHandler.DungeonProperties.Preset.CoridorCenter
                    + new Vector3(0, dataHandler.DungeonProperties.Preset.DungeonCellScale + 25, 0);
            }
            else
                player.transform.position = SaveSystem.Singletone.SaveSystem.Load_Preparation().PlayerPosition;
            player.GetComponent<CharacterController>().enabled = true;
            SaveSystem.Singletone.SaveSystem.Load_Preparation().PlayerPosition = player.transform.position;
            yield return null;
        }
    }
}
