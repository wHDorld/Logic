using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class E_SnowRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<DecalProjector>().material = new Material(GetComponent<DecalProjector>().material);
        GetComponent<DecalProjector>().material.SetFloat("_Power", Random.Range(0.1f, 1f));
        GetComponent<DecalProjector>().material.SetFloat("_Density", Random.Range(1f, 18f));
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
