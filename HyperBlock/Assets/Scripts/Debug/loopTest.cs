﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopTest : MonoBehaviour
{
    private int x = 0;
    // Start is called before the first frame update
    void Start()
    {
        
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
