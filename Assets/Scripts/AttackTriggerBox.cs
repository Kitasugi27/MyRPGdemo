using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerBox : MonoBehaviour
{

    // 定义一个攻击者变量
    public Transform attackPlayer;
    // 定义一个是否为主角攻击的变量
    public bool IsPlayerAttack = true;

    // 定义伤害值
    public float damage;

    // 定义一个接收外部传值的默认函数
    public void setAttackInit(bool isPlayer,float dam,Transform attacker)
    {
        // 接收外部传入的相关参数：是否为主角和攻击力
        IsPlayerAttack = isPlayer;
        damage = dam;
        attackPlayer = attacker;
    }
    
    void Start()
    {
        // 预制体出现0.2秒后自动销毁
        Destroy(gameObject,0.2f);
    }

    void Update()
    {
        
    }

    // 设置验证Trigger碰撞的函数
    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayerAttack == true)
        {
            // 如果碰到的是敌人，则扣血
            if (other.gameObject.tag == "Enemy")
            {
                // 调用敌方身上EnemyController脚本中的Hurt()函数，执行扣血。
                other.GetComponent<EnemyController>().Hurt(damage,attackPlayer);
            }
        }
        else
        {
            // 如果碰到的是敌人，则扣血
            if (other.gameObject.tag == "Player")
            {
                // 调用敌方身上EnemyController脚本中的Hurt()函数，执行扣血。
                other.GetComponent<HeroController>().Hurt(damage,attackPlayer);
            }
        }
    }
}
