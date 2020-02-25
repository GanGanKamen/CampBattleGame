using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    public enum Mode
    {
        OnePlayer,
        TwoPlayer,
    }

    [SerializeField]
    NextSceneSingleton nextSceneSingleton;
    [SerializeField] CharacterButton[] buttons;

    [SerializeField] private bool[] keyTrigger = new bool[2];
    [SerializeField] private int[] nowSelected = new int[2];
    [SerializeField] private string[] blockManNames = new string[2];
    [SerializeField] private GameObject nextText;

    private Mode mode;
    // Start is called before the first frame update
    private void Awake()
    {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        KeyCtrl();
    }

    public void Init(Mode _mode)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Cancel();
        }
        switch (_mode)
        {
            case Mode.OnePlayer:
                keyTrigger = new bool[1];
                nowSelected = new int[1];
                blockManNames = new string[1];
                buttons[0].Select(0, Color.red);
                nowSelected[0] = 0;
                break;
            case Mode.TwoPlayer:
                keyTrigger = new bool[2];
                nowSelected = new int[2];
                blockManNames = new string[2];
                buttons[0].Select(1, Color.red);
                buttons[1].Select(2, Color.blue);
                nowSelected[0] = 0;
                nowSelected[1] = 1;
                break;
        }

        for (int i = 0; i < keyTrigger.Length; i++)
        {
            keyTrigger[i] = false;
        }
        for (int i = 0; i < blockManNames.Length; i++)
        {
            blockManNames[i] = "nothing";
        }
        mode = _mode;
        nextText.SetActive(false);
    }

    private void KeyCtrl()
    {
        switch (mode)
        {
            case Mode.OnePlayer:
                if (blockManNames[0] == "nothing")
                {
                    if (Dualshock4.LeftStick(0).x != 0 && keyTrigger[0] == false)
                    {
                        if (Dualshock4.LeftStick(0).x > 0)
                        {
                            if (nowSelected[0] < buttons.Length - 1)
                            {
                                buttons[nowSelected[0]].Cancel();
                                buttons[nowSelected[0] + 1].Select(0, Color.red);
                                nowSelected[0] += 1;
                            }
                        }
                        else if (Dualshock4.LeftStick(0).x < 0)
                        {
                            if (nowSelected[0] > 0)
                            {
                                buttons[nowSelected[0]].Cancel();
                                buttons[nowSelected[0] - 1].Select(0, Color.red);
                                nowSelected[0] -= 1;
                            }
                        }
                        keyTrigger[0] = true;
                    }
                    else if (Dualshock4.LeftStick(0).x == 0)
                    {
                        if (keyTrigger[0] == true) keyTrigger[0] = false;
                    }
                    if (Dualshock4.CircleDown(0))
                    {
                        blockManNames[0] = buttons[nowSelected[0]].blockManName;
                        buttons[nowSelected[0]].Decision(0, Color.red);

                    }
                }
                    break;
            case Mode.TwoPlayer:
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
                break;
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
                switch (mode)
                {
                    case Mode.OnePlayer:
                        nextSceneSingleton.NextScene1P(blockManNames[0]);
                        Fader.FadeIn(2f, "VsCOM");
                        break;
                    case Mode.TwoPlayer:
                        nextSceneSingleton.NextScene2P(blockManNames[0], blockManNames[1]);
                        Fader.FadeIn(2f, "2P");
                        break;
                }
                
            }
        }
    }
}
