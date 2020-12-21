using LitJson;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CargarYGuardar : MonoBehaviour
{
    //json
    private JsonData itemData;
    private String jsonText;
    //variables de usuario
    public InputField nombre;
    public int monedas;
    public double tiempo;

    public void Guardar()
    {
        DatosUsuario settings = new DatosUsuario(nombre.text, monedas, tiempo);

        itemData = JsonMapper.ToJson(settings);
        File.WriteAllText(Application.dataPath + "/Datos.json", itemData.ToString());

        Debug.Log(JsonUtility.ToJson(settings));

        SqlServer prueba = new SqlServer();
        prueba.InsertarDatos(nombre.text, monedas, tiempo);

    }


    [Serializable]
    public class DatosUsuario
    {
        public string nombre;
        public int monedas;
        public double tiempo;

       public DatosUsuario(string nombre, int monedas, double tiempo)
        {
            this.nombre = nombre;
            this.monedas = monedas;
            this.tiempo = tiempo;
        }
    }
}
