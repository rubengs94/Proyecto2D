using MySql.Data.MySqlClient;
using System;
using LitJson;
using System.Data;
using System.Data.Common;
using UnityEngine;
using System.IO;

public class SqlServer : MonoBehaviour
{

    #region PROPIEDADES

    String cadenaConexion = "Server=localhost;Database=servidorjuego;Uid=root;Pwd=";
    MySqlConnection conexion = null;
    MySqlCommand cmd;
    MySqlDataAdapter adaptador;
    DataSet ds;
    DataTable dt;
    DataRow dr;
    private string rutaPath;
    string query;
    string respuesta;
    string jsonText;
    private JsonData itemData;
    private string guidCargado;
    private string nombreCargado;
    private int monedasCargadas;
    private double tiempoCargado;

    #endregion


    /// <summary>
    /// Constructor vacio
    /// </summary>
    public SqlServer()
    {

    }

    private void Awake()
    {
        rutaPath = Application.dataPath + "/Guid.json";
    }

    #region MANEJO DE DATOS

    /// <summary>
    /// Obtener informacion del usuario
    /// </summary>
    public void CargarDatosUsuario(string guid)
    {

        try
        {
            query = "SELECT * FROM datosjugador WHERE Guid like '"+ guid +"';";
            cmd = new MySqlCommand(query, conexion);
            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();

            adaptador = new MySqlDataAdapter(query, conexion);
            dt = new DataTable();
            adaptador.Fill(dt);

            guidCargado = dt.Rows[0]["Guid"].ToString();
            nombreCargado = dt.Rows[0]["Nombre"].ToString();
            monedasCargadas = int.Parse(dt.Rows[0]["Monedas"].ToString());
            tiempoCargado = Double.Parse(dt.Rows[0]["Tiempo"].ToString());

            Debug.Log("Se han cargado los datos con exito");
            conexion.Close();

        }
        catch (MySqlException ex)
        {
            Debug.Log("Error al cargar los datos: " + ex);
            cmd.Cancel();
            conexion.Close();
        }
    }//CargarDatosUsuario();

    /// <summary>
    /// Insertart datos del usuario
    /// </summary>
    /// <param name="guid">Clave principal del usuario</param>
    /// <param name="nombre">Nombre del usuario</param>
    /// <param name="monedas">Monedas en la cuenta</param>
    /// <param name="tiempo">Tiempo record del juego</param>
    public void InsertarDatos(string guid, string nombre, int monedas, double tiempo)
    {
        try
        {
            query = "INSERT INTO datosjugador(Guid, Nombre, Monedas, Tiempo)VALUES('"+guid+"','"+nombre+"',"+monedas+","+tiempo+");";

            conexion = new MySqlConnection(cadenaConexion);

            cmd = new MySqlCommand(query, conexion);

            conexion.Open();
            cmd.ExecuteNonQuery();
            Debug.Log("Se han insertado los datos con exito");
            conexion.Close();
            
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error al insertar datos: "+ex);
            cmd.Cancel();
            conexion.Close();
        }
    }//InsertarDatos();

    /// <summary>
    /// Actualizar datos del usuario
    /// </summary>
    /// <param name="monedas"></param>
    /// <param name="tiempo"></param>
    public void ActualizarDatos(int monedas, string tiempo)
    {
        jsonText = File.ReadAllText(rutaPath);
        itemData = JsonMapper.ToObject(jsonText);
        string guid = itemData[0]["guid"].ToString();

        try
        {
            //obtenemos el tiempo para comprobar si lo ha superado
            CargarDatosUsuario(guid);

            //Si el tiempo nuevo es mayor que en la base de datos
            //guardamos el tiempo y actualizamos
            //Si es menor, actualizamos solo las monedas
            if (Double.Parse(tiempo) > Double.Parse(respuesta))
            {
                query = "UPDATE datosjugador" +
                    " SET Monedas = " + monedas +
                    ",Tiempo = " + tiempo + 
                    " WHERE Guid like '"+ guid +"';";

                cmd = new MySqlCommand(query, conexion);

                conexion.Open();
                cmd.ExecuteNonQuery();
                Debug.Log("Se han actualizado los datos(1) con exito");
                conexion.Close();
            }
            else
            {
                query = "UPDATE datosjugador" +
                    " SET Monedas = " + monedas +
                    " WHERE Guid like '" + guid + "';";

                cmd = new MySqlCommand(query, conexion);

                conexion.Open();
                cmd.ExecuteNonQuery();
                Debug.Log("Se han actualizado los datos(2) con exito");
                conexion.Close();
            }
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error al actualizar datos: "+ex);
            cmd.Cancel();
            conexion.Close();
        }
    }//ActualizarDatos();

