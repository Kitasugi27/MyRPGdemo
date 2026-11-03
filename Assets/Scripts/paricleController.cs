using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paricleController : MonoBehaviour
{
    // 定义一个粒子系统变量
    public ParticleSystem particleSystem;


    void Start()
    {
        // 获取身上的粒子系统组件
        particleSystem = GetComponent<ParticleSystem>();
        // 设置粒子系统默认不播放
        particleSystem.Stop();
    }
    void Update()
    {
        // 如果按下空格键，判断粒子系统是否播放，如果播放则停止，否则播放
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
            else
            {
                particleSystem.Play();
            }
        }
    }
}
