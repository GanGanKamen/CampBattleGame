﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public enum Status
    {
        Ready,
        Normal,
        Collect,
        Attack,
        Damage,
        Down
    }
    public Status CharacterStatus { get { return status; } }
    public int MyBlockNum = 0;
    public int Damage { get { return damege; } }
    public string BlockManName { set; get; }
    public float moveSpeed { set; get; }
    public GameObject body { set; get; }
    public Color color { set; get; }
    public float blockChangerDis { set; get; }
    public float blockChangePeriod { set; get; }
    public float blockCollectPeriod { set; get; }
    public Transform hand { set; get; }
    public int maxCollectNum { set; get; }
    public float attackSpeed { set; get; }
    public float attackSaveTime { set; get; }
    public float coolDownTime { set; get; }
    public Vector3 MoveDirection{ get { return movedirection; } }
    public bool Invincible { get { return invincible; } }
    public bool IsDeath { get { return isDeath; } }

    [SerializeField] public AttackObj myAttack = null;
    [SerializeField] private Status status = Status.Ready;
    private float changeBlockCount = 0;
    [SerializeField]private float collectBlockCount = 0;
    [SerializeField]private Block targetBlock = null;

    private float coolDownCount = 0;
    private int damege = 0;
    private float recorveryCount = 0;
    private Vector3 hitDirection = Vector3.zero;
    private float realCoolDownTime = 0;
    private float realRecorveryTime = 1;
    private Vector3 movedirection = Vector3.zero;
    private bool invincible = false;
    private float invincibleCount = 0;
    private float invicibleTime = 4;
    private bool isDeath = false;
    private float realSpeed = 0;

    public void CharacterMove(Vector3 _direction)
    {
        if (status == Status.Attack || status == Status.Damage || status == Status.Ready) return;
        var direction = new Vector3(_direction.x, 0, _direction.z).normalized;
        transform.Translate(direction * Time.deltaTime * realSpeed);
        body.transform.localRotation = Quaternion.LookRotation(direction);
        movedirection = direction;
    }

    public void ResetMoveDirection()
    {
        movedirection = Vector3.forward;
    }

    public void ReadyToStart()
    {
        status = Status.Normal;
    }

    public void GameStop()
    {
        status = Status.Ready;
    }

    public void StatusUpdate()
    {
        InvincibleState();
        switch (status)
        {
            case Status.Ready:
                GetComponent<BlockMan>().Charge(false);
                GetComponent<BlockMan>().Collect(false);
                break;
            case Status.Normal:
                GetComponent<BlockMan>().Charge(true);
                GetComponent<BlockMan>().Collect(false);
                ChangeBlock(blockChangePeriod);
                break;
            case Status.Collect:
                GetComponent<BlockMan>().Charge(false);
                GetComponent<BlockMan>().Collect(true);
                break;
            case Status.Attack:
                GetComponent<BlockMan>().Charge(false);
                GetComponent<BlockMan>().Collect(false);
                if (coolDownCount < realCoolDownTime)
                {
                    coolDownCount += Time.deltaTime;
                }
                else
                {
                    coolDownCount = 0;
                    status = Status.Normal;
                }
                break;
            case Status.Damage:
                GetComponent<BlockMan>().Charge(false);
                GetComponent<BlockMan>().Collect(false);
                if (recorveryCount < realRecorveryTime)
                {
                    recorveryCount += Time.deltaTime;
                    transform.position += hitDirection * 5f * Time.deltaTime;
                }
                else
                {
                    recorveryCount = 0;
                    invincible = true;
                    status = Status.Normal;
                }
                break;
            case Status.Down:
                GetComponent<BlockMan>().Charge(false);
                GetComponent<BlockMan>().Collect(false);
                break;
        }
    }

    private void InvincibleState()
    {
        if (invincible)
        {
            if (invincibleCount <= invicibleTime)
            {
                invincibleCount += Time.deltaTime;
                if (invincibleCount % 0.2 >= 0.1)
                {
                    body.transform.localScale = Vector3.one;
                }
                else
                {
                    body.transform.localScale = Vector3.zero;
                }
            }
            else
            {
                invincibleCount = 0;
                invincible = false;
            }
        }
        else
        {
            if (body.transform.localScale == Vector3.zero) body.transform.localScale = Vector3.one;
        }
    }

    public void ChangeBlock(float period)
    {
        if (changeBlockCount < period)
        {
            changeBlockCount += Time.deltaTime;
        }
        else
        {
            var stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
            foreach (Block block in stageMng.blockList)
            {
                var dis = Vector3.Distance(block.gameObject.transform.position, transform.position);
                if (dis <= blockChangerDis)
                {
                    block.ColorChange(this);
                }
            }
            changeBlockCount = 0;
        }
    }



    public void CollectBlock(float period)
    {
        if (status == Status.Attack || status == Status.Damage)
        {
            changeBlockCount = 0;
            return;
        }
        if (MyBlockNum <= 0)
        {
            changeBlockCount = 0;
            status = Status.Normal;
            return;
        }
        if (status != Status.Collect)
        {
            collectBlockCount = 0;
            status = Status.Collect;
            var stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
            targetBlock = FurthestBlock(stageMng);
            targetBlock.MarkSwitch(true);
            var efe = Instantiate(ResourcesMng.ResourcesLoad("CubeEffect"), targetBlock.transform.position, Quaternion.identity);
            efe.GetComponent<BlockEffect>().Init(this, color);
            var attackObj = new GameObject();
            attackObj.AddComponent<AttackObj>();
            myAttack = attackObj.GetComponent<AttackObj>();
            myAttack.Init(this);
        }
        else
        {
            if (collectBlockCount < period)
            {
                collectBlockCount += Time.deltaTime;
            }
            else
            {
                if (myAttack.CollectNum < maxCollectNum)
                {
                    if (targetBlock.isStepOn == false && targetBlock.whos == this)
                    {
                        myAttack.BlockRegister(targetBlock);
                        targetBlock.MarkSwitch(false);
                        MyBlockNum--;
                    }

                    var stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
                    targetBlock = FurthestBlock(stageMng);
                    targetBlock.MarkSwitch(true);
                    var efe = Instantiate(ResourcesMng.ResourcesLoad("CubeEffect"), targetBlock.transform.position, Quaternion.identity);
                    efe.GetComponent<BlockEffect>().Init(this, color);
                }
                else
                {
                    if (targetBlock != null)
                    {
                        targetBlock.MarkSwitch(false);
                        targetBlock = null;
                    }


                }
                collectBlockCount = 0;
            }
        }
        
    }

    public void CollectBlockCancel()
    {
        if(status != Status.Damage) status = Status.Normal;
        collectBlockCount = 0;
        if (targetBlock != null)
        {
            targetBlock.MarkSwitch(false);
            targetBlock = null;
        }
        if (myAttack != null)
        {
            Destroy(myAttack.gameObject);
            myAttack = null;
        }
    }

    public void BlockAttack()
    {
        if (myAttack.CollectNum <= 0)
        {
            CollectBlockCancel();
        }
        else
        {
            if (targetBlock != null)
            {
                targetBlock.MarkSwitch(false);
                targetBlock = null;
            }
            realCoolDownTime = coolDownTime + (myAttack.CollectNum - 1) / 2f;
            status = Status.Attack;
            collectBlockCount = 0;
            var direction = Vector3.Scale((hand.transform.position - transform.position), new Vector3(1, 0, 1));
            myAttack.Attack(direction.normalized, attackSpeed, attackSaveTime);
            myAttack = null;
        }

    }

    private Block FurthestBlock(StageMng stageMng)
    {
        var sortList = new List<Block>();
        foreach (Block block in stageMng.blockList)
        {
            if (block.whos == this) sortList.Add(block);
        }
        sortList.Sort((a, b) => (int)Vector3.Distance(b.transform.position, transform.position) -
        (int)Vector3.Distance(a.transform.position, transform.position));
        return sortList[0];
    }

    public void Death()
    {
        status = Status.Down;
        isDeath = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            if (invincible) return;
            var attack = other.GetComponent<AttackObj>();
            if (status != Status.Ready && status != Status.Damage && attack.Master != this)
            {
                status = Status.Damage;
                CollectBlockCancel();
                damege += (attack.CollectNum * 2);
                realRecorveryTime = 1 + (damege / 100f);
                hitDirection = attack.Direction;
                var stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
                stageMng.AttackObjDelate(attack);
            }

        }

        if (other.CompareTag("Death"))
        {
            var stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
            stageMng.CharacterDeath(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            var block = collision.gameObject.GetComponent<Block>();
            if (block.whos == null) realSpeed = moveSpeed / 1.5f;
            else if (block.whos == this) realSpeed = moveSpeed;
            else realSpeed = moveSpeed / 4;
        }
        if (collision.gameObject.CompareTag("Hole") && status == Status.Damage)
        {
            GetComponent<CapsuleCollider>().isTrigger = true;
            transform.position = new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z);
            Debug.Log("Hole_Death");
        }
        else if(collision.gameObject.CompareTag("Wall") && status == Status.Damage)
        {
            GetComponent<CapsuleCollider>().isTrigger = true;
            Debug.Log("Wall_Death");
        }
    }
}
