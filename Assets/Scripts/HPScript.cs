using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 引入命名空间
using UnityEngine.UI;
public class HPScript : MonoBehaviour
{
    // 定义一个slider类型变量
    public Slider hpSlider;
    void Update()
    {
        // 让画布与血条一直对着摄像机
        transform.LookAt(Camera.main.transform);
    }

    public void SetHP(float hpHow,float hpMax)
    {
        // 设置血条的值
        hpSlider.value = hpHow / hpMax;
    }

    // 创建一个退出游戏函数
    public void ExitGame()
    {
        // 编辑器专用
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 玩家专用
        Application.Quit();
        #endif
    }

    // 创建一个进入"RPG游戏项目"场景的函数
    public void EnterRPGGame()
    {
        // 跳转到RPG游戏项目场景
        Application.LoadLevel("RPG游戏项目");
    }
}
