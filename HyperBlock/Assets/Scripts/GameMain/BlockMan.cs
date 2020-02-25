using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMan:MonoBehaviour
{
    [SerializeField] private string _name;
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
    public CharacterBase characterBase;
    public Transform lookat;

    [SerializeField] private GameObject chargeEffect;
    [SerializeField] private GameObject collectEffect;

    private void Awake()
    {       

    }

    public void Init()
    {
        characterBase = GetComponent<CharacterBase>();
        characterBase.BlockManName = _name;
        characterBase.moveSpeed = _moveSpeed;
        characterBase.body = _body;
        characterBase.color = _color;
        characterBase.blockChangerDis = _blockChangerDis;
        characterBase.blockChangePeriod = _blockChangePeriod;
        characterBase.blockCollectPeriod = _blockCollectPeriod;
        characterBase.hand = _hand;
        characterBase.maxCollectNum = _maxCollectNum;
        characterBase.attackSpeed = _attackSpeed;
        characterBase.attackSaveTime = _attackSaveTime;
        characterBase.coolDownTime = _coolDownTime;
        characterBase.ResetMoveDirection();
    }

    public void Charge(bool onoff)
    {
        switch (onoff)
        {
            case true:
                chargeEffect.SetActive(true);
                break;
            case false:
                chargeEffect.SetActive(false);
                break;
        }
        
    }

    public void Collect(bool onoff)
    {
        switch (onoff)
        {
            case true:
                collectEffect.SetActive(true);
                break;
            case false:
                collectEffect.SetActive(false);
                break;
        }
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
