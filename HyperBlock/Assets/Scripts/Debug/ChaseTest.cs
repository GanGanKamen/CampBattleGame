using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTest : MonoBehaviour
{
    [SerializeField] Transform targetEnemy;
    [SerializeField] Vector3 movePos;
    // Start is called before the first frame update
    void Start()
    {
        movePos = new Vector3(-10, 0, -10);
        //StartAction();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(movePos,transform.position) > 0.1f)
        {
            CharacterMove((movePos - transform.position).normalized);
        }
        else
        {
            SetMovePos();
        }
    }


    private void SetMovePos()
    {
        Debug.Log("set");
        var root = (Vector3.Scale(targetEnemy.position - transform.position, new Vector3(1, 0, 1))).normalized;
        movePos = Vector3.Scale(root, new Vector3(5, 0, 5));        
    }

    public void CharacterMove(Vector3 _direction)
    {
        var direction = new Vector3(_direction.x, 0, _direction.z).normalized;
        transform.Translate(direction * Time.deltaTime * 2f);
        //body.transform.localRotation = Quaternion.LookRotation(direction);
        //movedirection = direction;
    }
}
