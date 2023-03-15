using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab; // 血条图片Prefab
    Transform UIbar;    // UIbar 血条图片Prefab的位置

    public Transform barPoint; // 怪物的血条pos
    public bool alwaysVisible; // 是否一直可见
    public float visibleTime; // 显示时间
    Image healthSlider; // 绿色最底层的图片
    Transform cam;      // 摄像机

    private float timeLeft; // 显示时间剩余时间

    CharacterStats currentStats;

    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();
        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }
    private void OnEnable()
    {
        cam = Camera.main.transform;
        // 所有的Canvas
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                // canvas.transform 代表prefab设置的位置
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.gameObject.SetActive(alwaysVisible);
            }
        }
    }
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            if (UIbar != null)
            {
                Destroy(UIbar.gameObject);
                return;
            }
        }
        if (UIbar == null)
        {
            return;
        }
        UIbar.gameObject.SetActive(true);
        timeLeft = visibleTime;

        // 设置血条百分百
        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
    
    private void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = barPoint.position;
            UIbar.forward = -cam.forward;

            if (timeLeft <= 0 && !alwaysVisible)
                UIbar.gameObject.SetActive(false);
            else
                timeLeft -= Time.deltaTime;
        }
    }
}
