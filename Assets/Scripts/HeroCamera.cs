using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCamera : MonoBehaviour
{
    // 定义一个跟随的目标变量，与英雄进行绑定
    public Transform hero;
    // 定义一个偏移量
    public Vector3 offset;

    void Update()
    {
        offset = new Vector3(0f,3.5f,5.18f);

        // 设置退出按键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 如果是编辑模式，退出编辑器。
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                // 如果是运行模式，退出游戏。 
                Debug.Log("退出游戏");
                Application.Quit();
            #endif
        }
    }

    void LateUpdate()
    {
        if(hero != null)
        {
            transform.position = hero.position + offset;
        }
        
    }
}
