using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using AssemblyCSharp.Assets.Logic.AI.Entities;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Logic.DungeonGenerator.Components;
using UnityEngine.Rendering.Universal;

namespace AssemblyCSharp.Assets.Logic.AI.Components
{
    public class EnemySpawner : MonoBehaviour
    {
        public SpawnProperties[] Spawn;
        public float Distance = 4;
        public int Repeat = 1;

        private Transform player;
        private bool alreadyExist;

        private IEnumerator Start()
        {
            if (GameObject.FindGameObjectWithTag("Player"))
                player = GameObject.FindGameObjectWithTag("Player").transform;

            if (GetComponentInChildren<DecalProjector>() != null)
                GetComponentInChildren<DecalProjector>().size = Vector3.one * Distance * 2f;
        
            while (true)
            {
                yield return new WaitForSeconds(1f);

                if (player == null && GameObject.FindGameObjectWithTag("Player"))
                    player = GameObject.FindGameObjectWithTag("Player").transform;

                if (PlayerDistance < Distance)
                    DoSpawn();
            }
        }

        private void DoSpawn()
        {
            if (alreadyExist)
                return;
            var coridors = GameObject.FindGameObjectsWithTag("Coridor")
                .Where(x => Vector3.Distance(x.transform.position, player.position) < Distance * 2f)
                .OrderBy(x => Vector3.Distance(x.transform.position, player.position))
                .Skip(1);
            int split = coridors.Count() / 2;
            var farCoridors = coridors.Take(split);
            var nearCoridors = coridors.TakeLast(split);

            for (int i = 0; i < Repeat; i++) 
            {
                foreach (var a in Spawn)
                    SetObject(a, coridors, farCoridors, nearCoridors);
            }
            alreadyExist = true;


            if (GetComponentInChildren<DecalProjector>() != null)
                Destroy(GetComponentInChildren<DecalProjector>().gameObject);
        }
        private void SetObject(
            SpawnProperties properties, 
            IEnumerable<GameObject> allCoridors,
            IEnumerable<GameObject> farCoridors,
            IEnumerable<GameObject> nearCoridors)
        {
            GameObject g = Instantiate(properties.Spawn) as GameObject;
            g.SetActive(false);

            ESpawnDistance currentDistance = properties.Distance;
            if (currentDistance == ESpawnDistance.Near && nearCoridors.Count() == 0) currentDistance = ESpawnDistance.Random;
            if (currentDistance == ESpawnDistance.Far && farCoridors.Count() == 0) currentDistance = ESpawnDistance.Random;

            switch (currentDistance)
            {
                case ESpawnDistance.Random:
                    g.transform.position = allCoridors.ToList()[Random.Range(0, allCoridors.Count())].transform.position;
                    break;
                case ESpawnDistance.Far:
                    g.transform.position = farCoridors.ToList()[Random.Range(0, farCoridors.Count())].transform.position;
                    break;
                case ESpawnDistance.Near:
                    g.transform.position = nearCoridors.ToList()[Random.Range(0, nearCoridors.Count())].transform.position;
                    break;
            }

            g.transform.position += new Vector3(-1, 1, 1) * FindFirstObjectByType<DungeonDataHandler>().dungeonPreset.DungeonCellScale / 2f;
            g.SetActive(true);
        }

        private float PlayerDistance
        {
            get
            {
                if (player != null)
                    return Vector3.Distance(player.position, transform.position);
                else
                    return float.MaxValue;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 1, 0.1f);
            Gizmos.DrawSphere(transform.position, Distance);
        }
    }
}
