using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObj : MonoBehaviour
{
    public List<Block> buildBlocks;
    public int CollectNum { get { return collectNum; } }
    public CharacterBase Master { get { return master; } }
    private int collectNum = 0;
    private CharacterBase master = null;
    private BoxCollider boxCollider;
    public void blockRegister(Block block)
    {
        buildBlocks.Add(block);
        block.transform.parent = this.transform;
        block.transform.localRotation = this.transform.localRotation;
        if (collectNum == 0) block.transform.localPosition = Vector3.zero;
        else if (collectNum % 2 == 0) block.transform.localPosition = new Vector3(
             (collectNum / 2) * block.transform.localScale.x, 0, 0);
        else
            block.transform.localPosition = new Vector3(
             -((collectNum + 1) / 2) * block.transform.localScale.x, 0, 0);
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
