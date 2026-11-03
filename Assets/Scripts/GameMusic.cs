using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    // 定义音乐源的变量
    public AudioSource audioSource;

    // 定义一个音乐组列表
    public AudioClip[] sfx;

    // 创建一个播放音乐的函数
    public void PlayMusic(int index)
    {
        // 播放音乐，只播放一次
        audioSource.PlayOneShot(sfx[index]);
    }


}
