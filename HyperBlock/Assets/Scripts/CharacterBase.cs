using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public enum Status
    {
        Normal,
        Collect,
        Attack
    }

    public Status CharacterStatus { get { return status; } }
    public int hp { set; get; }
    public float moveSpeed { set; get; }
    public float jumpPower { set; get; }
    public GameObject body { set; get; }
    public Color color { set; get; }
    public float blockChangerDis { set; get; }
    public float blockChangePeriod { set; get; }
    public float blockCollectPeriod { set; get; }
    public Transform hand { set; get; }
    public int maxCollectNum { set; get; }

    private Status status = Status.Normal;
    private float changeBlockCount = 0;
    private float collectBlockCount = 0;
    private int collectNum = 0;

    public void CharacterMove(Vector3 _direction)
    {
        var direction = new Vector3(_direction.x, 0, _direction.z).normalized;
        transform.Translate(direction * Time.deltaTime * moveSpeed);
        body.transform.localRotation = Quaternion.LookRotation(direction);
    }

    public void StatusUpdate()
    {
        switch (status)
        {
            case Status.Normal:
                ChangeBlock(blockChangePeriod);
                break;
            case Status.Collect:

                break;
        }
    }

    public void ChangeBlock(float period)
    {
        if(changeBlockCount < period)
        {
            changeBlockCount += Time.deltaTime;
        }
        else
        {
            var stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
            foreach(Block block in stageMng.blockList)
            {
                var dis = Vector3.Distance(block.gameObject.transform.position, transform.position);
                if(dis <= blockChangerDis)
                {
                    block.ColorChange(this);
                }
            }
            changeBlockCount = 0;
        }
    }



    public void CollectBlock(float period)
    {
        if (status != Status.Collect)
        {
            collectBlockCount = 0;
            status = Status.Collect;
        }
        if(collectBlockCount < period)
        {
            collectBlockCount += Time.deltaTime;
        }
        else
        {
            if(collectNum < maxCollectNum)
            {
                var stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();

                if(stageMng.blockList.Count > 0)
                {
                    var targetBlock = FurthestBlock(stageMng);
                    targetBlock.transform.parent = hand;
                    targetBlock.transform.localRotation = hand.localRotation;
                    if (collectNum == 0) targetBlock.transform.localPosition = Vector3.zero;
                    else if (collectNum % 2 == 0) targetBlock.transform.localPosition = new Vector3(
                         (collectNum / 2) * targetBlock.transform.localScale.x, 0, 0);
                    else
                        targetBlock.transform.localPosition = new Vector3(
                         -((collectNum + 1) / 2) * targetBlock.transform.localScale.x, 0, 0);
                    collectNum += 1;
                }
                
            }
            collectBlockCount = 0;
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
}
