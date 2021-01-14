using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsButton : MonoBehaviour
{

    public GameObject piedra;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            piedra.SetActive(true);
        }
    }

}
