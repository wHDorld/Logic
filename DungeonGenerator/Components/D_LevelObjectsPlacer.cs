using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Abstract;
using System.Collections;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Enums;

namespace AssemblyCSharp.Assets.Logic.DungeonGenerator.Components
{
    [RequireComponent(typeof(D_Overpaint))]
    public class D_LevelObjectsPlacer : DungeonBehaviour
    {
        public Object EscapePod;
        public Object NextLevelPod;
        public Object Slave;

        D_Overpaint Overpaint;

        public override IEnumerator Generate()
        {
            Overpaint = GetComponent<D_Overpaint>();

            PlaceEscapePod();
            PlaceNextLevelPod();
            PlaceSlave();

            return base.Generate();
        }

        private Vector3 FindActualPoint(ColorType type)
        {
            var point = Overpaint.GetPointByType(type);
            var coridors = gameObject
                .GetComponentsInChildren<Transform>()
                .Where(x => x.tag == "Coridor")
                .Select(x => x.position + dataHandler.DungeonProperties.Preset.CoridorCenter)
                .Where(x => Vector3.Distance(x,  point.Item1) < point.Item2)
                .OrderBy(x => Vector3.Distance(x, point.Item1))
                .ToList();

            if (coridors.Count == 0)
            {
                coridors = gameObject
                    .GetComponentsInChildren<Transform>()
                    .Where(x => x.tag == "Coridor")
                    .Select(x => x.position + dataHandler.DungeonProperties.Preset.CoridorCenter)
                    .OrderBy(x => Vector3.Distance(x, point.Item1))
                    .ToList();
            }

            return coridors[Random.Range(0, coridors.Count)];
        }

        private void PlaceEscapePod()
        {
            Vector3 pos = FindActualPoint(ColorType.Escape);

            GameObject g = Instantiate(EscapePod) as GameObject;
            g.transform.position = pos;
        }
        private void PlaceNextLevelPod()
        {
            Vector3 pos = FindActualPoint(ColorType.NextLevel);

            GameObject g = Instantiate(NextLevelPod) as GameObject;
            g.transform.position = pos;
        }
        private void PlaceSlave()
        {
            Vector3 pos = FindActualPoint(ColorType.Slave);

            GameObject g = Instantiate(Slave) as GameObject;
            g.transform.position = pos;
        }
    }
}
