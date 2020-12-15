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
        SettingsData settings = new SettingsData();
        
        settings.monedas = new Monedas();
        settings.monedas.monedas = monedas;

        settings.tiempo = new Tiempo();
        settings.tiempo.tiempo = 2.7F;

        itemData = JsonMapper.ToJson(settings);
        File.WriteAllText(Application.dataPath + "/DatosUsuario/Datos.json", itemData.ToString());


        Debug.Log(JsonUtility.ToJson(settings));
    }


    [Serializable]
    public class SettingsData
    {
        public Tiempo tiempo;
        public Monedas monedas;
    }
    [Serializable]
    public class Tiempo
    {
        public float tiempo;
    }
    [Serializable]
    public class Monedas
    {
        public int monedas;
    }
}
