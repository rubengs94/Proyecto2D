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
    public Text monedas;
    public Button Jugar;
    public Button eliminar;
	public Button reportar;
    //private int monedas;
    //private double tiempo;
    private string rutaPath;

    #endregion

    private void Start()
    {
        Cargar();
    }

    private void Awake()
    {
        rutaPath = Application.dataPath + "/Guid.json";
    }

    private void Update()
    {
        if (String.IsNullOrEmpty(nombre.text))
        {
            Jugar.enabled = false;
            Jugar.interactable = false;
        }
        else
        {
            Jugar.enabled = true;
            Jugar.interactable = true;
        }
    }

    #region MANEJO DE DATOS

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
                sql.InsertarDatos(guid, nombre.text, 0, "00","00");

                GuardarGuid guidJson = new GuardarGuid(guid);
                itemData = JsonMapper.ToJson(guidJson);
                //Cambiar ruta datapath por persistentdatapath
                File.WriteAllText(rutaPath, itemData.ToString());


                Debug.Log("Json creado correctamente");
            }
            
        }
        catch(Exception ex)
        {
            sql.Publicar(SqlServer.Codes.ErrorAlGuardar.ToString()+ex.ToString(), "Excepcion");
        }
    }//Guardar()


    /// <summary>
    /// Cargar datos del usuario
    /// </summary>
    public void Cargar()
    {
        sql = new SqlServer();

        if (File.Exists(rutaPath))
        {
            jsonText = File.ReadAllText(rutaPath);
            itemData = JsonMapper.ToObject(jsonText);
            string guid = itemData["guid"].ToString();

            sql.CargarDatosUsuario(guid);

            if (!String.IsNullOrEmpty(sql.NombreCargado))
            {
                reportar.enabled = true;
                reportar.interactable = true;
				eliminar.enabled = true;
                eliminar.interactable = true;
                nombre.interactable = false;
                nombre.text = sql.NombreCargado;
                monedas.text = "Monedas: "+sql.MonedasCargadas.ToString();
                monedas.GetComponent<Text>().enabled = true;
            }

        }
        else
        {
			reportar.enabled = false;
			reportar.interactable = false;
            eliminar.enabled = false;
            eliminar.interactable = false;
            monedas.GetComponent<Text>().enabled = false;
        }
    }//Cargar()


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

    }

    #endregion


    #region CLASESUSUARIO

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
