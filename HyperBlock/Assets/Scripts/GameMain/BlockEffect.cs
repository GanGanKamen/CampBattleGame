using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEffect : MonoBehaviour
{
    [SerializeField] float delta;
    [SerializeField] float saveTime;
    public CharacterBase goal;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, saveTime);
    }

    public void Init(CharacterBase _goal,Color _color)
    {
        GetComponent<Renderer>().material.color = _color;
        goal = _goal;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, goal.transform.position + Vector3.up, Time.deltaTime * delta);
    }
}
