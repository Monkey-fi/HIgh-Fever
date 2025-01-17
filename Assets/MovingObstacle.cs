using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public Vector3 pointA; // Start position offset
    public Vector3 pointB; // End position offset
    public float speed = 2f; // Speed of the movement

    private Vector3 startPosition; // Initial position of the obstacle
    private Vector3 targetPosition; // Current target position

    void Start()
    {
        // Save the initial position of the obstacle
        startPosition = transform.position;

        // Set the first target position
        targetPosition = startPosition + pointB;
    }

    void Update()
    {
        // Move the obstacle towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // If the obstacle reaches the target position, switch to the other point
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = targetPosition == startPosition + pointA ? startPosition + pointB : startPosition + pointA;
        }
    }
}
