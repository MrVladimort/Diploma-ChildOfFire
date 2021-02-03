using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Attack,
    Hurt,
    Patrolling,
    Chasing,
    Dead
}

public class Enemy : MonoBehaviour
{
    public string enemyName = "Enemy";
    public EnemyState currentState;

    [Space] [Header("Loot")] public LootTable lootTable;

    [Space] [Header("Distances")] public float maxAngleOfPlayerDetection = 10f;
    public float maxDistanceOfPlayerDetection = 15f;
    public float minDistanceOfPlayerDetection = 4f;
    public float attackDistance = 2f;
    public float attackInterval = 2f;

    [Space] [Header("Detection")] public Transform trackPoint;

    [HideInInspector] public Player player;
    [HideInInspector] public LiveThing live;
    [HideInInspector] public bool seePlayer;

    protected bool canAttack = true;

    private GameMaster gameMaster;
    protected Animator anim;
    protected MoveableObject move;

    protected static readonly int AttackAnimation = Animator.StringToHash("attack");
    private static readonly int HurtAnimatorMapping = Animator.StringToHash("hurt");
    private static readonly int DieAnimatorMapping = Animator.StringToHash("die");

    private static readonly int HorizontalVelocityAnimatorMapping = Animator.StringToHash("HorizontalVelocity");

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        move = GetComponent<MoveableObject>();
        live = GetComponent<LiveThing>();
        anim = GetComponent<Animator>();
        player = gameMaster.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (live.CheckDead())
        {
            Track();
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

    private void Track()
    {
        if (!player.health.CheckDead() || !live.CheckDead())
        {
            seePlayer = false;
            return;
        }

        // if player is visible
        if (!CheckDistanceToPlayer(maxDistanceOfPlayerDetection))
        {
            seePlayer = false;
            return;
        }

        var position = transform.position;
        var playerPosition = player.transform.position;
        var trackPosition = trackPoint.position;

        if (CheckDistanceToPlayer(minDistanceOfPlayerDetection))
        {
            seePlayer = true;
            return;
        }

        if (Mathf.Abs(Vector3.Angle(move.direction * -1, position - playerPosition)) < maxAngleOfPlayerDetection)
        {
            Debug.DrawRay(trackPosition,
                (playerPosition - position).normalized *
                maxDistanceOfPlayerDetection);

            RaycastHit2D playerHit = Physics2D.Raycast(trackPosition,
                (playerPosition - position).normalized,
                maxDistanceOfPlayerDetection);

            if (playerHit != true) return;
            if (playerHit.collider.CompareTag("Player")) seePlayer = true;
        }
        else
        {
            seePlayer = false;
        }
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

    protected virtual void AttackLogic()
    {
        anim.SetTrigger(AttackAnimation);
    }

    public void Hit(float damage)
    {
        live.ApplyDamage(damage);

        if (live.CheckDead())
        {
            anim.SetTrigger(HurtAnimatorMapping);
            seePlayer = true;
        }
        else
            StartCoroutine(Die());
    }

    public void Knock(Rigidbody2D myRigidBody, Vector3 hitForce, float knockTime)
    {
        if (myRigidBody != null && currentState != EnemyState.Hurt && currentState != EnemyState.Attack &&
            currentState != EnemyState.Dead)
        {
            StartCoroutine(KnockCo(myRigidBody, hitForce, knockTime));
        }
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidBody, Vector3 hitForce, float knockTime)
    {
        myRigidBody.AddForce(hitForce);
        move.canMove = false;
        currentState = EnemyState.Hurt;
        yield return new WaitForSeconds(knockTime);
        myRigidBody.velocity = Vector2.zero;
        currentState = EnemyState.Idle;
        move.canMove = true;
    }

    private void SpawnLoot()
    {
        PowerupTrigger powerup = lootTable.GetLoot();

        if (powerup != null)
        {
            Instantiate(powerup, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator Die()
    {
        currentState = EnemyState.Dead;
        move.StopMoving();
        move.canMove = false;
        anim.SetTrigger(DieAnimatorMapping);
        yield return new WaitForSeconds(1.5f);
        SpawnLoot();
        Destroy(gameObject);
    }
}