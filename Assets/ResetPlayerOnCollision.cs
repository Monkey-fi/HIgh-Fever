using UnityEngine;

public class ResetPlayerOnCollision : MonoBehaviour
{
    // Store the player's starting position
    private Vector3 startingPosition;

    void Start()
    {
        // Record the starting position when the game starts
        startingPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is tagged as "Plane"
        if (collision.gameObject.CompareTag("Plane"))
        {
            ResetToStartingPosition();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Alternative if the plane uses a trigger collider
        if (other.CompareTag("Plane"))
        {
            ResetToStartingPosition();
        }
    }

    void ResetToStartingPosition()
    {
        // Reset the player's position to the starting position
        transform.position = startingPosition;
        Debug.Log("Player reset to starting position.");
    }
}
