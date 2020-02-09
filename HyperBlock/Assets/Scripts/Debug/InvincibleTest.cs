using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleTest : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] bool invincible;
    [SerializeField] float invincibleCount = 0;
    [SerializeField] float invicibleTime = 2;

    private void InvincibleState()
    {
        if (invincible)
        {
            if (invincibleCount <= invicibleTime)
            {
                invincibleCount += Time.deltaTime;
                if(invincibleCount % 0.2 >= 0.1)
                {
                    body.transform.localScale = Vector3.one;
                }
                else
                {
                    body.transform.localScale = Vector3.zero;
                }
            }
            else
            {
                invincibleCount = 0;
                invincible = false;
            }
        }
        else
        {
            if (body.transform.localScale == Vector3.zero) body.transform.localScale = Vector3.one;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InvincibleState();
    }
}
