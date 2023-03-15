using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MouseManager : SingleTon<MouseManager>
{
    //public static MouseManager Instance;
    RaycastHit hitInfo;
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;
    public Texture2D point, doorway, attack, target, arrow;
    protected virtual void Awake()
    {
        //if (Instance != null)
        //{
        //    Destroy(gameObject);
        //}
        //Instance = this;
        //Debug.Log("Awake, MouseManager");
        base.Awake();
        // 场景删除不要删除这个脚本
        DontDestroyOnLoad(this);
    }
    // Update is called once per frame
    void Update()
    {
        SetCursorTexture();
        MouseControll();
    }
    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            switch (hitInfo.collider.gameObject.tag)
            {
                case Tags.Ground:
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case Tags.Enemy:
                case Tags.AttackAble:
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case Tags.Portal:
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }
    void MouseControll()
    {
        if(Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag(Tags.Ground))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag(Tags.Enemy))
            {
                if (hitInfo.collider.gameObject != null)
                {
                    //Debug.Log("hitInfo.collider.gameObject is not null");
                    //if(OnEnemyClicked == null)
                    //{
                    //    Debug.Log("OnEn null");
                    //}
                    OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
                }
                
            }
            if (hitInfo.collider.gameObject.CompareTag(Tags.AttackAble))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag(Tags.Portal))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }

        }
    }
}   
