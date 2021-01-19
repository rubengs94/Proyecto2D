using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverFondo : MonoBehaviour
{
	public Renderer Fondo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Fondo.material.mainTextureOffset=Fondo.material.mainTextureOffset+new Vector2(0.020f,0)*Time.deltaTime;
    }
}
