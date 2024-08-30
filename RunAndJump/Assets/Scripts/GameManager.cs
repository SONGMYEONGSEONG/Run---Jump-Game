using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if(instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(GameManager).Name;
                    instance = obj.AddComponent<GameManager>();
                }

                //DontDestroyOnLoad(instance);
            }

            return instance;
        
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
           Destroy(gameObject);
        }
    }

    [SerializeField] GameObject block;
    //DontDestroyOnLoad() 실행 시, Inspector에서 참조한경우 자동으로 참조하지 못하기에 다 터져버린다.
    //코드 스크립트에서 참조 해줄수 있게 해줘야함 
    [SerializeField] Text scoreTxt;
    [SerializeField] Text levelTxt;
    [SerializeField] GameObject endPanel;
    [SerializeField] BlockSpawner blockSpawner;
    //
    [Header("EndPanel")]
    [SerializeField] Text bestscoreTxt;
    [SerializeField] Text bestlevelTxt;
    [SerializeField] Text curscoreTxt;
    [SerializeField] Text curlevelTxt;
    //
    int totalscore = 0;
    [SerializeField] int curLevel = 1;
    public int CurLevel
    {
        get { return curLevel; }
    }

    public void Initialize()
    {
        Time.timeScale = 1.0f;
        totalscore = 0;
        curLevel = 1;

    }

    private void Update()
    {
        StageLevelUp();
    }


    public void AddScore(int score)
    {
        totalscore += score;
        scoreTxt.text = totalscore.ToString();
    }

    public void StageLevelUp()
    {
        //curLevel = (totalscore / 2000) + 1;
        levelTxt.text = curLevel.ToString();
    }

    private void OnEndPanel()
    { 
        Transform EndObj = endPanel.transform.GetChild(0);
        EndObj.gameObject.SetActive(true);

        EndObj.GetComponent<UI_SpringAnim>().StartAnim();
    }

    private void DisEndPanel()
    {
        Transform EndObj = endPanel.transform.GetChild(0);
        EndObj.gameObject.SetActive(false);
    }

    public void PlayerDie()
    {
        
        OnEndPanel();

        if (PlayerPrefs.HasKey("bestscore"))
        {
            if(PlayerPrefs.GetInt("bestscore") < totalscore)
            {
                PlayerPrefs.SetInt("bestscore", totalscore);
                PlayerPrefs.SetInt("bestlv", curLevel); 
            }
        }
        else
        {
            PlayerPrefs.SetInt("bestscore", totalscore);
            PlayerPrefs.SetInt("bestlv", curLevel);
        }

        bestscoreTxt.text = PlayerPrefs.GetInt("bestscore").ToString();
        bestlevelTxt.text = PlayerPrefs.GetInt("bestlv").ToString();
        curscoreTxt.text = totalscore.ToString();
        curlevelTxt.text = curLevel.ToString(); 

    }

    private void TextFindObject(ref Text txt,string objname)
    {
        if (txt == null)
        {
            GameObject Obj = GameObject.Find(objname);
            if (Obj != null) txt = Obj.GetComponent<Text>();
        }
    }

    private void SceneLoad()
    {
        // 씬 전환 시 매번 호출하여 최신 오브젝트 참조를 설정합니다.
        TextFindObject(ref scoreTxt, "UI_Score");
        TextFindObject(ref levelTxt, "UI_Level");
        TextFindObject(ref bestscoreTxt, "BestScore");
        TextFindObject(ref bestlevelTxt, "BestLv");
        TextFindObject(ref curscoreTxt, "CurScore");
        TextFindObject(ref curlevelTxt, "CurLv");

        Initialize();

        scoreTxt.text = totalscore.ToString();
        levelTxt.text = curLevel.ToString();

        if (blockSpawner == null)
        {
            GameObject blockSpawnerObj = GameObject.Find("BlockSpanwer");
            if (blockSpawnerObj != null)
            {
                blockSpawner = blockSpawnerObj.GetComponent<BlockSpawner>();
            }
        }

        if(endPanel ==  null)
        {
            GameObject endPanelObj = GameObject.Find("EndPopUp");
            if (endPanelObj != null)
            {
                endPanel = endPanelObj;
            }
            else
            {
                Debug.Log("EndPanel Object Missing!!");
            }
        }

        DisEndPanel();
        blockSpawner.BlockSpawnerON();
        
    }
    private void OnEnable()
    {

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // 씬이 로드된 후 Scene 로드
       SceneLoad();
    }
}
