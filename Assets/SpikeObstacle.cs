using UnityEngine;

public class SpikeObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player hits the spike
        if (other.CompareTag("Player"))
        {
            // Replace this with your player damage logic
            Debug.Log("Player hit the spike!");
            
        }
    }
}
