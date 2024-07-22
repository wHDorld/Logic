using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using System.Collections;
using Sirenix.OdinInspector;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Entities;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Enums;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    public class D_Overpaint : DungeonBehaviour
    {
        public bool IsDebug;
        [FoldoutGroup("Color")] public Gradient BasicColor;
        [FoldoutGroup("Color")] public DungeonColorProperty[] OverrideColors;
        [FoldoutGroup("Color")] [MinMaxSlider(0, 1f)] public Vector2 RadiusRange;
        [FoldoutGroup("Color")] public Color NoiseColor;
        [FoldoutGroup("Color")] [MinMaxSlider(-1, 1f)] public Vector2 NoiseRange;

        private List<Renderer> RenderObjects = new List<Renderer>();

        Vector3[] colorPoints;
        float[] colorRadius;
        float currentEvaluateColor;
        Bounds bounds;

        public override IEnumerator Generate()
        {
            FindAllRenderedObjects();
            ColorPointPlace();
            yield return ReColorObjects();
        }

        private void FindAllRenderedObjects()
        {
            RenderObjects.AddRange(GameObject.FindGameObjectsWithTag("Coridor")
                .Where(x => x.activeSelf)
                .SelectMany(x => x.GetComponentsInChildren<Renderer>()));
            RenderObjects.AddRange(GameObject.FindGameObjectsWithTag("Environment")
                .Where(x => x.activeSelf)
                .Where(x => x.GetComponent<Renderer>() != null)
                .Select(x => x.GetComponent<Renderer>()));

            foreach (var a in RenderObjects)
                bounds.Encapsulate(a.transform.position);
        }
        private void ColorPointPlace()
        {
            colorPoints = new Vector3[OverrideColors.Length];
            colorRadius = new float[OverrideColors.Length];

            for (int i = 0; i < OverrideColors.Length; i++)
            {
                colorPoints[i] = bounds.center + (new Vector3(
                    Random.Range(-bounds.extents.x, bounds.extents.x),
                    Random.Range(-bounds.extents.y, bounds.extents.y),
                    Random.Range(-bounds.extents.z, bounds.extents.z)
                    ));
                colorRadius[i] = Random.Range(RadiusRange.x, RadiusRange.y) * dataHandler.DungeonProperties.Size;
            }
        }
        private IEnumerator ReColorObjects()
        {
            currentEvaluateColor = Random.value;

            int cnt = 0;
            foreach (var a in RenderObjects)
            {
                cnt++;
                var basicColor = BasicColor.Evaluate(currentEvaluateColor);
                var cols = GetIntersects(a.transform.position);

                float[] mainColor = new float[3];
                float basicColorWeigth = 1f;
                for (int i = 0; i < cols.Count; i++)
                {
                    mainColor[0] += cols[i].Item1.r * cols[i].Item2;
                    mainColor[1] += cols[i].Item1.g * cols[i].Item2;
                    mainColor[2] += cols[i].Item1.b * cols[i].Item2;

                    basicColorWeigth -= cols[i].Item2;
                }
                basicColorWeigth = basicColorWeigth < 0 ? 0 : basicColorWeigth;

                a.materials[0].SetColor(
                    "_BaseColor",
                    new Color(mainColor[0], mainColor[1], mainColor[2], 1)
                    + basicColor * basicColorWeigth
                    + NoiseColor * Random.Range(NoiseRange.x, NoiseRange.y)
                    );

                if (cnt % GeneratorTicks == 0)
                {
                    dataHandler.DungeonProperties.DebugLog("overpaintGenerate", cnt / (float)RenderObjects.Count);
                    yield return null;
                }
            }
            dataHandler.DungeonProperties.DebugLog("overpaintGenerate", 1);
        }

        private List<(Color, float)> GetIntersects(Vector3 position)
        {
            List<(Color, float)> ret = new List<(Color, float)>();

            for (int i = 0; i < colorPoints.Length; i++)
            {
                float d = Vector3.Distance(colorPoints[i], position);
                if (d >= colorRadius[i])
                    continue;
                ret.Add((OverrideColors[i].Color, 1f - d / colorRadius[i]));
            }

            return ret;
        }

        private void OnDrawGizmos()
        {
            if (!IsDebug)
                return;
            Gizmos.color = new Color(1, 1, 1, 0.1f);
            Gizmos.DrawCube(
                bounds.center,
                bounds.size
                );

            if (colorPoints == null)
                return;
            for (int i = 0; i < colorPoints.Length; i++)
            {
                Gizmos.color = OverrideColors[i].Color - new Color(0, 0, 0, 0.8f);
                Gizmos.DrawSphere(
                    colorPoints[i],
                    colorRadius[i]
                    );
            }
        }

        public (Vector3, float) GetPointByType(ColorType type)
        {
            for (int i = 0; i < OverrideColors.Length; i++)
                if (OverrideColors[i].Type == type)
                    return (colorPoints[i], colorRadius[i]);

            return (Vector3.zero, 0);
        }
    }
}
