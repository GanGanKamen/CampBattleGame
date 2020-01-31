using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMng : MonoBehaviour
{
    public enum Mode
    {
        P2,
        COM2,
        COM4
    }
    [SerializeField] int weight;
    [SerializeField] int hight;
    [SerializeField] GameObject cubeObj;
    [HideInInspector] public List<Block> blockList;
    [HideInInspector] public List<GameObject> holes;
    [HideInInspector] public int savedCharacterSize;
     public List<CharacterBase> allCharacters;
    [HideInInspector] public Vector3[] startPoints = new Vector3[4];
    [SerializeField]private PlayerCamera[] playerCameras;

    [SerializeField] private NextSceneSingleton sceneSingleton;
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
        
    }

    private void SetStartPoint()
    {
        var x = weight / 2;
        var z = hight / 2;
        var y = 1;
        var delta = cubeObj.transform.localScale.x * 5;
        startPoints[0] = new Vector3(x - delta, y, z - delta);
        startPoints[1] = new Vector3(-x + delta, y, z - delta);
        startPoints[2] = new Vector3(-x + delta, y, -z + delta);
        startPoints[3] = new Vector3(x - delta, y, -z + delta);
    }

    private void CreateField()
    {
        var wNum = weight / cubeObj.transform.localScale.x;
        var hNum = hight / cubeObj.transform.localScale.x;
        for(float i = (-weight / 2); i < (weight/2); i++)
        {
            for(float j = (-hight/2); j < (hight/2); j++)
            {
                GameObject obj = Instantiate(cubeObj, new Vector3(i, 0, j), Quaternion.identity);
                blockList.Add(obj.GetComponent<Block>());
            }
        }
    }

    public void Init()
    {
        CreateField();
        SetStartPoint();
    }

    public void CharacterRegister(string p1Name,string p2Name)
    {
        GameObject chara1 = Instantiate(ResourcesMng.ResourcesLoad("BlockMans/" + p1Name),
            startPoints[0], Quaternion.identity);
        chara1.AddComponent<Player>();
        chara1.GetComponent<Player>().SetControllerNum(1);
        playerCameras[0].Init(chara1.GetComponent<Player>());
        chara1.GetComponent<BlockMan>().Init();
        GameObject chara2 = Instantiate(ResourcesMng.ResourcesLoad("BlockMans/" + p2Name),
            startPoints[2], Quaternion.identity);
        chara2.AddComponent<Player>();
        chara2.GetComponent<Player>().SetControllerNum(2);
        playerCameras[1].Init(chara2.GetComponent<Player>());
        chara2.GetComponent<BlockMan>().Init();
        foreach (GameObject charaObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            allCharacters.Add(charaObj.GetComponent<CharacterBase>());
            savedCharacterSize += 1;
        }
        playerCameras[0].CameraStartAction();
        playerCameras[1].CameraStartAction();
        Invoke("GameStart", 2f);
    }

    private void GameStart()
    {
        for(int i = 0; i < allCharacters.Count; i++)
        {
            allCharacters[i].ReadyToStart();
        }
    }

    public void CreateHole(Block target)
    {
        blockList.Remove(target);
        var holeObj = Instantiate<GameObject>(ResourcesMng.ResourcesLoad("Hole"), target.transform.position, Quaternion.identity);
        holes.Add(holeObj);
    }

    private void PosShuffle()
    {
        for (int i = 0; i < startPoints.Length; i++)
        {
            var tmp = startPoints[i];
            int randomIndex = Random.Range(i, startPoints.Length);
            startPoints[i] = startPoints[randomIndex];
            startPoints[randomIndex] = tmp;
        }
    }

    public void CharacterDeath(CharacterBase character)
    {
        savedCharacterSize -= 1;
        if (savedCharacterSize == 1)
        {
            for (int i = 0; i < allCharacters.Count; i++)
            {
                allCharacters[i].GameOver();
            }
            sceneSingleton.NextSceneResult(character.BlockManName);
            Fader.FadeInBlack(2f, "Result");
        }
    }
}
