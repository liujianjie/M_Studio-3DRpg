using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 人物的属性。有普通属性和攻击属性
public class CharacterStats : MonoBehaviour
{
    public AudioSource hurtAudio;
    public AudioSource deathAudio;

    public event Action<int, int> UpdateHealthBarOnAttack;
    public CharacterData_SO characterTemplateData;
    public CharacterData_SO characterData;
    public Attack_SO attackData;
    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        if(characterTemplateData != null)
        {
            characterData = Instantiate(characterTemplateData);
        }
    }
    #region Character state
    public int MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else { return 100; } }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else { return 100; } }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else { return 30; } }
        set { characterData.baseDefence = value; }
    }
    public int CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else { return 30; } }
        set { characterData.currentDefence = value; }
    }
    #endregion

    #region Character Combat
    // 受伤
    public void TakeDamage(CharacterStats attacker, CharacterStats defener, bool isPlayerHurtAudio = false)
    {
        // TODO:根据距离判定是否攻击生效

        int damage = Mathf.Max(attacker.CurrentDamage(attacker) - defener.CurrentDefence, 1);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        // 根据攻击者暴击播放防御者的受伤动画
        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
            // 暴击，不管防御者是谁都要播放音效
            if (defener.hurtAudio != null)
            {
                defener.hurtAudio.Stop();
                defener.hurtAudio.Play();
            }
        }
        else
        {
            // 普通受伤击中音效，每个enemy都有，当敌人是石头人的时候，主角会发出
            if (defener.tag != Tags.Player || isPlayerHurtAudio)
            {
                if (defener.hurtAudio != null)
                {
                    defener.hurtAudio.Stop();
                    defener.hurtAudio.Play();
                }
            }
        }
        //TODO:ui
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //TODO:经验 
        // 本身生命为0，攻击者增加经验
        if (CurrentHealth <= 0)
        {
            // 只有攻击者是玩家才增加经验和播放升级音效
            if (attacker.tag == Tags.Player)
            {
                attacker.characterData.UpdateExp(characterData.killPoint);
            }
            if (deathAudio != null)
            {
                deathAudio.Stop();
                deathAudio.Play();
            }
        }
    }
    // 受伤 - 石头碰撞的时候
    public void TakeDamage(int damage, CharacterStats defener)
    {
        // 石头碰撞到玩家或者石头人时的音效
        if (defener.hurtAudio != null)
        {
            defener.hurtAudio.Stop();
            defener.hurtAudio.Play();
        }

        int currentDamage = Mathf.Max(damage - defener.CurrentDefence, 1);
        //Debug.Log("CurrentHealth"+CurrentHealth);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        //Debug.Log("CurrentHealth" + CurrentHealth);
        //TODO:ui
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        // 本身生命为0，玩家增加经验
        if (CurrentHealth <= 0)
        {
            // 只有攻击者是玩家才增加经验 和播放升级音效
            if (this.tag == Tags.Player)
            {
                GameManager.Instance.playerStates.characterData.UpdateExp(characterData.killPoint);
            }
            if (deathAudio != null)
            {
                deathAudio.Stop();
                deathAudio.Play();
            }
        }
    }
    private int CurrentDamage(CharacterStats attacker)
    {
        float coredamage = UnityEngine.Random.Range(attacker.attackData.minDamage, attacker.attackData.maxDamage);
        if (isCritical)
        {
            coredamage *= attacker.attackData.criticalMultiplier;
            //Debug.Log("暴击");

        }
        return (int)coredamage;
    }
    #endregion
}
