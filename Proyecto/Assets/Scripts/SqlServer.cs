using LitJson;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using UnityEngine;

public class SqlServer : MonoBehaviour
{
    /// <summary>
    /// Constructor vacio
    /// </summary>
    public SqlServer(){}

    #region PROPIEDADES

    static readonly string cadenaConexion = "Server=localhost;Database=servidorjuego;Uid=root;Pwd=";
    static MySqlConnection conexion = new MySqlConnection(cadenaConexion);
    static readonly string rutaPath = Application.dataPath + "/Guid.json";
    MySqlCommand cmd;
    MySqlDataAdapter adaptador;
    DataTable dt;
    string query;
    string jsonText;
    private JsonData itemData;
    private string guidCargado;
    private string nombreCargado;
    private int monedasCargadas;
    private double tiempoCargado;

    #endregion


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

            if (dt.Rows.Count > 0)
            {
                guidCargado = dt.Rows[0]["Guid"].ToString();
                nombreCargado = dt.Rows[0]["Nombre"].ToString();
                monedasCargadas = int.Parse(dt.Rows[0]["Monedas"].ToString());
                tiempoCargado = Double.Parse(dt.Rows[0]["Tiempo"].ToString());
                Publicar("Carga de datos correcta", "Log");
            }

            conexion.Close();

        }
        catch (MySqlException ex)
        {
            Publicar(guidCargado+Errores.ErrorAlCargar.ToString()+ex.ToString(), "Excepciones");
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

            cmd = new MySqlCommand(query, conexion);

            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();

            Publicar("El usuario " + guid + " con nombre " + nombre + " se ha insertado correctamente", "Log");
            
        }
        catch (MySqlException ex)
        {
            Publicar(guid+Errores.ErrorAlInsertar.ToString()+ex.ToString(), "Excepciones");
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

        string guid = LeerGuidJson();

        try
        {
            //obtenemos el tiempo para comprobar si lo ha superado
            CargarDatosUsuario(guid);

            //Si el tiempo nuevo es mayor que en la base de datos
            //guardamos el tiempo y actualizamos
            //Si es menor, actualizamos solo las monedas
            if (Double.Parse(tiempo) > tiempoCargado)
            {
                query = "UPDATE datosjugador" +
                    " SET Monedas = " + monedas +
                    ",Tiempo = " + tiempo + 
                    " WHERE Guid like '"+ guid +"';";

                cmd = new MySqlCommand(query, conexion);

                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();

                Publicar("Los datos del usuario "+guid+" se han actualizado correctamente", "Log");
            }
            else
            {
                query = "UPDATE datosjugador" +
                    " SET Monedas = " + monedas +
                    " WHERE Guid like '" + guid + "';";

                cmd = new MySqlCommand(query, conexion);

                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();

                Publicar("Carga de datos correcta", "Log");
            }
        }
        catch (MySqlException ex)
        {
            Publicar(guidCargado+Errores.ErrorAlActualizar.ToString()+ex.ToString(), "Excepciones");
            cmd.Cancel();
            conexion.Close();
        }
    }//ActualizarDatos();

    /// <summary>
    /// Eliminar datos del usuario
    /// </summary>
    /// <param name="guid"></param>
    public void EliminarDatos()
    {
        string guidJson = LeerGuidJson();

        try
        {
            CargarDatosUsuario(guidJson);

            if (guidCargado.Equals(guidJson) && ComprobarGuid(guidJson))
            {
                query = "DELETE FROM datosjugador WHERE Guid like '" + guidCargado + "';";

                conexion = new MySqlConnection(cadenaConexion);

                cmd = new MySqlCommand(query, conexion);

                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();

                Publicar("Se han eliminado los datos del usuario "+guidCargado, "Log");
            }
        }
        catch (MySqlException ex)
        {
            Publicar(guidCargado+Errores.ErrorAlEliminar.ToString()+ex.ToString(), "Excepciones");
            cmd.Cancel();
            conexion.Close();
        }
    }//EliminarDatos();

    #endregion


    #region LOG/EXCEPCIONES

    public enum Errores
    {
        SinDeterminar,
        AliasEnUso,
        ErrorAlGuardar,
        ErrorAlInsertar,
        ErrorAlActualizar,
        ErrorAlCargar,
        ErrorAlEliminar,
        ErrorEnElGuid
    }

    /// <summary>
    /// Guardar log/excepciones en Base de Datos
    /// </summary>
    /// <param name="texto"></param>
    /// <param name="tipo"></param>
    public void Publicar(string texto, string tipo)
    {
        DateTime fecha = DateTime.Now;
        fecha.ToString("yyyy-MM-dd H:mm:ss");
        try
        {
            string query = "INSERT INTO " + tipo + "(ID,Texto, Fecha) VALUES(0,'" + texto + "','" + fecha + "');";

            cmd = new MySqlCommand(query, conexion);
            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();
            Debug.Log("Se han insertado los datos con exito");
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error al insertar el error: " + ex);
            cmd.Cancel();
            conexion.Close();
        }

    }

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
            if (!ComprobarGuid(nuevoGuid))
            {
                Publicar("Guid generado correctamente", "Log");
                return nuevoGuid;
            }
            else
            {
                GenerarGuid();
            }
        }
        catch (MySqlException ex)
        {
            Publicar(ex.ToString(), "Excepciones");
        }

        return Guid.Empty.ToString(); ;
    }
   
    /// <summary>
    /// Comprobar si existe el guid en la base de datos
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public bool ComprobarGuid(string guid)
    {

        try
        {
            CargarDatosUsuario(guid);

            if (!String.IsNullOrEmpty(guidCargado))
            {
                return true;
            }

            conexion.Close();

        }
        catch (MySqlException ex)
        {
            Publicar(ex.ToString(), "Excepciones");
            cmd.Cancel();
            conexion.Close();
        }

        return false;
    }

    /// <summary>
    /// Devolvemos el guid del Json
    /// </summary>
    /// <returns></returns>
    public string LeerGuidJson()
    {
        jsonText = File.ReadAllText(rutaPath);
        itemData = JsonMapper.ToObject(jsonText);
        string guid = itemData[0]["guid"].ToString();

        return guid;
    }

    #endregion
}