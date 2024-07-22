using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_gpui : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(stst());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator stst()
    {
        while (true)
        {
            Debug.Log("23143");
            GetComponent<Renderer>().materials[0].SetFloat("Vector1_c833fff4d044426ebdb6d3b88ab28cb5", Random.value);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
