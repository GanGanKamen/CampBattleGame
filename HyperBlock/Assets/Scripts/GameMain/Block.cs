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

    public CharacterBase whos = null;
    public Status status = Status.Normal;
    public bool isStepOn = false;

    [SerializeField] private GameObject mark;

    // Start is called before the first frame update
    public void ColorChange(CharacterBase character)
    {
        if (status != Status.Normal) return;
        if (whos != null && whos != character)
        {
            whos = null;
            GetComponent<Renderer>().material.color = Color.white;
        }
        else if (whos == null)
        {
            whos = character;
            GetComponent<Renderer>().material.color = character.color;
            character.MyBlockNum++;
        }
    }

    public void MarkSwitch(bool onoff)
    {
        if (onoff)
        {
            mark.gameObject.SetActive(true);
        }
        else
        {
            mark.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        whos = null;
        status = Status.Normal;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isStepOn == false) isStepOn = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isStepOn == true) isStepOn = false;
        }
    }
}
