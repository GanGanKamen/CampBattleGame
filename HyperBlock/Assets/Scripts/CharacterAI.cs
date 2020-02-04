using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : CharacterBase
{
    [SerializeField] private float searchDis;
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
    [SerializeField] Vector3 debugPos;
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
        StartSearch();
    }

    private void AIStateUpdate()
    {
        StatusUpdate();
        switch (ai)
        {
            case AIStatus.Search:
                break;
        }
    }

    private void StartSearch()
    {
        StartCoroutine(SearchOnePoint());
    }

    private IEnumerator SearchOnePoint()
    {
        var pos = transform.position + MoveDirection * searchDis + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        if(Mathf.Abs(pos.x) >= (stageMng.Weight/2 - 1) || Mathf.Abs(pos.z) >= (stageMng.Hight/2 - 1))
        {
            pos = transform.position - MoveDirection * searchDis;
        }
        var progressRoot = (pos - transform.position).normalized;

        var hit = IsHitHole(progressRoot);
        //var hit = false;
        var angle = 0;
        while (hit)
        {
            Debug.Log("hit");
            angle += 10;
            pos += new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            Debug.Log(pos);
            var nextRoot = (pos - transform.position).normalized;
            hit = IsHitHole(nextRoot);
            if (angle > 360) break;
        }
        debugPos = pos;
        while(Vector3.Distance(transform.position, pos) > 0.1f)
        {
            base.CharacterMove((pos - transform.position).normalized);
            yield return null;
        }
        StartSearch();
        yield break;
    }

    private bool IsHitHole(Vector3 direction)
    {
        var ostacleList = new List<GameObject>();
        for (int i = 0; i < stageMng.holes.Count; i++)
        {
            var perpendicularFootPoint = transform.position +
                Vector3.Project(stageMng.holes[i].transform.position - transform.position, direction);
            var perpendicular = Vector3.Distance(perpendicularFootPoint, stageMng.holes[i].transform.position);
            var dot = Vector3.Dot(direction, (stageMng.holes[i].transform.position - transform.position).normalized);
            var rad = Mathf.Acos(dot);
            var dis = Vector3.Distance(stageMng.holes[i].transform.position, transform.position);
            if (dot >= 0.8f && dis <= searchDis)
            {
                ostacleList.Add(stageMng.holes[i]);
            }
        }
        if (ostacleList.Count > 0) return true;
        /*ostacleList.Sort((a, b) => (int)Vector3.Distance(a.transform.position, transform.position) -
    (int)Vector3.Distance(b.transform.position, transform.position));
        if (ostacleList[0] != null) return true;
        */
        else return false;
    }

    private void GoToNextPoint(Vector3 pos)
    {
        var direction3d = (pos - transform.position).normalized;
        base.CharacterMove(direction3d);
    }
}
