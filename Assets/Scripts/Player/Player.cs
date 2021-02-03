using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PlayerState
{
    Idle,
    Slide,
    Walk,
    Run,
    Jump,
    Attack,
    StrongAttack,
    Cast,
    UseItem,
    Fire,
    Evade,
    Heal,
    Interact,
    Hurt,
    Dash,
    WallGrab,
    WallSlide,
    Dead
}

public class Player : MonoBehaviour
{
    public PlayerState currentState = PlayerState.Idle;
    public Signal playerHealthSignal;
    public Signal playerMagicSignal;
    public Signal playerKicked;
    public Signal playerDieSignal;
    public PlayerPosition spawnPosition;
    public Inventory inventory;

    [Space] [Header("Projectiles")]
    public Projectile bowProjectile;
    public Projectile magicProjectile;

    [HideInInspector] public PlayerHealth health;
    [HideInInspector] public PlayerMagic magic;
    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public bool canFire = true;
    [HideInInspector] public bool canEvade = true;
    [HideInInspector] public bool combo;
    private AnimationScript anim;
    private Movement move;

    [Space] [Header("TriggersValue")]
    private static readonly int AttackAnimatorMapping = Animator.StringToHash("attack");

    private static readonly int StrongAttackAnimatorMapping = Animator.StringToHash("strongAttack");
    private static readonly int FireAnimatorMapping = Animator.StringToHash("fire");
    private static readonly int CastAnimatorMapping = Animator.StringToHash("cast");
    private static readonly int EndCastAnimatorMapping = Animator.StringToHash("endCast");
    private static readonly int HealAnimatorMapping = Animator.StringToHash("heal");
    private static readonly int EndHealAnimatorMapping = Animator.StringToHash("endHeal");
    private static readonly int EvadeAnimatorMapping = Animator.StringToHash("evade");
    private static readonly int HurtAnimatorMapping = Animator.StringToHash("hurt");
    private static readonly int DieAnimatorMapping = Animator.StringToHash("die");

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.Idle;

        anim = GetComponentInChildren<AnimationScript>();
        move = GetComponent<Movement>();
        health = GetComponent<PlayerHealth>();
        magic = GetComponent<PlayerMagic>();
        
        transform.position = spawnPosition.position.RuntimeValue;
    }

    private void Update()
    {
        if (health.CheckDead())
        {
            if (Input.GetButtonDown("Attack") && canAttack && currentState != PlayerState.Attack)
            {
                Attack();
            }

            if (Input.GetButton("Attack") && move.coll.onGround)
            {
                combo = true;
            }
            else if (Input.GetButtonUp("Attack") && !move.coll.onGround)
            {
                combo = false;
            } else if (Input.GetButtonDown("StrongAttack"))
            {
                if (canAttack && !move.coll.onGround && currentState != PlayerState.StrongAttack)
                {
                    anim.SetTrigger(StrongAttackAnimatorMapping);
                }
            }
            else if (Input.GetButtonDown("Bow") && canFire && currentState != PlayerState.Fire)
            {
                if (inventory.TakeArrow())
                {
                    Fire();
                }
            }
            else if (Input.GetButtonDown("Evade") && move.coll.onGround && canEvade && currentState != PlayerState.Evade)
            {
                anim.SetTrigger(EvadeAnimatorMapping);
            }
            else if (Input.GetButtonDown("CastSpell") && magic.CheckMagic(2f) && move.coll.onGround && currentState != PlayerState.Cast)
            {
                CastSpell();
            }
            else if (Input.GetButtonDown("Heal") && magic.CheckMagic(3f) && move.coll.onGround && currentState != PlayerState.Heal && !health.CheckFullHealth())
            {
                CastHeal();
            }
        }
    }

    public void Attack()
    {
        anim.SetTrigger(AttackAnimatorMapping);
        StartCoroutine(AttackCo());
    }

    private IEnumerator AttackCo()
    {
        canAttack = false;
        currentState = PlayerState.Attack;
        yield return new WaitForSeconds(0.75f);
        currentState = PlayerState.Idle;
        canAttack = true;
        combo = false;
    }
    
    public void Fire()
    {
        anim.SetTrigger(FireAnimatorMapping);
        StartCoroutine(FireCo());
    }

    private IEnumerator FireCo()
    {
        canFire = false;
        currentState = PlayerState.Fire;
        yield return new WaitForSeconds(0.5f);
        MakeProjectile(bowProjectile);
        currentState = PlayerState.Idle;
        yield return new WaitForSeconds(0.7f);
        canFire = true;
    }

    private void MakeProjectile(Projectile projectile)
    {
        Projectile instantiatedProjectile =
            Instantiate(projectile, transform.position + anim.direction * 1.5f, Quaternion.identity).GetComponent<Projectile>();

        instantiatedProjectile.Setup(anim.direction);
    }

    private void CastHeal()
    {
        currentState = PlayerState.Heal;
        LockMove();
        magic.SpendMagicPower(3f);
        playerMagicSignal.Raise();
        anim.SetTrigger(HealAnimatorMapping);
        StartCoroutine(HealCo());
    }

    public void Heal(float healPoints)
    {
        health.RestoreHealth(healPoints);
        playerHealthSignal.Raise();
    }

    private IEnumerator HealCo()
    {
        yield return new WaitForSeconds(2.5f);
        Heal(2f);
        anim.SetTrigger(EndHealAnimatorMapping);
        UnlockMove();
    }

    private void CastSpell()
    {
        currentState = PlayerState.Cast;
        LockMove();
        magic.SpendMagicPower(2f);
        playerMagicSignal.Raise();
        anim.SetTrigger(CastAnimatorMapping);
        StartCoroutine(CastCo(1.5f));
    }

    private IEnumerator CastCo(float duration)
    {
        yield return new WaitForSeconds(duration / 2);
        MakeProjectile(magicProjectile);
        yield return new WaitForSeconds(duration / 2);
        anim.SetTrigger(EndCastAnimatorMapping);
        UnlockMove();
    }

    public void Hit(float damage)
    {
        health.ApplyDamage(damage);
        
        if (health.CheckDead())
            anim.SetTrigger(HurtAnimatorMapping);
        else
            StartCoroutine(Die());

        playerHealthSignal.Raise();
        playerKicked.Raise();
    }

    public void Knock(Rigidbody2D myRigidBody, Vector3 hitForce, float knockTime)
    {
        if (myRigidBody != null && currentState != PlayerState.Hurt)
        {
            StartCoroutine(KnockCo(myRigidBody, hitForce, knockTime));
        }
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidBody, Vector3 hitForce, float knockTime)
    {
        myRigidBody.AddForce(hitForce);
        LockMove();
        currentState = PlayerState.Hurt;
        yield return new WaitForSeconds(knockTime);
        myRigidBody.velocity = Vector2.zero;
        currentState = PlayerState.Idle;
        UnlockMove();
    }
    
    private IEnumerator Die()
    {
        currentState = PlayerState.Dead;
        anim.SetTrigger(DieAnimatorMapping);
        yield return new WaitForSeconds(1f);
        playerDieSignal.Raise();
    }

    public void LockMove()
    {
        move.canMove = false;
    }

    public void UnlockMove()
    {
        move.canMove = true;
    }

    public void UnlockCombo()
    {
        combo = false;
    }
}