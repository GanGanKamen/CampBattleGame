using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDebug : MonoBehaviour
{
    [SerializeField] StageMng stageMng;
    [SerializeField] CharacterAI characterAI;

    [SerializeField] List<GameObject> holes;
    [SerializeField] Vector3 rootDebug;

    [SerializeField] float dis;
    [SerializeField] float dot;
    // Start is called before the first frame update


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        holes = stageMng.holes;
        dis = Vector3.Distance(transform.position, characterAI.transform.position);
        dot = Vector3.Dot(rootDebug, (transform.position - characterAI.transform.position).normalized);
        Debug.DrawRay(transform.position + Vector3.up, rootDebug, Color.blue, 1f, false);
    }
}