    /// <summary>
    /// Eliminar datos del usuario
    /// </summary>
    /// <param name="guid"></param>
    public void EliminarDatos(string GuidText)
    {
        try
        {
            CargarDatosUsuario(GuidText);

            if (guidCargado.Equals(GuidText) && ComprobarGuid(GuidText, out respuesta))
            {
                query = "DELETE FROM datosjugador WHERE Guid like '" + guidCargado + "';";

                conexion = new MySqlConnection(cadenaConexion);

                cmd = new MySqlCommand(query, conexion);

                conexion.Open();
                cmd.ExecuteNonQuery();
                Debug.Log("Se han borrado los datos con exito");
                conexion.Close();
            }
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error al borrar datos: "+ex);
            cmd.Cancel();
            conexion.Close();
        }
    }//EliminarDatos();

    #endregion

    #region GUID

    /// <summary>
    /// Generamos un Guid de tipo String
    /// </summary>
    /// <returns>Devuelve un String de tamaño 13</returns>
    public string GenerarGuid()
    {
        string nuevoGuid = Guid.NewGuid().ToString().Substring(0,13);
        try
        {
            if (!ComprobarGuid(nuevoGuid, out respuesta))
            {
                Debug.Log("Guid creado correctamente");
                return nuevoGuid;
            }
            else
            {
                GenerarGuid();
            }
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error al crear el Guid: " + ex);
        }

        return Guid.Empty.ToString(); ;
    }
   
    /// <summary>
    /// Comprobar si existe el guid en la base de datos
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public bool ComprobarGuid(string guid, out string respuesta)
    {
        respuesta = "";

        try
        {
            CargarDatosUsuario(guid);

            if (!String.IsNullOrEmpty(guidCargado))
            {
                respuesta = guidCargado;
                return true;
            }

            conexion.Close();

        }
        catch (MySqlException ex)
        {
            Debug.Log("Error al comprobar Guid: " + ex);
            cmd.Cancel();
            conexion.Close();
        }

        return false;
    }


    #endregion
}


#region COMENTARIOS
/*
 string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=test;";
// Tu consulta en SQL
string query = "SELECT * FROM user";

// Prepara la conexión
MySqlConnection databaseConnection = new MySqlConnection(connectionString);
MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
commandDatabase.CommandTimeout = 60;
MySqlDataReader reader;

// A consultar !
try
{
    // Abre la base de datos
    databaseConnection.Open();

    // Ejecuta la consultas
    reader = commandDatabase.ExecuteReader();

    // Hasta el momento todo bien, es decir datos obtenidos

    // IMPORTANTE :#
    // Si tu consulta retorna un resultado, usa el siguiente proceso para obtener datos
    
    if (reader.HasRows)
    {
        while (reader.Read())
        {
            // En nuestra base de datos, el array contiene:  ID 0, FIRST_NAME 1,LAST_NAME 2, ADDRESS 3
            // Hacer algo con cada fila obtenida
            string[] row = { reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3) };
        }
    }
    else
    {
        Console.WriteLine("No se encontraron datos.");
    }

    // Cerrar la conexión
    databaseConnection.Close();
}
catch (Exception ex)
{
    // Mostrar cualquier excepción
    MessageBox.Show(ex.Message);
}
     */
#endregion
