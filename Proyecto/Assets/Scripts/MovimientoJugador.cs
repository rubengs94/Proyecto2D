using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    Rigidbody2D rb;
    float salto=3f;
    public static bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
       rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.D) || Input.GetKey("right")){
            if(GetComponent<SpriteRenderer>().flipX==true){
                GetComponent<SpriteRenderer>().flipX=false;
            }
            transform.Translate(0.05f,0,0);
        }
        else if(Input.GetKey(KeyCode.A) || Input.GetKey("left")){
            if(GetComponent<SpriteRenderer>().flipX==false){
                GetComponent<SpriteRenderer>().flipX=true;
            }
            transform.Translate(-0.02f,0,0);
        }
        else{
            transform.Translate(0,0,0);
        }
        if(Input.GetKey(KeyCode.Space) && isGrounded==true){
            rb.AddForce(new Vector2(0f,salto),ForceMode2D.Impulse);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision){
        isGrounded=true;
    }
    private void OnTriggerExit2D(Collider2D collision){
        isGrounded=false;
    }

    #region CHOQUE ENEMIGOS/TRAMPAS


    #endregion


}
