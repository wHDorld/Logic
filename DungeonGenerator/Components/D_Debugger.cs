using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    public class D_Debugger : DungeonBehaviour
    {
        public override IEnumerator Generate()
        {
            int cnt = 0;
            for (int y = 0; y < dataHandler.DungeonProperties.Dungeon.GetLength(1); y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Dungeon.GetLength(0); x++)
                    if (dataHandler.DungeonProperties.Dungeon[x,y] != null)
                        if (dataHandler.DungeonProperties.Dungeon[x, y].IsMess) cnt++;
            Debug.Log("Mess : " + cnt);

            cnt = 0;
            for (int y = 0; y < dataHandler.DungeonProperties.Dungeon.GetLength(1); y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Dungeon.GetLength(0); x++)
                    if (dataHandler.DungeonProperties.Dungeon[x, y] != null)
                        if (dataHandler.DungeonProperties.Dungeon[x, y].IsRoom) cnt++;
            Debug.Log("Room : " + cnt);

            cnt = 0;
            for (int y = 0; y < dataHandler.DungeonProperties.Dungeon.GetLength(1); y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Dungeon.GetLength(0); x++)
                    if (dataHandler.DungeonProperties.Dungeon[x, y] != null)
                        if (dataHandler.DungeonProperties.Dungeon[x, y].IsLongCorridor) cnt++;
            Debug.Log("LC : " + cnt);

            cnt = 0;
            for (int y = 0; y < dataHandler.DungeonProperties.Dungeon.GetLength(1); y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Dungeon.GetLength(0); x++)
                    if (dataHandler.DungeonProperties.Dungeon[x, y] != null)
                        if (dataHandler.DungeonProperties.Dungeon[x, y].IsCross) cnt++;
            Debug.Log("Cross : " + cnt);

            cnt = 0;
            for (int y = 0; y < dataHandler.DungeonProperties.Dungeon.GetLength(1); y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Dungeon.GetLength(0); x++)
                    if (dataHandler.DungeonProperties.Dungeon[x, y] != null)
                        if (dataHandler.DungeonProperties.Dungeon[x, y].IsDoorhole) cnt++;
            Debug.Log("DH : " + cnt);

            cnt = 0;
            for (int y = 0; y < dataHandler.DungeonProperties.Dungeon.GetLength(1); y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Dungeon.GetLength(0); x++)
                    if (dataHandler.DungeonProperties.Dungeon[x, y] != null)
                        if (dataHandler.DungeonProperties.Dungeon[x, y].IsRoomExit) cnt++;
            Debug.Log("RE : " + cnt);

            cnt = 0;
            for (int y = 0; y < dataHandler.DungeonProperties.Dungeon.GetLength(1); y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Dungeon.GetLength(0); x++)
                    if (dataHandler.DungeonProperties.Dungeon[x, y] != null)
                        if (dataHandler.DungeonProperties.Dungeon[x, y].IsDeadend) cnt++;
            Debug.Log("DE : " + cnt);

            yield return null;
        }
    }
}
