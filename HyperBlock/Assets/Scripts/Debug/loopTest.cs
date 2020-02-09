using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopTest : MonoBehaviour
{
    private int x = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("0.2 % 2 =" + 0.2f % 2);
        Debug.Log("0.1 % 0.2 =" + 0.1f % 0.2f);
        Debug.Log("0.25 % 0.2 =" + 0.25f % 0.2f);
        Debug.Log("0.4 % 0.2 =" + 0.4f % 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
        while(x < 20)
        {
            x++;
            Debug.Log(x);
        }
    }
}
