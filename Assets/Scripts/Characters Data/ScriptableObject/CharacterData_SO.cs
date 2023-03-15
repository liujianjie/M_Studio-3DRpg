using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Stats/Data", fileName = "New Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;
    public int killPoint;

    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;

    public float LevelMultiplier
    {
        get { return 1 + (currentLevel - 1) * levelBuff; }    
    }
    public void UpdateExp(int point)
    {
        currentExp += point;
        if(currentExp >= baseExp)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        // 播放升级音效
        if (!GameManager.Instance.isPlayerDeath)
        {
            GameManager.Instance.PlayLevelUpSound();
        }

        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        // 要求经验值变化
        baseExp += (int)(baseExp * LevelMultiplier);

        // 补血
        maxHealth = (int)(maxHealth * LevelMultiplier);
        currentHealth = maxHealth;

        Debug.Log("Level up!" + currentLevel + "Max health:" + maxHealth);
    }
}
