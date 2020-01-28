using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    [SerializeField][Range(0,4)] private int dualshock4Num;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private int _hp;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _body;
    [SerializeField] private Color _color;
    [SerializeField] private float _blockChangerDis;
    [SerializeField] private float _blockChangePeriod;
    [SerializeField] private float _blockCollectPeriod;
    [SerializeField] private Transform _hand;
    [SerializeField] private int _maxCollectNum;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackSaveTime;
    [SerializeField] private float _coolDownTime;

    private void Awake()
    {
        hp = _hp;
        moveSpeed = _moveSpeed;
        body = _body;
        color = _color;
        blockChangerDis = _blockChangerDis;
        blockChangePeriod = _blockChangePeriod;
        blockCollectPeriod = _blockCollectPeriod;
        hand = _hand;
        maxCollectNum = _maxCollectNum;
        attackSpeed = _attackSpeed;
        attackSaveTime = _attackSaveTime;
        coolDownTime = _coolDownTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ControllerOperate();
        StatusUpdate();
    }

    private void ControllerOperate()
    {
        if (Dualshock4.LeftStick(dualshock4Num).magnitude > 0)
        {
            //var cameraVec = transform.position - playerCamera.transform.position;
            var cameraForward = Vector3.Scale(playerCamera.forward, new Vector3(1, 0, 1)).normalized;
            var cameraRight = Vector3.Scale(playerCamera.right, new Vector3(1, 0, 1)).normalized;
            var moveDirection = (cameraForward * Dualshock4.LeftStick(dualshock4Num).y + cameraRight * Dualshock4.LeftStick(dualshock4Num).x).normalized;
            CharacterMove(moveDirection);
        }

        if (Dualshock4.R2(dualshock4Num))
        {
            CollectBlock(blockCollectPeriod);
        }

        else if (Dualshock4.R2Up(dualshock4Num))
        {
            if (myAttack == null) return;
            BlockAttack();
        }
    }
}
