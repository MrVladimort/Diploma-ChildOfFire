using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public float speed;
    public Vector3 direction = new Vector3(1, 0, 0);
    public Vector3 move = Vector3.zero;
    public bool canMove = true;

    private void Update()
    {
        Flip();

        if (canMove)
            transform.Translate(move);
    }

    public void Move(Vector2 movingVector)
    {
        Vector3 newMove = Time.deltaTime * speed * movingVector;
        move = newMove;
    }

    public void StopMoving()
    {
        Move(Vector2.zero);
    }

    private void Flip()
    {
        if (move.x < -0.001f && direction.x == 1) FlipGameObject();
        else if (move.x > 0.001f && direction.x == -1) FlipGameObject();
    }

    private void FlipGameObject()
    {
        direction *= -1;
        Vector3 mirrorScale = gameObject.transform.localScale;
        mirrorScale.x *= -1;
        gameObject.transform.localScale = mirrorScale;
    }

    public bool CheckDistanceToTarget(Vector3 targetPosition, float distance)
    {
        var position = transform.position;
        return Vector3.Distance(position, targetPosition) <= distance;
    }

    public Vector3 GetDirectionToTarget(Vector3 targetPosition, bool isHorizontal)
    {
        var heading = targetPosition - transform.position;
        var distance = heading.magnitude;
        var directionToTarget = heading / distance; // This is now the normalized direction.

        if (isHorizontal) return new Vector3(directionToTarget.x, 0, 0);

        return directionToTarget;
    }

    public static bool IsNotSameDirection(float a, float b)
    {
        return !(a < 0 && b < 0 || a > 0 && b > 0);
    }
}