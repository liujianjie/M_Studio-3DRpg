using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

// 场景切换
public class SceneController : SingleTon<SceneController>, IEndGameObserver
{
    bool fadeOk; // 是否播放过
    public GameObject playerPrefab;
    GameObject player;
    NavMeshAgent agent;
    // 场景切换的类
    public SceneFader sceneFaderPrefab;
    protected override void Awake()
    {
        base.Awake();
        // 场景删除不要删除这个脚本
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        GameManager.Instance.AddEndGameObserver(this);
        fadeOk = false;
        //StartCoroutine(TestEnum());
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.EndGame:
                // 结束游戏，回到主界面
                TransitionToMain();
                break;
        }
    }
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destination)
    {
        // TODO:保存玩家数据
        SaveManager.Instance.SavePlayerData();
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneFader fade = Instantiate(sceneFaderPrefab);
            yield return StartCoroutine(fade.FadeOut(2f));
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destination).transform.position, GetDestination(destination).transform.rotation);
            // 加载玩家数据
            SaveManager.Instance.LoadPlayerData();
            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;
        }
        else
        {
            // 同场景
            player = GameManager.Instance.playerStates.gameObject;
            agent = player.GetComponent<NavMeshAgent>();
            agent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destination).transform.position, GetDestination(destination).transform.rotation);
            agent.enabled = true;
            yield return null;
        }
    }
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        for(int i = 0; i < entrances.Length; i++)
        {
            if(entrances[i].destinationTag == destinationTag)
            {
                return entrances[i];
            }
        }
        return null;
    }

    // 加载场景
    public void TransitionToFirstLevel()// 新游戏
    {
        StartCoroutine(LoadLevel("Level1"));
        //StartCoroutine(LoadLevel("Game"));
    }
    public void TransitionToLoadGame()// 继续游戏
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));// 根据保存的关卡名，加载这个关卡，人物数据保存由人物类自己在start获取
    }
    public void TransitionToMain()// 主菜单
    {
        StartCoroutine(LoadMain());
    }
    IEnumerator LoadLevel(string scene)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        if (scene != "")
        {
            //Debug.Log("fade ing");
            yield return StartCoroutine(fade.FadeOut(2f));
            //Debug.Log("fade ok");
            // 加载场景
            //Debug.Log("LoadSceneAsync qian");
            yield return SceneManager.LoadSceneAsync(scene);
            //Debug.Log("LoadSceneAsync hou");
            // 实例化主角
            //Debug.Log("Instantiate playerPrefab qian");
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);
            //Debug.Log("Instantiate playerPrefab hou");

            // 再次保存数据
            // 新游戏时删除了数据，这里初始化了数据
            SaveManager.Instance.SavePlayerData();
            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;
        }
    }
    // 返回主界面
    IEnumerator LoadMain()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        // 加载场景
        yield return StartCoroutine(fade.FadeOut(2f));
        yield return SceneManager.LoadSceneAsync("Mains");
        yield return StartCoroutine(fade.FadeIn(2f));
        yield break;
    }

    public void EndNotify()
    {
        if (!fadeOk)
        {
            fadeOk = true;
            StartCoroutine(LoadMain());
        }
    }
    float timers = 0.0f;
    IEnumerator TestEnum()
    {
        while (timers <= 6)
        {
            timers += Time.deltaTime;
            Debug.Log(timers);
            yield return new WaitForSeconds(2);
            Debug.Log("TestEnum");
        }
        //yield break;
    }
}
