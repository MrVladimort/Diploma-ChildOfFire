using UnityEngine;

namespace Trigger
{
    public class DieTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D player)
        {
            if (player.CompareTag("Player"))
            {
                player.GetComponent<Player>().Hit(99999f);
            }
        }
    }
}