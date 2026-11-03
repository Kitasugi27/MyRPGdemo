using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 引入UnityEngine.AI 命名空间
using UnityEngine.AI;

public class HeroController : MonoBehaviour
{
    // 定义一个NavMeshAgent对象，用它来控制人物在地图上移动。
    public NavMeshAgent nav;
    // 定义一个坐标变量，用来指定人物移动的终点。
    public Vector3 target;

    // 定义一个Animator的变量，用来获取自身的Animator组件
    public Animator anim;

    // 定义一个敌人的游戏物体的变量
    public GameObject tarEnemy;

    // 定义一个攻击状态
    public bool isAttack = false;

    // 定义一个主角的攻击位置，位于距离敌人1.1f左右，跟据实际情况调整。
    Vector3 attackPos;


    // 主角的基础属性
    // 定义主角的攻击力
    private float ATK = 20f;
    // 定义最大血量值
    private float HP_Max = 100f;
    // 定义当前血量值
    private float HP_Now;
    // 定义死亡状态
    private bool isDeath = false;

    // 定义调用敌人身上的脚本的变量EnemyController.cs
    public EnemyController enemyScript;

    // 定义一个画布中的脚本变量，用来获取画布中的血条
    public HPScript hpCanvas;

    void Start()
    {
        // 设置主角的当前生命值为最大生命值。
        HP_Now = HP_Max;
        // 设置血条的血量值
        hpCanvas.SetHP(HP_Now,HP_Max);

        // 将自身的NavMeshAgent组件值赋值给nav;
        nav = GetComponent<NavMeshAgent>();
        // 获取自身的Animator组件
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Debug.Log(gameObject.name + "主角的血量：" + HP_Now);
        DoMove();
        DoAttack();
    }

    // 定义移动函数，将和移动相关的代码放这里
    public void DoMove()
    {
        // 通过鼠标点击来设置人物的移动位置（终点）。
        // Input.GetMouseButtonDown(值),值：0表示鼠标左键      1表示鼠标右键
        if (Input.GetMouseButtonDown(0))
        {
            // 定义一个射线函数变量
            // 确定主摄像机的tag为MainCamera
            // 参数说明:
            // ‌Camera.main‌：获取场景中标记为 "MainCamera" 的主摄像机。
            // ‌Input.mousePosition‌：获取当前鼠标在屏幕上的像素坐标。
            // ‌ScreenPointToRay‌将鼠标的屏幕坐标转换为一条从摄像机镜头发出、穿过鼠标指针位置的三维空间射线
            // （Ray）。这条射线代表了目标对象“指向”的方向。
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // RaycastHit hit‌：声明一个结构体变量，用于存储射线碰撞到的详细信息
            // （如碰撞点位置、法线、碰撞物体等）。
            RaycastHit hit;

            // Physics.Raycast(ray, out hit)‌：物理引擎沿 ray 的方向发射射线。
            // 如果射线击中了任何带有碰撞器（Collider）的物体，函数返回 true，并将碰撞信息填入 hit 变量中。
            // 如果没有击中任何物体，返回 false，代码块内部不会执行。
            if (Physics.Raycast(ray, out hit))
            {
                // 如果射线射中的目标的tag为'earth'的物体，则将目标设置 为该物体的位置
                if (hit.transform.tag == "earth")
                {
                    // 设置攻击目标为空
                    tarEnemy = null;

                    // 如果移动时处在攻击状态，结束攻击动画
                    if (isAttack == true)
                    {
                        anim.SetTrigger("attackEnd");
                        nav.isStopped = false;
                    }

                    // 设置终点为鼠标所指物体的位置
                    target = hit.point;
                    nav.SetDestination(target);
                }
            }
        }

        // nav.velocity.magnitude用来判断人物移动速度大于0.1f，则执行"walk"动画
        if (nav.velocity.magnitude > 0.1f)
        {
            // 设置人物移动
            anim.SetBool("walk", true);
            // 设置攻击状态为结束
            anim.SetTrigger("attackEnd");
        }
        else
        {
            anim.SetBool("walk", false);

            // 设置攻击状态为结束
            anim.SetTrigger("attackEnd");
        }

    }

