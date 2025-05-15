using UnityEngine;

public class SpeedBoostZoneP2 : MonoBehaviour
{
    // Hệ số tăng tốc (ví dụ: 1.5 nghĩa là tốc độ tăng 50%)
    public int boostAdd = 5;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("EnemyTank"))
        {
            Player2Movement player = other.GetComponent<Player2Movement>();
            if (player != null)
            {
                // Tăng tốc độ của player
                player.speed = player.defaultSpeed + boostAdd;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("EnemyTank"))
        {
            Player2Movement player = other.GetComponent<Player2Movement>();
            if (player != null)
            {
                // Reset tốc độ về mặc định bằng cách chia lại boostMultiplier
                player.speed = player.defaultSpeed;
            }
        }
    }
}
