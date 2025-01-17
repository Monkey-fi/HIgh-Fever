using UnityEngine;
using System.Collections; // Add this line to use IEnumerator

public class PlayerRespawn : MonoBehaviour
{
    // Store the starting position of the player
    private Vector3 startingPosition;

    // Optional: You can set a respawn delay
    public float respawnDelay = 0f;

    void Start()
    {
        // Store the initial position of the player
        startingPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the plane
        if (other.CompareTag("Plane"))
        {
            // Start the respawn coroutine
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        // Optional: Wait for a specified delay before respawning
        yield return new WaitForSeconds(respawnDelay);

        // Reset the player's position to the starting position
        transform.position = startingPosition;
    }
}