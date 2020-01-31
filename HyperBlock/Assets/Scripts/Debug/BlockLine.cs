using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLine : MonoBehaviour
{
    [SerializeField]private LineRenderer lineRenderer;
    [SerializeField] private Transform s,g;

    // Start is called before the first frame update
    public void Init()
    {
        lineRenderer.positionCount = 100;

    }

    public void ConnectCharacter(Transform character,Transform thisBlock)
    {
        var start = thisBlock.transform.position;
        var goal = character.transform.position;
        var disXdelta = Mathf.Abs(goal.x - start.x)/100;
        var disZdelta = Mathf.Abs(goal.x - start.x)/100;
        for(int i = 0; i < 100; i++)
        {
            var pos = start + new Vector3(i * disXdelta, 0, i * disZdelta);
            lineRenderer.SetPosition(i, pos);
        }
    }

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        ConnectCharacter(g, s);
    }
}
