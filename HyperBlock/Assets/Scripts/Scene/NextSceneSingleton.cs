using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneSingleton : MonoBehaviour
{
    private string startSceneName;
    private string[] blockmanNames;

    private string winnerName;
    // Start is called before the first frame update
    private void Awake()
    {
        startSceneName = SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SceneChange();
    }

    public void NextScene2P(string p1,string p2)
    {
        blockmanNames = new string[2];
        blockmanNames[0] = p1;
        blockmanNames[1] = p2;
    }

    public void NextSceneResult(string name)
    {
        winnerName = name;
    }

    private void SceneChange()
    {
        if(SceneManager.GetActiveScene().name != startSceneName)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "2P":
                    var stageMng = GameObject.Find("StageMng").GetComponent<StageMng>();
                    stageMng.Init();
                    stageMng.CharacterRegister(blockmanNames[0], blockmanNames[1]);
                    break;
                case "Result":
                    var result = GameObject.Find("ResultCanvas").GetComponent<Result>();
                    result.SetWinner(winnerName);
                    break;
                        
            }

            Destroy(gameObject);
        }
    }
}
