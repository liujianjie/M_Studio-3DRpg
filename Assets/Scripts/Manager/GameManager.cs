using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

// 游戏管理者，监听者服务，控制游戏中的实体
public class GameManager : SingleTon<GameManager>
{
    // 升级音效
    public AudioSource levelUpAudio;
    // 胜利音效
    public AudioSource victoryAudio;
    // 玩家是否死亡
    public bool isPlayerDeath = false;


    public CharacterStats playerStates;
    // 实体统一实现了IEndGameObserver接口
    public List<IEndGameObserver> endGameObserversList = new List<IEndGameObserver>();
    private CinemachineFreeLook followCamera;
    protected virtual void Awake()
    {
        base.Awake();
        // 场景删除不要删除这个脚本
        DontDestroyOnLoad(this);
    }
    // 注册方法
    public void RigisterPlayer(CharacterStats player)
    {
        playerStates = player;
        isPlayerDeath = false;

        // 跟踪相机
        //followCamera = FindObjectOfType<CinemachineFreeLook>();
        //if(followCamera != null)
        //{
        //    followCamera.Follow = playerStates.transform.GetChild(2);
        //    followCamera.LookAt = playerStates.transform.GetChild(2);
        //}
    }
    // 
    public void AddEndGameObserver(IEndGameObserver ob)
    {
        endGameObserversList.Add(ob);
    }
    public void RemoveEndGameObserver(IEndGameObserver ob)
    {
        endGameObserversList.Remove(ob);
    }
    // 广播
    public void NotifyObservers()
    {
        foreach (var observer in endGameObserversList)
        {
            observer.EndNotify();
        }
    }

    // 找传送门
    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENTER)
                return item.transform;
        }
        return null;
    }

    // 播放升级音效
    public void PlayLevelUpSound()
    {
        if (levelUpAudio != null)
        {
            levelUpAudio.Stop();
            levelUpAudio.Play();
        }
    }
    // 播放胜利音效
    public void PlayVictorySound()
    {
        if (victoryAudio != null)
        {
            victoryAudio.Stop();
            victoryAudio.Play();
        }
    }
}
