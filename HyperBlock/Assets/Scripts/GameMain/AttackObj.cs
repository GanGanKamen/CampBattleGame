using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObj : MonoBehaviour
{
    public List<Block> buildBlocks;
    public int CollectNum { get { return collectNum; } }
    public CharacterBase Master { get { return master; } }
    public bool IsAttack { get { return attack; } }
    public Vector3 Direction { get { return direction; } }

    private int collectNum = 0;
    private CharacterBase master = null;
    private BoxCollider boxCollider;
    private bool attack = false;
    private Vector3 direction = Vector3.zero;
    private float speed = 0;
    private float saveTime = 0;
    private float processTime = 0;

    public void BlockRegister(Block block)
    {
        var stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
        //stageMng.blockList.Remove(block);
        buildBlocks.Add(block);
        stageMng.CreateHole(block);
        block.transform.parent = this.transform;
        block.transform.localRotation = this.transform.localRotation;
        Destroy(block.GetComponent<BoxCollider>());
        if (collectNum == 0) block.transform.localPosition = Vector3.zero;
        else if (collectNum % 2 == 0) block.transform.localPosition = new Vector3(
             (collectNum / 2) * block.transform.localScale.x, 0, 0);
        else
            block.transform.localPosition = new Vector3(
             -((collectNum + 1) / 2) * block.transform.localScale.x, 0, 0);
        ColliderReset(collectNum, block.transform.localScale.x);
        collectNum += 1;
        
    }

    public void Init(CharacterBase character)
    {
        collectNum = 0;
        master = character;
        buildBlocks = new List<Block>();
        gameObject.AddComponent<BoxCollider>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = Vector3.zero;
        boxCollider.isTrigger = true;
        transform.parent = character.hand;
        transform.localPosition = Vector3.zero;
        transform.localRotation = character.hand.localRotation;
    }

    public void Attack(Vector3 _direction,float _speed,float _saveTime)
    {
        if (IsAttack) return;
        attack = true;
        transform.parent = null;
        gameObject.tag = "Attack";
        direction = _direction;
        speed = _speed;
        saveTime = _saveTime;
        var stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
        stageMng.AttackObjRegister(this);
    }

    private void ColliderReset(int num,float scale)
    {
        if(num == 0)
        {
            boxCollider.size = new Vector3(scale, scale, scale);
            boxCollider.center = Vector3.zero;
        }
        else if (num % 2 == 0)
        {
            boxCollider.size = new Vector3(scale * (num+1), scale, scale);
            boxCollider.center = Vector3.zero;
        }
        else
        {
            boxCollider.size = new Vector3(scale * (num + 1), scale, scale);
            boxCollider.center = new Vector3(-scale / 2, 0, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAttack)
        {
            if(processTime <= saveTime)
            {
                processTime += Time.deltaTime;
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                var stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
                stageMng.AttackObjDelate(this);
            }
        }
    }
}
