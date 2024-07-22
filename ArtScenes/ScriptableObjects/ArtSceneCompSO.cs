using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using AssemblyCSharp.Assets.Logic.ArtScenes.Entities;

namespace AssemblyCSharp.Assets.Logic.ArtScenes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Default Art Scene", menuName = "Create Art Scene")]
    public class ArtSceneCompSO : ScriptableObject
    {
        public string Name;

        [TableList(ShowIndexLabels = true, ShowPaging = true)]
        public List<FrameObject> FrameObjects = new List<FrameObject>();

        public bool IsRepeated(int frameNumber, int id)
        {
            var range = RepeatRange(id);
            return frameNumber <= range[1] && frameNumber >= range[0];
        }
        public int[] RepeatRange(int id)
        {
            int start = -1;
            int end = -1;
            for (int i = 0; i < FrameObjects.Count; i++)
            {
                if (FrameObjects[i].id != id)
                    continue;
                if (FrameObjects[i].FrameType == Enums.FrameType.RepeatStart)
                    start = i;
                if (FrameObjects[i].FrameType == Enums.FrameType.RepeatEnd)
                    end = i;
                if (start != -1 && end != -1)
                    break;
            }
            return new int[2] { start, end };
        }
        public int[] SequenceRange(int id)
        {
            int start = -1;
            int end = FrameObjects.Count;
            for (int i = 0; i < FrameObjects.Count; i++)
            {
                if (FrameObjects[i].id == id - 1)
                {
                    if (FrameObjects[i].FrameType == Enums.FrameType.RepeatEnd)
                        start = i;
                }
                if (FrameObjects[i].id == id)
                {
                    if (FrameObjects[i].FrameType == Enums.FrameType.RepeatStart)
                        end = i;
                }
            }
            return new int[2] { start + 1, end - 1 };
        }
    }
}
