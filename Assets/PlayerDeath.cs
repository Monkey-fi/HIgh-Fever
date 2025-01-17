using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public string obstacleTag = "Obstacle"; // Tag to identify obstacles

    // This method is triggered when the player collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the specified tag
        if (collision.gameObject.CompareTag(obstacleTag))
        {
            // Call the player death logic
            Die();
        }
    }

    // Logic for what happens when the player dies
    void Die()
    {
        Debug.Log("Player is dead!");

        // Optionally disable player controls or hide the player
        gameObject.SetActive(false);

        // Add additional death logic here (e.g., restart the game, show a game-over screen, etc.)
    }
}
