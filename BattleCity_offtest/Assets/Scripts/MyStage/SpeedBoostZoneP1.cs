using UnityEngine;

public class SpeedBoostZoneP1 : MonoBehaviour
{
    // Hệ số tăng tốc (ví dụ: 1.5 nghĩa là tốc độ tăng 50%)
    public int boostAdd = 5;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTank"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                // Tăng tốc độ của player
                player.speed = player.defaultSpeed + boostAdd;
                Debug.Log("Player team " + player + " entered boost zone. New speed: " + player.speed);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTank"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                // Reset tốc độ về mặc định bằng cách chia lại boostMultiplier
                player.speed = player.defaultSpeed;
                Debug.Log("Player team " + player + " left boost zone. Speed reset to: " + player.speed);
            }
        }
    }
}
