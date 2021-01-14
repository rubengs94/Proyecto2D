using UnityEngine;

public class PruebaMovimiento : MonoBehaviour
{
	public bool grounded;
    public new AudioSource audio;
	public float jumpPower;
	private Rigidbody2D rb2d;
	private Animator anim;
	private bool jump;

    // Start is called before the first frame update
    void Start()
    {

        rb2d=GetComponent<Rigidbody2D>();
		anim=GetComponent<Animator>();
		
    }

    // Update is called once per frame
    void Update()
    {
        
		anim.SetBool("Grounded", grounded);
		if(Input.GetButtonDown("Jump") && grounded)
        {
			audio.Play();//sonido salto
		    jump= true;
		}
		
		
    }
	
	void FixedUpdate()
	{
		if(Input.GetKey(KeyCode.D) || Input.GetKey("right")){
			anim.SetBool("Speed",true);
            if(GetComponent<SpriteRenderer>().flipX==true){
                GetComponent<SpriteRenderer>().flipX=false;
            }
            transform.Translate(0.05f,0,0);
        }
        else if(Input.GetKey(KeyCode.A) || Input.GetKey("left")){
			anim.SetBool("Speed",true);
            if(GetComponent<SpriteRenderer>().flipX==false){
                GetComponent<SpriteRenderer>().flipX=true;
            }
            transform.Translate(-0.05f,0,0);
        }
        else{
			anim.SetBool("Speed",false);
            transform.Translate(0,0,0);
        }
		
		
		
		
		//salto
		if(jump){
			anim.SetBool("Speed",false);
			rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
			jump=false;
		}
		//Debug.Log(rb2d.velocity.x);
	}

    #region RESPAWN

    public void Respawn()
    {
        Invoke("LlamadaRespawn",1f);
    }

    /// <summary>
    /// mueve el personaje al principio del mapa 1
    /// </summary>
    void LlamadaRespawn()
    {
        transform.position = new Vector3(-0.04f, -1.39f, 0.0f);
    }


    #endregion

    #region coins



    #endregion

}
