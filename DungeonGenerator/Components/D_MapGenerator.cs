using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    public class D_MapGenerator : DungeonBehaviour
    {
        public bool WriteToPng;
        public Camera MinimapCam;
        public static Texture2D Map;

        public override IEnumerator Generate()
        {
            yield return GeneratePhoto();
            //yield return GenerateImage();
            yield return null;
        }

        private IEnumerator GeneratePhoto()
        {
            MinimapCam.transform.position =
                (dataHandler.dungeonPreset.DungeonCellScaleFactor * dataHandler.dungeonPreset.DungeonSize)
                * new Vector3(1, 0, 1) / 2f;
            MinimapCam.transform.position += new Vector3(0, 10, 0);
            MinimapCam.orthographicSize = dataHandler.dungeonPreset.DungeonSize * 2;

            yield return null;

            Map = new Texture2D(MinimapCam.targetTexture.width, MinimapCam.targetTexture.height);
            RenderTexture.active = MinimapCam.targetTexture;
            Map.ReadPixels(new Rect(0, 0, MinimapCam.targetTexture.width, MinimapCam.targetTexture.height), 0, 0);
            Map.Apply();
            MinimapCam.gameObject.SetActive(false);

            if (WriteToPng)
                SaveToFile();
            dataHandler.DungeonProperties.DebugLog("minimapGenerate", 1);
        }
        private IEnumerator GenerateImage()
        {
            Map = new Texture2D(dataHandler.DungeonProperties.Size, dataHandler.DungeonProperties.Size);
            Map.alphaIsTransparency = true;
            Map.filterMode = FilterMode.Point;

            int cnt = 0;
            for (int y = 0; y < dataHandler.DungeonProperties.Size; y++)
                for (int x = 0; x < dataHandler.DungeonProperties.Size; x++)
                {
                    Map.SetPixel(x, y, ColorByCoord(x, y));

                    cnt = (y * dataHandler.DungeonProperties.Size + x);
                    if (cnt % GeneratorTicks == 0)
                    {
                        dataHandler.DungeonProperties.DebugLog("minimapGenerate", cnt / Mathf.Pow(dataHandler.DungeonProperties.Size, 2));
                        yield return null;
                    }
                }
            Map.Apply();
            if (WriteToPng)
                SaveToFile();
            dataHandler.DungeonProperties.DebugLog("minimapGenerate", 1);
        }

        private Color ColorByCoord(int x, int y)
        {
            Color ret = new Color(0, 0, 0, 0);
            if (dataHandler.DungeonProperties.Dungeon[x, y] != null)
                ret = new Color(1, 1, 1, 1);
            return ret;
        }
        private void SaveToFile()
        {
            var _bytes = Map.EncodeToPNG();

            var dirPath = Application.dataPath + "/../SaveImages/";

            if (!Directory.Exists(dirPath))
            {

                Directory.CreateDirectory(dirPath);

            }

            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            File.WriteAllBytes(dirPath + name + timeStamp + ".jpg", _bytes);
        }
    }
}
