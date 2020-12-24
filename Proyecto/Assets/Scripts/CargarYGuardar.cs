using LitJson;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CargarYGuardar : MonoBehaviour
{
    #region PROPIEDADES

    SqlServer sql;
    ControlPuntuacion controlpuntuacion;
    private JsonData itemData;
    private String jsonText;
    //variables de usuario
    private string guid;
    public InputField nombre;
    //private int monedas;
    //private double tiempo;
    private string rutaPath;

    #endregion

    private void Awake()
    {
        rutaPath = Application.dataPath + "/Guid.json";
    }
    
    /// <summary>
    /// Metodo para guardar datos de la partida del usuario
    /// </summary>
    public void Guardar()
    {

        SqlServer prueba = new SqlServer();

        if (!File.Exists(rutaPath))
        {
            //guardado en Json
            guid = prueba.GenerarGuid();

            GuardarGuid guidJson = new GuardarGuid(guid, nombre.text);
            itemData = JsonMapper.ToJson(guidJson);
            //Cambiar ruta datapath por persistentdatapath
            File.WriteAllText(rutaPath, itemData.ToString());
            
            Debug.Log(JsonUtility.ToJson(guidJson));
        }
        else
        {
            ///sistema de monedas y tiempo en controlpuntuacion

            ///lectura de datos del archivo Json
            jsonText = File.ReadAllText(rutaPath);
            itemData = JsonMapper.ToObject(jsonText);

            guid = itemData[0]["guid"].ToString();
            nombre.text = itemData[0]["nombre"].ToString();

            //guardado en BD
            //prueba.InsertarDatos(guid, nombre.text, monedas, tiempo);
        }
    }

    #region CLASESUSUARIO
    /// <summary>
    /// datos del usuario
    /// </summary>
    [Serializable]
    public class DatosUsuario
    {
        public string guid;
        public string nombre;
        public int monedas;
        public double tiempo;

       public DatosUsuario(string guid, string nombre, int monedas, double tiempo)
        {
            this.guid = guid;
            this.nombre = nombre;
            this.monedas = monedas;
            this.tiempo = tiempo;
        }
    }
    /// <summary>
    /// guid personal del usuario
    /// </summary>
    [Serializable]
    public class GuardarGuid
    {
        string guid;
        string nombre;

        public GuardarGuid(string guid,string nombre)
        {
            this.guid = guid;
            this.nombre = nombre;
        }
    }

    #endregion
}
