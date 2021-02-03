using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{

    public List<Door> doors;
    public List<Enemy> enemies;

    // Update is called once per frame
    void Update()
    {
        CheckEnemies();

        if (IsEnemiesKilled())
        {
            OpenDoors();
        }
    }

    private void CheckEnemies()
    {
        enemies.RemoveAll(e => !e.live.CheckDead());
    }

    public bool IsEnemiesKilled()
    {
        return enemies.Count == 0;
    }

    public void CloseDoors()
    {
        doors.ForEach(door => door.Close());
    }

    private void OpenDoors()
    {
        doors.ForEach(door => door.Open());
    }
}
