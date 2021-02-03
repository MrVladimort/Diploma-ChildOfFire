using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Healthbar healthbar;

    public EnemyState currentState;

    public float attackDistance = 2f;
    public float attackInterval = 2f;
    public float rushSpeed = 10f;
    public float normalSpeed = 5f;

    [Space] [Header("Push Left")] public Transform pushLeft;
    [Space] [Header("Push Right")] public Transform pushRight;

    [HideInInspector] public Player player;
    [HideInInspector] public LiveThing live;

    protected bool canAttack = true;

    private GameMaster gameMaster;
    private bool active;
    private bool rushing;
    private bool canHurt = true;
    private LinkedList<Transform> rushPoints = new LinkedList<Transform>();
    protected Animator anim;
    protected MoveableObject move;

    protected static readonly int AttackAnimation = Animator.StringToHash("Attack");
    private static readonly int HurtAnimatorMapping = Animator.StringToHash("Hurt");
    private static readonly int DieAnimatorMapping = Animator.StringToHash("Die");

    private static readonly int HorizontalVelocityAnimatorMapping = Animator.StringToHash("HorizontalVelocity");

    void Start()
    {
        gameMaster = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        move = GetComponent<MoveableObject>();
        live = GetComponent<LiveThing>();
        anim = GetComponent<Animator>();
        player = gameMaster.GetPlayer();
        
        healthbar.SetMaxHealth(live.health.initValue);
        healthbar.SetHealth(live.health.initValue);
    }

    public void Activate()
    {
        if (!active)
        {
            active = true;
            move.canMove = true;
            healthbar.gameObject.SetActive(true);
            StartCoroutine(RushDelayCo(8f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (live.CheckDead() && active)
        {
            if (!rushing)
            {
                move.speed = normalSpeed;
                BaseLogic();
            }
            else
            {
                move.speed = rushSpeed;
                RushLogic();
            }
            
            SetAnimatorParametres();
        }
        else
        {
            move.StopMoving();
            move.canMove = false;
        }
    }

    private void SetAnimatorParametres()
    {
        anim.SetFloat(HorizontalVelocityAnimatorMapping, Mathf.Abs(move.move.x));
    }

    public bool CheckAttackDistance()
    {
        return CheckDistanceToPlayer(attackDistance) && player.health.CheckDead();
    }


    public bool CheckDistanceToPlayer(float distance)
    {
        var playerPosition = player.transform.position;
        return move.CheckDistanceToTarget(playerPosition, distance) && player.health.CheckDead();
    }

    public Vector3 GetDirectionToPlayer()
    {
        var playerPosition = player.transform.position;
        return move.GetDirectionToTarget(playerPosition, true);
    }

    private void BaseLogic()
    {
        if (CheckAttackDistance())
        {
            move.StopMoving();
            Attack();
        }
        else
        {
            move.Move(GetDirectionToPlayer());
        }
    }

    private void RushLogic()
    {
        if (move.canMove)
        {
            if (!rushPoints.Any())
            {
                Vector3 dirToPlayer = GetDirectionToPlayer();

                if (dirToPlayer.x > 0f)
                {
                    rushPoints.AddLast(pushRight);
                    rushPoints.AddLast(pushLeft);
                    rushPoints.AddLast(pushRight);
                    rushPoints.AddLast(pushLeft);
                }
                else
                {
                    rushPoints.AddLast(pushLeft);
                    rushPoints.AddLast(pushRight);
                    rushPoints.AddLast(pushLeft);
                    rushPoints.AddLast(pushRight);
                }
            }

            if (!move.CheckDistanceToTarget(rushPoints.First.Value.position, attackDistance))
            {
                move.Move(move.GetDirectionToTarget(rushPoints.First.Value.position, true));
            }
            else
                rushPoints.RemoveFirst();
            
            if (!rushPoints.Any())
            {
                StartCoroutine(RushBreakCo(3f));
            }
        }
    }

    private IEnumerator RushDelayCo(float delay)
    {
        yield return new WaitForSeconds(delay);
        canHurt = false;
        anim.SetTrigger("RushStart");
        rushing = true;
        move.canMove = false;
        move.StopMoving();
        yield return new WaitForSeconds(0.7f);
        move.canMove = true;
        anim.SetTrigger("Rush");
    }
    
    private IEnumerator RushBreakCo(float delay)
    {
        anim.SetTrigger("RushBreak");
        move.canMove = false;
        move.StopMoving();
        canHurt = true;
        yield return new WaitForSeconds(delay);
        rushing = false;
        move.canMove = true;
        anim.SetTrigger("RushEnd");
        StartCoroutine(RushDelayCo(8f));
    }
    public void Attack()
    {
        if (currentState != EnemyState.Attack && currentState != EnemyState.Dead && canAttack)
        {
            StartCoroutine(AttackCo());
        }
    }

    private IEnumerator AttackCo()
    {
        canAttack = false;
        currentState = EnemyState.Attack;
        AttackLogic();
        yield return new WaitForSeconds(attackInterval / 4);
        currentState = EnemyState.Idle;
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
    }
    
    private IEnumerator HurtDelayCo(float delay)
    {
        canHurt = false;
        yield return new WaitForSeconds(delay);
        canHurt = true;
    }

    protected virtual void AttackLogic()
    {
        anim.SetTrigger(AttackAnimation);
    }

    public void Hit(float damage)
    {
        if (canHurt)
        {
            live.ApplyDamage(damage);
            healthbar.TakeDamage(damage);

            if (live.CheckDead())
            {
                if ( currentState != EnemyState.Attack) 
                    anim.SetTrigger(HurtAnimatorMapping);
            }
            else
                StartCoroutine(Die());

            StartCoroutine(HurtDelayCo(0.6f));
        }
    }

    private IEnumerator Die()
    {
        currentState = EnemyState.Dead;
        move.StopMoving();
        move.canMove = false;
        anim.SetTrigger(DieAnimatorMapping);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        gameMaster.GetLevelManager().FadeToLevel("Scenes/EndMenu");
    }
}