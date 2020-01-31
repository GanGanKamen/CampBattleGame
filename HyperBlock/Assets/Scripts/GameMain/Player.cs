using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public int PlayerNum { get { return dualshock4Num; } }

    [SerializeField][Range(0,4)] private int dualshock4Num;

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

    public void SetControllerNum(int num)
    {
        dualshock4Num = num;
    }

    private void ControllerOperate()
    {
        if (Dualshock4.LeftStick(dualshock4Num).magnitude > 0)
        {
            //var cameraVec = transform.position - playerCamera.transform.position;
            //var cameraForward = Vector3.Scale(playerCamera.forward, new Vector3(1, 0, 1)).normalized;
            //var cameraRight = Vector3.Scale(playerCamera.right, new Vector3(1, 0, 1)).normalized;
            //var moveDirection = (cameraForward * Dualshock4.LeftStick(dualshock4Num).y + cameraRight * Dualshock4.LeftStick(dualshock4Num).x).normalized;
            var moveDirection = new Vector3(Dualshock4.LeftStick(dualshock4Num).x, 0, Dualshock4.LeftStick(dualshock4Num).y).normalized;
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
