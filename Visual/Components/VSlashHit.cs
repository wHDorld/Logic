using AssemblyCSharp.Assets.Logic.Unit.Components;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Visual.Components
{
    public class VSlashHit : MonoBehaviour
    {
        public Object slashPrefab;
        public bool IsChild = true;
        public float LifeTime = 1f;
        public float ScaleFactor = 0.4f;
        public Transform Anchor;
        public Vector3 Up = Vector3.up;
        public Vector3 Offset = new Vector3(0, 0.5f, 0);
        public UnitStatContainer StatContainer;

        private void Start()
        {
            StatContainer.OnHit += OnHit;
        }

        private void OnHit()
        {
            GameObject g = Instantiate(slashPrefab) as GameObject;
            Destroy(g, LifeTime);

            g.transform.position = transform.position + Offset;
            g.transform.rotation = Quaternion.LookRotation(Anchor.right, Up);
            g.transform.localScale = Vector3.one * ScaleFactor;
            if (IsChild)
                g.transform.SetParent(transform, true);
        }
    }
}
