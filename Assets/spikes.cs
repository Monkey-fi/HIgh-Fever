using UnityEngine;

public class spikes : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public float height = 1f; // Movement height

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time * speed) * height, 0);
    }
}
