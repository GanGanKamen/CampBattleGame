﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColideTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter");
    }
}
