using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDebug : MonoBehaviour
{
    [SerializeField] Transform transform1,transform2,transform3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var vec1 = (transform1.position - transform2.position).normalized;
        var vec2 = (transform3.position - transform2.position).normalized;
        var dot = Vector3.Dot(vec1, vec2);
        Debug.Log(dot);
    }
}
