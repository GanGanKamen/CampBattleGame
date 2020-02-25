using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : CharacterBase
{
    [SerializeField] private float searchDis;
    [SerializeField] private float enemySearchDis;
    [SerializeField] private float attackRange;
    [SerializeField] private float runPeriod;
    [SerializeField] private float dangerSearchDis;

    [SerializeField] private CharacterBase targetEnemy;
    [SerializeField] private Vector3 nowMovePos;
    [SerializeField] private Vector3 nowRoot;
    [SerializeField] private AttackObj targetAttackObj;
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
    private void Awake()
    {

    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AIStateUpdate();
    }

    public void Init()
    {
        stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
        characterStatus = CharacterStatus;
        nowRoot = GetSearchRoot();
    }


    public void AttackVigilanceCancel()
    {
        if (ai == AIStatus.Avoidance)
        {
            if (targetAttackObj.Master.IsDeath == false)
            {
                var dis = Vector3.Distance(transform.position, targetAttackObj.Master.transform.position);
                if(dis <= enemySearchDis * 2)
                {
                    targetEnemy = targetAttackObj.Master;
                    ai = AIStatus.Chase;
                }
                else
                {
                    ToSearch();
                }
            }
            else
            {
                ToSearch();
            }
            targetAttackObj = null;
            
        }
    }

    public void GetEnemyDeath(CharacterBase enemy)
    {
        if(targetEnemy != null && targetEnemy == enemy)
        {
            ToSearch();
        }
    }

    private void AIStateUpdate()
    {
        StatusUpdate();
        switch (ai)
        {
            case AIStatus.Search:
                Search();
                ToChase();
                ToAvoidance();
                break;
            case AIStatus.Chase:
                Chase();
                ToAvoidance();
                break;
            case AIStatus.Run:
                Run();
                ToAvoidance();
                break;
            case AIStatus.Avoidance:
                Avoidance();
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
            nowRoot = GetChaseEnemy(targetEnemy);
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
        if (dis <= 1f)
        {
            CharacterMove(GetSearchRoot());
        }
        else
        {
            CharacterMove(nowRoot);
        }

        if (MyBlockNum >= 100) CollectBlock(blockCollectPeriod);
        if(myAttack != null)
        {
            if(myAttack.CollectNum == maxCollectNum)
            {
                Attack();
            }
        }
    }

    private Vector3 GetSearchRoot()
    {
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
            angle += 10;
            pos += new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
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
                if (dis <= 1f) CharacterMove(GetRunRoot());
                else CharacterMove(nowRoot);
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

    private void ToAvoidance()
    {
         for(int i = 0; i < stageMng.attacks.Count; i++)
        {
            var attackPredictPos = (stageMng.attacks[i].Direction * dangerSearchDis)
                + stageMng.attacks[i].transform.position;
            var attackPredictDis = Vector3.Distance(transform.position, attackPredictPos);
            if(attackPredictDis <= 1 && stageMng.attacks[i].Master != this)
            {
                targetAttackObj = stageMng.attacks[i];
                break;
            }
        }
    }

    private void Avoidance()
    {
        var dis = Vector3.Distance(transform.position, nowMovePos);
        if (dis <= 1f)
        {
            if (targetAttackObj != null)
            {
                CharacterMove(GetAvoidanceRoot());
            }
        }
        else CharacterMove(nowRoot);
    }

    private Vector3 GetAvoidanceRoot()
    {
        var targetAttackPos = targetAttackObj.transform.position;
        var nowPos = transform.position;
        var root = targetAttackObj.Direction;
        var pos = Vector3.Scale(root, new Vector3(searchDis, 0, searchDis)) + nowPos;
        if (Mathf.Abs(pos.x) >= (stageMng.Weight / 2 - 1) || Mathf.Abs(pos.z) >= (stageMng.Hight / 2 - 1))
        {
            var left = Vector3.Scale(pos, new Vector3(-1, 0, 1));
            var right = Vector3.Scale(pos, new Vector3(1, 0, -1));
            if (Mathf.Abs(left.x) < (stageMng.Weight / 2 - 1) && Mathf.Abs(left.z) < (stageMng.Weight / 2 - 1)) pos = left;
            else if (Mathf.Abs(right.x) < (stageMng.Weight / 2 - 1) && Mathf.Abs(right.z) < (stageMng.Weight / 2 - 1)) pos = right;
            else
            {
                pos = nowPos;
                nowMovePos = pos;
                nowRoot = Vector3.zero;
                return Vector3.zero;
            } 
        }
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
