using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIAndLogic : SingleTon<GameUIAndLogic>
{
    public GameObject lv1TipCanvas;
    public Text lv1Tiptxt;

    public GameObject lv1Potral;

    public int level = 0;
    public string tiptxtstr;
    private bool isShowTip = false;
    protected virtual void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        Lv1ShowTip(tiptxtstr);
    }
    // 检测是否没有敌人了
    void Update()
    {
        if (level != 2 || isShowTip)
        {
            return;
        }
        if (FindObjectsOfType<EnemyController>().Length == 0 && !GameManager.Instance.isPlayerDeath)
        {
            isShowTip = true;
            // 没有敌人了

            // 播放胜利音效
            GameManager.Instance.PlayVictorySound();

            // 提示信息
            Lv2ShowTip();
            Lv1ShowPotral();
        }
    }
    public void Lv1ShowTip(string str)
    {
        if (lv1TipCanvas == null) return;
        lv1TipCanvas.SetActive(true);
        lv1Tiptxt.text = str;
    }
    // 关卡1提示，并出现传送门
    public void Lv1ShowPotral()
    {
        if (lv1Potral == null) return;
        lv1Potral.SetActive(true);
    }
    public void Lv1ShowTip()
    {
        Lv1ShowTip("BOSS清理完成，传送门已开启！");
    }
    public void Lv2ShowTip()
    {
        Lv1ShowTip("任务完成,恭喜通关！\n按ESC或用金色传送门回到主界面");
    }
}
