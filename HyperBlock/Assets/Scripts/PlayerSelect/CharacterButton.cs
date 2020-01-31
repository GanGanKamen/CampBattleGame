using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    [SerializeField] Image mark;
    [SerializeField] Text okMark;
    public int selected = 0;
    public string blockManName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Decision(int playerNum, Color color)
    {
        okMark.text = playerNum.ToString() + "P OK";
        okMark.color = color;
    }

    public void Select(int playerNum,Color color)
    {
        selected = playerNum;
        mark.color = color;
    }

    public void Cancel()
    {
        selected = 0;
        mark.color = Color.clear;
        okMark.color = Color.clear;
    }
}
