using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoomTrigger : MonoBehaviour
{

    public EnemyRoom enemyRoom;
    
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player") && !enemyRoom.IsEnemiesKilled())
        {
            enemyRoom.CloseDoors();
        }
    }
}
