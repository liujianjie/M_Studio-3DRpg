using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : SingleTon<SaveManager>
{
    public string sceneName = "levelname";

    public string SceneName { get { return PlayerPrefs.GetString(sceneName, "Game"); } }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TransitionToMain();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SavePlayerData();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //Debug.Log(PlayerController.Instance);
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.characterStats.CurrentHealth = PlayerController.Instance.characterStats.MaxHealth;
            }
        }
    }
    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStates.characterData, GameManager.Instance.playerStates.characterData.name);
    }
    public void LoadPlayerData()
    {

        Load(GameManager.Instance.playerStates.characterData, GameManager.Instance.playerStates.characterData.name);
    }
    public void Save(Object data, string key)
    {
        //Debug.Log("Save");
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, jsonData);
        // 保存当前场景名
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    public void Load(Object data, string key)
    {
        //Debug.Log("Load");
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
