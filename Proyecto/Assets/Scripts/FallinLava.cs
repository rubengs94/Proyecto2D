using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallinLava : MonoBehaviour
{
    public float fallDelay = 0.5f;
    public float respawnDelay = 5f;

    private Rigidbody2D rb2d;
    private Vector3 start;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        start = transform.position;
        Invoke("Respawn",respawnDelay);
        
    }


    /// <summary>
    /// Respawn del GameObject
    /// </summary>
    void Respawn()
    {
        transform.position = start;
        rb2d.velocity = Vector3.zero;
        Invoke("Respawn",respawnDelay);
    }
}
