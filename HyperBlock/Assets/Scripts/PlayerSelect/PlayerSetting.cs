using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    [SerializeField]
    NextSceneSingleton nextSceneSingleton;
    [SerializeField] CharacterButton[] buttons;

    [SerializeField] private bool[] keyTrigger = new bool[2];
    [SerializeField] private int[] nowSelected = new int[2];
    [SerializeField] private string[] blockManNames = new string[2];
    [SerializeField] private GameObject nextText;
    // Start is called before the first frame update
    private void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Cancel();
        }
        buttons[0].Select(1, Color.red);
        buttons[1].Select(2, Color.blue);
        nowSelected[0] = 0;
        nowSelected[1] = 1;
        for (int i = 0; i < keyTrigger.Length; i++)
        {
            keyTrigger[i] = false;
        }
        for (int i = 0; i < blockManNames.Length; i++)
        {
            blockManNames[i] = "nothing";
        }
        nextText.SetActive(false);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        KeyCtrl();
    }

    private void KeyCtrl()
    {
        for (int i = 0; i < keyTrigger.Length; i++)
        {
            if (blockManNames[i] == "nothing")
            {
                if (Dualshock4.LeftStick(i + 1).x != 0 && keyTrigger[i] == false)
                {
                    if (Dualshock4.LeftStick(i + 1).x > 0)
                    {
                        if (nowSelected[i] < buttons.Length - 1)
                        {
                            if (buttons[nowSelected[i] + 1].selected != 0)
                            {
                                if (nowSelected[i] + 2 <= buttons.Length - 1)
                                {
                                    buttons[nowSelected[i]].Cancel();
                                    switch (i)
                                    {
                                        case 0:
                                            buttons[nowSelected[i] + 2].Select(i + 1, Color.red);
                                            break;
                                        case 1:
                                            buttons[nowSelected[i] + 2].Select(i + 1, Color.blue);
                                            break;
                                    }
                                    nowSelected[i] += 2;
                                }
                            }
                            else
                            {
                                buttons[nowSelected[i]].Cancel();
                                switch (i)
                                {
                                    case 0:
                                        buttons[nowSelected[i] + 1].Select(i + 1, Color.red);
                                        break;
                                    case 1:
                                        buttons[nowSelected[i] + 1].Select(i + 1, Color.blue);
                                        break;
                                }
                                nowSelected[i] += 1;
                            }
                        }
                    }
                    else if (Dualshock4.LeftStick(i + 1).x < 0)
                    {
                        if (nowSelected[i] > 0)
                        {
                            if (buttons[nowSelected[i] - 1].selected != 0)
                            {
                                if (nowSelected[i] - 2 >= 0)
                                {
                                    buttons[nowSelected[i]].Cancel();
                                    switch (i)
                                    {
                                        case 0:
                                            buttons[nowSelected[i] - 2].Select(i + 1, Color.red);
                                            break;
                                        case 1:
                                            buttons[nowSelected[i] - 2].Select(i + 1, Color.blue);
                                            break;
                                    }
                                    nowSelected[i] -= 2;
                                }
                            }
                            else
                            {
                                buttons[nowSelected[i]].Cancel();
                                switch (i)
                                {
                                    case 0:
                                        buttons[nowSelected[i] - 1].Select(i + 1, Color.red);
                                        break;
                                    case 1:
                                        buttons[nowSelected[i] - 1].Select(i + 1, Color.blue);
                                        break;
                                }
                                nowSelected[i] -= 1;
                            }
                        }
                    }
                    keyTrigger[i] = true;
                }
                else if (Dualshock4.LeftStick(i + 1).x == 0)
                {
                    if (keyTrigger[i] == true) keyTrigger[i] = false;
                }
                if (Dualshock4.CircleDown(i + 1))
                {
                    blockManNames[i] = buttons[nowSelected[i]].blockManName;
                    switch (i)
                    {
                        case 0:
                            buttons[nowSelected[i]].Decision(i + 1, Color.red);
                            break;
                        case 1:
                            buttons[nowSelected[i]].Decision(i + 1, Color.blue);
                            break;
                    }

                }
            }
        }

        int num = 0;
        foreach (string name in blockManNames)
        {
            if (name != "nothing") num += 1;
        }
        if (num == blockManNames.Length)
        {
            nextText.SetActive(true);
            if (Dualshock4.OptionsDown(0))
            {
                nextSceneSingleton.NextScene2P(blockManNames[0], blockManNames[1]);
                Fader.FadeIn(2f, "2P");
            }
        }
    }
}
