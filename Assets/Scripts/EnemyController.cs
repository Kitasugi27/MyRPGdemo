using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 引入UnityEngine.AI 命名空间
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    // 定义是随感血量值 
    // private和public的区别：
    // public类型的变量，Unity界面可以读取和设置，Unity会锁死此变量类型，
    // private 定义的变量只能代码获取，Unity也可以读取，但不会锁死变量。

    // 定义最大血量值
    private float HP_Max = 100f;
    // 定义当前血量值
    private float HP_Now;

    // 定义一个animator的变量
    public Animator animator;

    // 定义主角的攻击力
    private float ATK = 5f;


    // 定义一个死亡变量，用来判断是否死亡
    public bool isDeath = false;
    // 定义一个获取agent组件的变量
    public NavMeshAgent agent;
    // 定义目标位置的坐标变量
    public Transform target;
    // 定义一个目标列表
    public Transform[] targetList;

    // 判断是否被攻击
    public bool beAttack = false;
    // 获取攻击者的变量
    public Transform attackPlayer;

    // 定义一个攻击状态
    public bool isAttack = false;


    // 创建一个玩家脚本的游戏物体。
    public HeroController heroScript;

     // 定义血条的变量
    public HPScript hpCanvas;


    // 创建一个爆炸物体的粒子变量
    public ParticleSystem explosion;

    void Start()
    {
        // 设置敌人的默认血量
        HP_Now = HP_Max;

        // 设置血条的血量
        hpCanvas.SetHP(HP_Now, HP_Max);


        // 获取自己身上的动画控制器
        animator = GetComponent<Animator>();
        // 首先将标位置设置为目标列表中的第一项
        target = targetList[0];

        // 获取自己上的NavMeshAgent组件
        agent = GetComponent<NavMeshAgent>();

        // 爆炸动画默认不播放
        explosion.Stop();

    }
    void Update()
    {
        // Debug.Log(gameObject.name + "敌人的血量：" + HP_Now);

        DoMove();
        DoAttack();

    }

    // 定义一个伤害、扣血的函数
    public void Hurt(float damage,Transform attacker)
    {
        // 计算伤害
        HP_Now = HP_Now - damage;

        // 设置血条的血量
        hpCanvas.SetHP(HP_Now, HP_Max);

        // 如果敌人未死亡
        if (isDeath == false)
        {
            if (HP_Now <= 0)
            {
                // 设置死亡状态为true
                isDeath = true;
                // 设置攻击状态为false
                isAttack = false;
                // 设置攻击者为null
                attackPlayer = null;
                // 设置攻击状态为false
                animator.SetTrigger("attackEnd");

                // 设置死亡动画
                animator.SetBool("death", true);

                // 播放爆炸动画
                explosion.Play();

            
                // 添加死亡音效
                // GameObject.Find("游戏物体的名字").GetComponent<游戏物体身上的插件名>();
                GameMusic gameMusic = GameObject.Find("GameMusic").GetComponent<GameMusic>();
                gameMusic.PlayMusic(1);
                
                // 2秒后销毁尸体
                Destroy(gameObject, 2f);
            }

            // 如果攻击者是主角hero，调用伤害函数
            if(attacker.tag == "Player")
            {
                // 获取攻击者身上的脚本HeroController.cs
                heroScript = attacker.GetComponent<HeroController>();
            }
        }

    }

    // 碰撞检测
    private void OnTriggerEnter(Collider other)
    {
        // 判断碰撞体是主角的攻击盒子
        if (other.gameObject.tag == "AttackBox")
        {
            // 设置被攻击的状态为true;
            beAttack = true;
        }
    }

    public void DoMove()
    {
        // 判断如果敌人未死亡，则向目标地点移动。
        if (isDeath == false)
        {
            // 如果没有受到攻击，按原有路线继续巡逻。
            if (beAttack == false)
            {
                // 如果跟离目标位置小于等于1.5f，则切换下一个目标位置
                if (Vector3.Distance(transform.position, target.position) <= 1.5f)
                {
                    // 随机切换下一个目标
                    target = targetList[Random.Range(0, targetList.Length)];
                }

            }
            else
            {
                // 设置移动目标为攻击者的位置。
                target = attackPlayer;

                // 设置敌人到达主角的距离。
                agent.stoppingDistance = 1.2f;
            }

            // 如果主角没有死亡，让敌人追着主角攻击。
            if(attackPlayer != null)
            {
                agent.SetDestination(target.position);
            }

            // 设置走路动画
            // agent.velocity.magnitude用来判断人物移动速度大于0.05f，则执行"walk"动画
            if (agent.velocity.magnitude > 0.05f)
            {
                // 设置人物移动
                animator.SetBool("walk", true);
            }
            else
            {
                animator.SetBool("walk", false);
            }
        }
    }



    // 定义一个攻击函数，在这里执行攻击效果
    public void DoAttack()
    {
        if (isDeath == false)
        {
            // 防诈尸，必须是攻击者不为空且血大于0
            if (attackPlayer != null && HP_Now>0)
            {
                // 判断自己和攻击目标的距离
                if (Vector3.Distance(transform.position, attackPlayer.transform.position) <= 1.7f)
                {
                    // 判断当前攻击状态
                    if (isAttack == false)
                    {
                        // 攻击开始，自己不再移动
                        agent.SetDestination(attackPlayer.transform.position);
                        agent.isStopped = true;

                        // 播放攻击动画
                        isAttack = true;
                        animator.SetTrigger("attack");

                        // 1秒后执行攻击动画停止，可以再次执行攻击动画。
                        Invoke("AttackEnd", 1f);
                    }
                    else
                    {
                        // 攻击结束，自己可以继续移动
                        agent.isStopped = false;
                        animator.SetTrigger("attackEnd");
                    }

                    // 设置让自己一直面主角
                    Vector3 dir = attackPlayer.transform.position - transform.position;
                    dir.y = 0;
                    dir.Normalize();
                    // 设置主角方向面向敌人
                    transform.rotation = Quaternion.LookRotation(dir);
                }
            }
        }
    }

    // 定义一个攻击结束的函数
    public void AttackEnd()
    {
        isAttack = false;
    }

    // 定义一个返回攻击力的函数
    public float showATK()
    {
        return ATK;
    }

    // 定义一个返回敌人血量的函数
    public float showHP()
    {
        return HP_Now;
    }
}
