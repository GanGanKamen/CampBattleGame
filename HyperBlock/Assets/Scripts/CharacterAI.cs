﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : CharacterBase
{
    [SerializeField] private float searchDis;
    [SerializeField] private float enemySearchDis;
    [SerializeField] private float attackRange;
    [SerializeField] private float runPeriod;

    [SerializeField] private CharacterBase targetEnemy;
    [SerializeField] private Vector3 nowMovePos;
    [SerializeField] private Vector3 nowRoot;
    public enum AIStatus
    {
        Search,
        Chase,
        Avoidance,
        Run
    }

    [SerializeField]private AIStatus ai;
    private StageMng stageMng;
    private float runCount = 0;
    private Status characterStatus;
    // Start is called before the first frame update

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        AIStateUpdate();

    }

    public void Init()
    {
        ai = AIStatus.Search;
        stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
        nowRoot = GetSearchRoot();
        characterStatus = CharacterStatus;
    }

    private void AIStateUpdate()
    {
        StatusUpdate();
        switch (ai)
        {
            case AIStatus.Search:
                Search();
                ToChase();
                break;
            case AIStatus.Chase:
                Chase();
                if (targetEnemy == null) ToSearch();
                break;
            case AIStatus.Run:
                Run();
                break;
        }
    }

    private void ToSearch()
    {
        ai = AIStatus.Search;
        nowRoot = GetSearchRoot();
    }

    private void ToChase()
    {
        if (MyBlockNum <= 10) return;
        var enemyList = new List<CharacterBase>();
        for (int i = 0; i < stageMng.allCharacters.Count; i++)
        {
            if (stageMng.allCharacters[i] != this)
            {
                var dis = Vector3.Distance(stageMng.allCharacters[i].transform.position, transform.position);
                if (dis <= enemySearchDis) enemyList.Add(stageMng.allCharacters[i]);
            }
        }
        if (enemyList.Count > 0)
        {
            enemyList.Sort((a, b) => (int)Vector3.Distance(a.transform.position, transform.position) -
            (int)Vector3.Distance(b.transform.position, transform.position));
            targetEnemy = enemyList[0];
            ai = AIStatus.Chase;
        }
        else
        {
            if(ai != AIStatus.Search) ToSearch();
        }
    }

    private void ToRun()
    {
        ai = AIStatus.Run;
        nowRoot = GetRunRoot();
    }

    private void Chase()
    {
        var dis = Vector3.Distance(transform.position, nowMovePos);
        if (dis <= 3f) CharacterMove(GetChaseEnemy(targetEnemy));
        else CharacterMove(nowRoot);
        Attack();
    }

    private void Attack()
    {
        CollectBlock(blockCollectPeriod);
        if(myAttack != null && myAttack.CollectNum >0 && CharacterStatus != Status.Attack)
        {
            var dot = Vector3.Dot(MoveDirection,
                (targetEnemy.transform.position - transform.position).normalized);
            var dis = Vector3.Distance(targetEnemy.transform.position, transform.position);
            if(dis <= attackRange && dot >= 0.8)
            {
                BlockAttack();
                ToRun();
            }
        }
    }

    private Vector3 GetChaseEnemy(CharacterBase target)
    {
        var nowPos = transform.position;
        var root = Vector3.Scale((target.transform.position - nowPos),
            new Vector3(1, 0, 1)).normalized;
        var pos = Vector3.Scale(root, new Vector3(searchDis, 0, searchDis)) + nowPos;
        var angle = 0;
        var hit = IsHitHole(root, nowPos);
        while (hit)
        {
            Debug.Log("hit");
            angle += 10;
            pos += new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            Debug.Log(pos);
            root = (pos - nowPos).normalized;
            hit = IsHitHole(root, nowPos);
            if (angle > 360) break;
        }
        nowMovePos = pos;
        nowRoot = root;
        return root;
    }

    private void Search()
    {
        var dis = Vector3.Distance(transform.position, nowMovePos);
        //Debug.Log("dis =" + dis);
        if (dis <= 0.1f)
        {
            CharacterMove(GetSearchRoot());
        }
        else
        {
            CharacterMove(nowRoot);
        }
    }

    private Vector3 GetSearchRoot()
    {
        Debug.Log("GetSearchRoot");
        var randomAngle = Random.Range(0, 180);
        var pos = transform.position + MoveDirection * searchDis + new Vector3(Mathf.Sin(randomAngle * Mathf.Deg2Rad), 0, Mathf.Cos(randomAngle * Mathf.Deg2Rad));
        if (Mathf.Abs(pos.x) >= (stageMng.Weight / 2 - 1) || Mathf.Abs(pos.z) >= (stageMng.Hight / 2 - 1))
        {
            pos = transform.position - MoveDirection * searchDis;
        }
        var progressRoot = (pos - transform.position).normalized;
        var nowPos = transform.position;
        var angle = 0;
        var hit = IsHitHole(progressRoot, nowPos);
        while (hit)
        {
            Debug.Log("hit");
            angle += 10;
            pos += new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            Debug.Log(pos);
            progressRoot = (pos - nowPos).normalized;
            hit = IsHitHole(progressRoot, nowPos);
            if (angle > 360) break;
        }
        nowMovePos = pos;
        nowRoot = progressRoot;
        return progressRoot;
    }

    private void Run()
    {
        if(targetEnemy != null)
        {
            if (runCount < runPeriod)
            {
                if(CharacterStatus != Status.Attack) runCount += Time.deltaTime;
                var dis = Vector3.Distance(transform.position, nowMovePos);
                if (dis <= 0.1f) CharacterMove(GetRunRoot());
            }
            else
            {
                runCount = 0;
                if (MyBlockNum > 10) ToChase();
            }
        }
        else
        {
            runCount = 0;
            ToSearch();
        }
    }

    private Vector3 GetRunRoot()
    {
        var enemyPos = targetEnemy.transform.position;
        var root = Vector3.Scale((transform.position - enemyPos),
            new Vector3(1, 0, 1)).normalized;
        var pos = Vector3.Scale(root, new Vector3(searchDis, 0, searchDis)) + enemyPos;
        if (Mathf.Abs(pos.x) >= (stageMng.Weight / 2 - 1) || Mathf.Abs(pos.z) >= (stageMng.Hight / 2 - 1))
        {
            pos = transform.position - MoveDirection * searchDis;
        }
        var angle = 0;
        var hit = IsHitHole(root, enemyPos);
        while (hit)
        {
            Debug.Log("hit");
            angle += 10;
            pos += new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            Debug.Log(pos);
            root = (pos - enemyPos).normalized;
            hit = IsHitHole(root, enemyPos);
            if (angle > 360) break;
        }
        nowMovePos = pos;
        nowRoot = root;
        return root;
    }

    private bool IsHitHole(Vector3 direction,Vector3 position)
    {
        var ostacleList = new List<GameObject>();
        for (int i = 0; i < stageMng.holes.Count; i++)
        {
            var perpendicularFootPoint = position +
                Vector3.Project(stageMng.holes[i].transform.position - position, direction);
            var perpendicular = Vector3.Distance(perpendicularFootPoint, stageMng.holes[i].transform.position);
            var dot = Vector3.Dot(direction.normalized, (stageMng.holes[i].transform.position - position).normalized);
            var rad = Mathf.Acos(dot);
            var dis = Vector3.Distance(stageMng.holes[i].transform.position, position);
            if (dot >= 0.8f && dis <= searchDis)
            {
                ostacleList.Add(stageMng.holes[i]);
            }
        }
        if (ostacleList.Count > 0) return true;
        else return false;
    }
}
