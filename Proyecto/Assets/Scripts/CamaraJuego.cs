using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraJuego : MonoBehaviour
{
	public GameObject follow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float posy=follow.transform.position.y;
		
		transform.position=new Vector3(transform.position.x,posy,transform.position.z);
    }
}
