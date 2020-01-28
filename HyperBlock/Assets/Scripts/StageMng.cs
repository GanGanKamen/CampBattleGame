using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMng : MonoBehaviour
{
    [SerializeField] int weight;
    [SerializeField] int hight;
    [SerializeField] GameObject cubeObj;
    [HideInInspector] public List<Block> blockList;
    [HideInInspector] public int playerSize;
     public List<CharacterBase> allCharacters;
    // Start is called before the first frame update
    private void Awake()
    {
        foreach(GameObject charaObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            allCharacters.Add(charaObj.GetComponent<CharacterBase>());
        }
    }

    void Start()
    {
        CreateField();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateField()
    {
        var wNum = weight / cubeObj.transform.localScale.x;
        var hNum = hight / cubeObj.transform.localScale.x;
        for(float i = (-weight / 2); i < weight; i++)
        {
            for(float j = (-hight/2); j < hight; j++)
            {
                GameObject obj = Instantiate(cubeObj, new Vector3(i, 0, j), Quaternion.identity);
                blockList.Add(obj.GetComponent<Block>());
            }
        }
    }
}
