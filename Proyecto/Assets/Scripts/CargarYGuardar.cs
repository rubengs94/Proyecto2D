using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.IO;
using LitJson;

public class CargarYGuardar : MonoBehaviour
{
    //json
    private JsonData itemData;
    private String jsonText;
    //variables de usuario
    public int monedas;
    
    private void Start()
    {
        DatosUsuario settings = new DatosUsuario(monedas, 2.7f);

        itemData = JsonMapper.ToJson(settings);
        File.WriteAllText(Application.dataPath + "/DatosUsuario/Datos.txt", itemData.ToString());

        Debug.Log(JsonUtility.ToJson(settings));
    }


    class DatosUsuario
    {
        public int monedas;
        public float tiempo;

        public DatosUsuario(int monedas, float tiempo)
        {
            this.monedas = monedas;
            this.tiempo = tiempo;
        }
    }

}
