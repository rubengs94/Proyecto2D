using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using UnityEngine;

public class SqlServer : MonoBehaviour
{

    #region PROPIEDADES
    String cadenaConexion = "Server=localhost;Database=servidorjuego;Uid=root;Pwd=";
    //private string  server,port,user,password,bd;
    MySqlConnection conexion = null;
    MySqlCommand cmd;
    MySqlDataAdapter adaptador;
    DataSet ds;
    DataTable dt;
    DataRow dr;
    string query;
    #endregion


    /// <summary>
    /// Constructor parametrizado
    /// </summary>
    public SqlServer()
    {

    }


    #region MANEJO DE DATOS

    /// <summary>
    /// Obtener informacion del usuario
    /// </summary>
    public void ObtenerUsuario()
    {
    }

    /// <summary>
    /// Insertart datos del usuario
    /// </summary>
    /// <param name="guid">Clave principal del usuario</param>
    /// <param name="nombre">Nombre del usuario</param>
    /// <param name="monedas">Monedas en la cuenta</param>
    /// <param name="tiempo">Tiempo record del juego</param>
    public void InsertarDatos(string nombre, int monedas, double tiempo)
    {
        try
        {
            query = "INSERT INTO datosjugador(Guid, Nombre, Monedas, Tiempo)VALUES('"+GenerarGuid()+"','"+nombre+"',"+monedas+","+tiempo+");";

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

    }

    /// <summary>
    /// Actualizar datos del usuario
    /// </summary>
    /// <param name="nombre"></param>
    /// <param name="monedas"></param>
    /// <param name="tiempo"></param>
    public void ActualizarDatos(string nombre, int monedas, string tiempo)
    {

    }

    /// <summary>
    /// Eliminar datos del usuario
    /// </summary>
    /// <param name="guid"></param>
    public void EliminarDatos(Guid guid)
    {

    }

    #endregion

    #region GUID

    /// <summary>
    /// Generamos un Guid de tipo String
    /// </summary>
    /// <returns>Devuelve un String de tamaño 13</returns>
    public String GenerarGuid()
    {
        string nuevoGuid = Guid.NewGuid().ToString().Substring(0,13);
        try
        {
            if (!ComprobarGuid(nuevoGuid))
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
   

    public bool ComprobarGuid(String guid)
    {
        query = "SELECT Guid FROM datosjugador WHERE Guid like '" + guid + "'";

        try
        {
            conexion = new MySqlConnection(cadenaConexion);
            cmd = new MySqlCommand(query, conexion);

            conexion.Open();
            adaptador = new MySqlDataAdapter(query, conexion);
            dt = new DataTable();

            adaptador.Fill(dt);

            if (dt.Rows.Count > 0)
            {
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
