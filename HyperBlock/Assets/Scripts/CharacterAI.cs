using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : CharacterBase
{
    [SerializeField] private float searchDis;
    [SerializeField] private float enemySearchDis;
    [SerializeField] private CharacterBase targetEnemy;
    public enum AIStatus
    {
        Search,
        Chase,
        Avoidance,
        Run
    }

    [SerializeField]private AIStatus ai;
    private AIStatus pre;
    private StageMng stageMng;
    [SerializeField] private IEnumerator nowAction;

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
        pre = ai;
        stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
        nowAction = SearchOnePoint();
        StartAction(SearchOnePoint());
    }

    private void AIStateUpdate()
    {
        StatusUpdate();
        switch (ai)
        {
            case AIStatus.Search:
                ToChase();
                break;
            case AIStatus.Chase:
                ChaseEnemy();
                break;
        }
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
            StopCoroutine(nowAction);
        }
    }

    private void ChaseEnemy()
    {
        if(targetEnemy == null)
        {
            ai = AIStatus.Search;
            StartAction(SearchOnePoint());
            return;
        }
        Debug.Log("startChase");
        var enemyPos = targetEnemy.transform.position;
        var nowPos = transform.position;
        var root = (Vector3.Scale(enemyPos - nowPos,new Vector3(1,0,1))).normalized;
        var movePos = Vector3.Scale(root, new Vector3(searchDis, 0, searchDis));
        var angle = 0;
        var hit = IsHitHole(root, nowPos);
        while (hit)
        {
            angle += 10;
            movePos += new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            var nextRoot = (movePos - nowPos).normalized;
            hit = IsHitHole(nextRoot, nowPos);
            if (angle > 360) break;
        }
        base.CharacterMove((movePos - transform.position).normalized);
    }

    private void StartAction(IEnumerator action)
    {
        //if (nowAction != null) StopCoroutine(nowAction);
        nowAction = action;
        StartCoroutine(action);
    }

    private IEnumerator SearchOnePoint()
    {
        Debug.Log("StartSearch");
        var randomAngle = Random.Range(0, 180);
        var pos = transform.position + MoveDirection * searchDis + new Vector3(Mathf.Sin(randomAngle * Mathf.Deg2Rad), 0, Mathf.Cos(randomAngle * Mathf.Deg2Rad));
        if(Mathf.Abs(pos.x) >= (stageMng.Weight/2 - 1) || Mathf.Abs(pos.z) >= (stageMng.Hight/2 - 1))
        {
            pos = transform.position - MoveDirection * searchDis;
        }
        var progressRoot = (pos - transform.position).normalized;
        var nowPos = transform.position;
        var hit = IsHitHole(progressRoot,nowPos);
        //var hit = false;
        var angle = 0;
        while (hit)
        {
            Debug.Log("hit");
            angle += 10;
            pos += new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            Debug.Log(pos);
            var nextRoot = (pos -nowPos).normalized;
            hit = IsHitHole(nextRoot,nowPos);
            if (angle > 360) break;
        }

        while(Vector3.Distance(transform.position, pos) > 1)
        {
            base.CharacterMove((pos - transform.position).normalized);
            yield return null;
        }
        Debug.Log("oneSrarch");
        StartAction(SearchOnePoint());
        yield break;
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
