using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button onePlayerButton;
    [SerializeField] private Button twoPlayerButton;
    // Start is called before the first frame update
    private void Awake()
    {
        onePlayerButton.onClick.AddListener(() => GoToOnePlayerMode());
        twoPlayerButton.onClick.AddListener(() => GoToTwoPlayerMode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GoToOnePlayerMode()
    {
        Fader.FadeIn(2f, "CharacterSelect");
    }

    private void GoToTwoPlayerMode()
    {
        Fader.FadeIn(2f, "CharacterSelect2P");
    }
}
