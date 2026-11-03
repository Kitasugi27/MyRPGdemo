using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    // 定义攻击点位置，AttackTriggerBox出现的位置坐标。
    public Transform Attack1pos;
    // 定义攻击盒子的预制体变量
    public GameObject AttackTriggerBox;

    // 定义主角的攻击力
    private float ATK;

    // 定义获取HeroController的变量
    public EnemyController Enemy;

    // 定义攻击主角的函数
    void AttackHerroEvent()
    {

        // 获取HeroController中的ATK；
        ATK = Enemy.showATK();
        Debug.Log("敌人攻击力：" + ATK);

        // 添加攻击音效
        // GameObject.Find("游戏物体的名字").GetComponent<游戏物体身上的插件名>();
        GameMusic gameMusic = GameObject.Find("GameMusic").GetComponent<GameMusic>();
        gameMusic.PlayMusic(0);

        // 在攻击点位置创建一个攻击盒子
        GameObject BOX = Instantiate(AttackTriggerBox, Attack1pos.position, Attack1pos.rotation);
        // 定义攻击盒子的大小与攻击点位置的大小一致。
        BOX.transform.localScale = Attack1pos.localScale;

        // 将主角的攻击力传给AttackTriggerBox.cs中的OnTriggerEnter()
        BOX.GetComponent<AttackTriggerBox>().setAttackInit(false, ATK, Enemy.transform);

    }
}