    // 定义一个攻击函数，将主角攻击的内容放这里
    public void DoAttack()
    {
        // 设置点击鼠标右键，如果点中的是敌人，就移到敌人面前
        if (Input.GetMouseButtonDown(1))
        {

            // 设置攻击状态
            // anim.SetTrigger("attack");

            // 判断是否遇到敌人
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // 如果射线射中的目标的tag为'Enemy'的物体，则将目标设置 为该物体的位置
                if (hit.transform.tag == "Enemy")
                {
                    // 获取敌人的位置
                    Vector3 enemyPos = hit.transform.position;
                    // 获取敌人的方向 = (敌人的位置 - 自己的位置).normalized
                    Vector3 dir = (enemyPos - transform.position).normalized;
                    // 设置主角的攻击位置，让主角跟离敌人1米远的位置就停止前进。
                    attackPos = enemyPos - dir * 1f;
                    // 让主角移动攻击位置上，停止前进。
                    nav.SetDestination(attackPos);

                    // 将碰撞目标设为敌人，将碰撞目标的变量赋给tarEnemey;
                    tarEnemy = hit.transform.gameObject;
                }
            }
        }

        // 判断如果敌人目标不空，且自身血量大于0，自动攻击
        // 防止诈尸
        if (tarEnemy != null && HP_Now > 0)
        {
            // Debug.Log(Vector3.Distance(transform.position, tarEnemy.transform.position));
            // 判断如果主角和敌人的距离小于1f，则攻击敌人
            if (Vector3.Distance(transform.position, tarEnemy.transform.position) <= 1.5f)
            {
                // 获取敌人的血量
                float enemyHP = tarEnemy.GetComponent<EnemyController>().showHP();
                // 调用敌人身上的脚本的伤害函数
                // 如果攻击状态为false，且敌人的血量大于0
                if (isAttack == false && enemyHP > 0 )
                {

                    // 防止主角攻击时受物理影响继续移动
                    nav.SetDestination(transform.position); //设置主角位置为自己坐标
                    // 设置寻址停止。
                    nav.isStopped = true;

                    // 设置攻击状态为true
                    isAttack = true;
                    // 播放攻击动画，此处只能播放一次攻击动画，就停止了。
                    anim.SetTrigger("attack");

                    // 1秒后执行攻击动画停止，可以再次执行攻击动画。
                    Invoke("AttackEnd", 1f);
                }
                else
                {
                    // 设置寻址停止。
                    nav.isStopped = false;
                }
            }
            else
            {
                // 设置寻址停止。
                nav.isStopped = false;
            }

            // 设置让主角一直面敌人
            // 获取敌和主角的方向
            Vector3 dir = tarEnemy.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            // 设置主角方向面向敌人
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    // 定义一个停卡攻击的函数
    public void AttackEnd()
    {
        // 设置攻击状态为false
        isAttack = false;
    }

    // 定义一个返回攻击力的函数
    public float showATK()
    {
        return ATK;
    }


    // 定义一个伤害、扣血的函数
    public void Hurt(float damage,Transform attacker)
    {
        // 计算伤害
        HP_Now = HP_Now - damage;

        // 设置血条的血量值
        hpCanvas.SetHP(HP_Now,HP_Max);

        // 如果敌人未死亡
        if (isDeath == false)
        {
            if (HP_Now <= 0)
            {
                // 设置死亡动画
                anim.SetBool("death", true);

                // 添加死亡音效
                // GameObject.Find("游戏物体的名字").GetComponent<游戏物体身上的插件名>();
                GameMusic gameMusic = GameObject.Find("GameMusic").GetComponent<GameMusic>();
                gameMusic.PlayMusic(1);
                
                // 2秒后销毁尸体
                Destroy(gameObject, 2f);
            }

            // 如果攻击者是敌人，则调用伤害函数
            if(attacker.tag == "Enemny")
            {
                // 获取攻击者身上的脚本HeroController.cs
                enemyScript = attacker.GetComponent<EnemyController>();
            }
        }

    }
}
