using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawn : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("w");
    }
    public Vector3 startposition;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("reset"))
        {
            Debug.Log("collision");
            DelayPlayer();
        }
    }
    void Respawn()
    {
        transform.position = startposition;
    }
    void DelayPlayer()
    {
        Debug.Log("hitting");
        Invoke("Respawn", 2f);

    }
}
