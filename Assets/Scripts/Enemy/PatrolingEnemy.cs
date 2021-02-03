using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PatrolingEnemy : MonoBehaviour
{
    private MoveableObject move;
    private Animator anim;
    private Enemy enemy;
    private Vector3 spawnPoint;
    private bool sawPlayer;

    public Transform[] patrolPoints;
    public float patrolDelay;

    private int patrollingIndex;
    private float switchTime = float.PositiveInfinity;
    private Vector3 currentPatrolPoint;
    private bool wasAtSpawn = true;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        move = GetComponent<MoveableObject>();
        enemy = GetComponent<Enemy>();

        var position = transform.position;
        spawnPoint = new Vector3(position.x, position.y, position.z);
        currentPatrolPoint = spawnPoint;
    }

    private void Update()
    {
        if (enemy.currentState == EnemyState.Dead) return;

        if (enemy.seePlayer)
        {
            if (!sawPlayer)
            {
                sawPlayer = true;
                wasAtSpawn = false;
            }

            if (enemy.CheckAttackDistance())
            {
                move.StopMoving();
                enemy.Attack();
            }
            else
            {
                if (enemy.currentState != EnemyState.Attack && enemy.currentState != EnemyState.Hurt)
                {
                    enemy.currentState = EnemyState.Chasing;
                    move.Move(enemy.GetDirectionToPlayer());
                }
            }
        }
        else
        {
            if (sawPlayer)
            {
                StartCoroutine(CalmDownCo());
            }

            var checkedDistanceToSpawn = move.CheckDistanceToTarget(spawnPoint, 1f);
            if (!checkedDistanceToSpawn && !sawPlayer && !wasAtSpawn)
            {
                enemy.currentState = EnemyState.Patrolling;
                move.Move(move.GetDirectionToTarget(spawnPoint, true) * 0.5f);
            }
            else if (checkedDistanceToSpawn && !sawPlayer && !wasAtSpawn)
            {
                wasAtSpawn = true;
            }
            else if (!sawPlayer && wasAtSpawn)
            {
                Patrolling();
            }
            else
            {
                enemy.currentState = EnemyState.Idle;
                move.StopMoving();
            }
        }
    }

    private IEnumerator CalmDownCo()
    {
        yield return new WaitForSeconds(2f);
        sawPlayer = false;
    }

    void Patrolling()
    {
        if (patrolPoints.Length == 0) return;

        if (move.CheckDistanceToTarget(currentPatrolPoint, 1f))
        {
            move.StopMoving();
            patrollingIndex++;
            patrollingIndex %= patrolPoints.Length;
            currentPatrolPoint = patrolPoints[patrollingIndex].position;
        }
        else
        {
            Vector3 point = move.GetDirectionToTarget(currentPatrolPoint, true) * 0.5f;
            move.Move(point);
        }
    }
}