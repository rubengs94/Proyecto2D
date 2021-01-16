using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlJugador : MonoBehaviour
{

    public GameObject gameOver;
    public Text texto;
    private BoxCollider2D bc2d;
    private BoxCollider2D bc2dChildren;
    private Rigidbody2D rb2d;

    private void Start()
    {
        bc2d = GetComponentInParent<BoxCollider2D>();
        bc2dChildren = GameObject.Find("CheckGround").GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        gameOver.SetActive(false);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Lava" ||
            collision.gameObject.tag == "Enemy" ||
            collision.gameObject.tag == "Trampa" )
        {

            Destroy(bc2d);
            Destroy(bc2dChildren);
            rb2d.gravityScale = 0;
            gameOver.SetActive(true);

            if (collision.gameObject.tag == "Lava")
            {
                texto.text = "Ten cuidado, la lava puede estar muy caliente";
            }

            if (collision.gameObject.tag == "Enemy")
            {
                texto.text = "Los esqueletos son peligrosos, no te acerques a ellos";
            }

            if (collision.gameObject.tag == "Trampa")
            {
                texto.text = "Evita caer en trampas, presta atencion al entorno";
            }

            Time.timeScale = 0;

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Lava")
        {

            Destroy(bc2d);
            Destroy(bc2dChildren);
            rb2d.gravityScale = 0;
            gameOver.SetActive(true);
            texto.text = "Consejo: Evita entrar en contacto con la lava";
            Time.timeScale = 0;

        }
    }


    public void QuitarUI()
    {
        gameOver.SetActive(false);
    }
}
