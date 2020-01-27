using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum Status
    {
        Normal,
        Translate,
        Build
    }

    [HideInInspector] public CharacterBase whos = null;
    [HideInInspector] public Status status = Status.Normal;
    // Start is called before the first frame update
    public void ColorChange(CharacterBase character)
    {
        if (status != Status.Normal) return;
        if(whos != null && whos != character)
        {
            whos = null;
            GetComponent<Renderer>().material.color = Color.white;
        }
        else
        {
            whos = character;
            GetComponent<Renderer>().material.color = character.color;
        }
    }


    private void Awake()
    {
        whos = null;
        status = Status.Normal;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
