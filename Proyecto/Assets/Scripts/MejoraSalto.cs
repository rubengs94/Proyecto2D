 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MejoraSalto : MonoBehaviour
{
	public float saltoAlto= 5f;
	public float saltoCorto= 1f;
	
	Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.y < 0){
			rb.velocity += Vector2.up * Physics2D.gravity.y * saltoAlto * Time.deltaTime;
		}else if(rb.velocity.y > 0 && !Input.GetButtonDown("Jump")){
			rb.velocity += Vector2.up * Physics2D.gravity.y * saltoCorto * Time.deltaTime;
		}
			
    }
}
