using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallin : MonoBehaviour
{
    private float fallDelay = 0.5f;
    private float respawnDelay = 5f;

    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;
    private Vector3 start;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        start = transform.position;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            
            Invoke("Fall", fallDelay);
            Invoke("Respawn", fallDelay + respawnDelay);

        }
    }

    /// <summary>
    /// Caida del GameObject
    /// </summary>
    void Fall()
    {
        rb2d.isKinematic = false;
        bc2d.isTrigger = true;
    }


    /// <summary>
    /// Respawn del GameObject
    /// </summary>
    void Respawn()
    {
        transform.position = start;
        rb2d.isKinematic = true;
        bc2d.isTrigger = false;
        rb2d.velocity = Vector3.zero;
    }

}
