using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimator : MonoBehaviour
{
    // 定义攻击点位置，AttackTriggerBox出现的位置坐标。
    public Transform Attack1pos;
    // 定义攻击盒子的预制体变量
    public GameObject AttackTriggerBox;

    // 定义主角的攻击力
    private float ATK;

    // 定义获取HeroController的变量
    public HeroController hero;

    // 定义攻击敌人的函数
    void AttackEnemyEvent()
    {

        // 获取HeroController中的ATK；
        ATK = hero.showATK();
        Debug.Log("主角攻击力："+ATK);

        // 添加攻击音效
        // GameObject.Find("游戏物体的名字").GetComponent<游戏物体身上的插件名>();
        GameMusic gameMusic = GameObject.Find("GameMusic").GetComponent<GameMusic>();
        gameMusic.PlayMusic(0);

        // 在攻击点位置创建一个攻击盒子
        GameObject BOX = Instantiate(AttackTriggerBox,Attack1pos.position,Attack1pos.rotation );
        // 定义攻击盒子的大小与攻击点位置的大小一致。
        BOX.transform.localScale = Attack1pos.localScale;

        // 将主角的攻击力传给AttackTriggerBox.cs中的OnTriggerEnter()
        BOX.GetComponent<AttackTriggerBox>().setAttackInit(true,ATK,hero.transform);
        
    }
}
