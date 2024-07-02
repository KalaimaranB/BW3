using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wreckage : MonoBehaviour
{
    public List<MeshRenderer> meshesToDarken;
    [Range(0,1)]
    public float darkenValue;
    // Start is called before the first frame update
    void Start()
    {
        Color darkVersion = new Color(darkenValue, darkenValue, darkenValue);
        for (int i = 0;i < meshesToDarken.Count;  i++)
        {
            foreach(Material mm in meshesToDarken[i].materials)
            {
                mm.color = darkVersion;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
