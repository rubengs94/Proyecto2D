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
    private string jsonText;
    //variables de usuario
    private string guid;
    public InputField nombre;
    //private int monedas;
    //private double tiempo;
    private string rutaPath;
    private string respuesta;

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
        try
        {
            sql = new SqlServer();

            if (!File.Exists(rutaPath))
            {
                guid = sql.GenerarGuid();

                GuardarGuid guidJson = new GuardarGuid(guid);
                itemData = JsonMapper.ToJson(guidJson);
                //Cambiar ruta datapath por persistentdatapath
                File.WriteAllText(rutaPath, itemData.ToString());

                sql.InsertarDatos(guid, nombre.text, 0, 0.0);

                Debug.Log("Json creado correctamente");
            }
            else
            {
                ///lectura de datos del archivo Json
                jsonText = File.ReadAllText(rutaPath);
                itemData = JsonMapper.ToObject(jsonText);

                guid = itemData[0]["guid"].ToString();

                ///sistema de monedas y tiempo en controlpuntuacion
                sql.CargarDatosUsuario(guid);

               
            }
        }
        catch(Exception ex)
        {
            sql.Publicar(SqlServer.Errores.ErrorAlGuardar.ToString()+ex.ToString(), "Excepciones");
        }
    }//Guardar()


    /// <summary>
    /// Cargar datos del usuario
    /// </summary>
    public void Cargar()
    {

    }//Cargar()


    /// <summary>
    /// Actualizar monedas y tiempo del usuario
    /// </summary>
    public void actualizarDatos()
    {
        ///cargamos las monedas y el tiempo de
        ///controlpuntuacion y los guardamos

        sql = new SqlServer();

        



    }


    /// <summary>
    /// Metodo para eliminar datos del usuario
    /// </summary>
    public void EliminarDatos()
    {
        sql = new SqlServer();

        if (File.Exists(rutaPath))
        {
            sql.EliminarDatos();
        }
        else
        {
            
        }
    }//EliminarDatos()




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
        public string guid;

        public GuardarGuid(string guid)
        {
            this.guid = guid;
        }
    }

    #endregion
}
