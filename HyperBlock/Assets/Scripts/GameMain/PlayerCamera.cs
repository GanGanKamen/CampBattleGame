using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    public Player Player { get { return player; } }

    [SerializeField] private Player player;
    [SerializeField] private Text playerName;
    [SerializeField] private Text characterInfo;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineVirtualCamera startCamera;
    // Start is called before the first frame update
    public void Init(Player _player)
    {
        player = _player;
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.GetComponent<BlockMan>().lookat;
    }

    public void CameraStartAction()
    {
        startCamera.gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            playerName.text = "P" + Player.PlayerNum.ToString();
            characterInfo.text = Player.Damage + "%" + "\n" + "Blocks *" + Player.MyBlockNum;
        }

    }
}
