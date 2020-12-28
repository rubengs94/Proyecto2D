using LitJson;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SqlServer : MonoBehaviour
{

    #region PROPIEDADES

    static readonly string cadenaConexion = "Server=localhost;Database=servidorjuego;Uid=root;Pwd=";
    static MySqlConnection conexion = new MySqlConnection(cadenaConexion);
    static readonly string rutaPath = Application.dataPath + "/Guid.json";
    MySqlCommand cmd;
    MySqlDataAdapter adaptador;
    DataTable dt;
    string query;
    string jsonText;
    CargarYGuardar cargaryguardar;
    private JsonData itemData;
    public string GuidCargado { get; set; }
    public string NombreCargado { get; set; }
    public int MonedasCargadas { get; set; }
    public double TiempoCargado { get; set; }

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
                GuidCargado = dt.Rows[0]["Guid"].ToString();
                NombreCargado = dt.Rows[0]["Nombre"].ToString();
                MonedasCargadas = int.Parse(dt.Rows[0]["Monedas"].ToString());
                TiempoCargado = Double.Parse(dt.Rows[0]["Tiempo"].ToString());
                Publicar(Codes.UsuarioCargado.ToString() + "///"+GuidCargado, "Log");
            }

            conexion.Close();

        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(GuidCargado+Codes.ErrorAlCargar.ToString()+ex.ToString(), "Excepcion");
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

            Publicar(Codes.UsuarioCreado.ToString() + "///" +guid+"///"+nombre, "Log");
            
        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(guid+Codes.ErrorAlInsertar.ToString()+ex.ToString(), "Excepcion");
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
            if (Double.Parse(tiempo) > TiempoCargado)
            {
                query = "UPDATE datosjugador" +
                    " SET Monedas = " + monedas +
                    ",Tiempo = " + tiempo + 
                    " WHERE Guid like '"+ guid +"';";

                cmd = new MySqlCommand(query, conexion);

                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();

                Publicar(Codes.UsuarioActualizado.ToString() + "///" +guid, "Log");
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

                Publicar(Codes.UsuarioActualizado.ToString()+"///"+guid, "Log");
            }
        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(GuidCargado+Codes.ErrorAlActualizar.ToString()+ex.ToString(), "Excepcion");
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

            if (GuidCargado.Equals(guidJson) && ComprobarGuid(guidJson))
            {

                File.Delete(rutaPath);
                File.Delete(rutaPath+".meta");

                query = "DELETE FROM datosjugador WHERE Guid like '" + GuidCargado + "';";

                conexion = new MySqlConnection(cadenaConexion);

                cmd = new MySqlCommand(query, conexion);

                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();

                Publicar(Codes.UsuarioEliminado.ToString()+"///"+GuidCargado, "Log");

                SceneManager.LoadScene("MenuPrincipal");
            }
        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(GuidCargado+Codes.ErrorAlEliminar.ToString()+ex.ToString(), "Excepcion");
        }
    }//EliminarDatos();

    #endregion


    #region LOG/EXCEPCIONES

    public enum Codes
    {
        SinDeterminar,
        Log_Excepcion,
        UsuarioCreado,
        UsuarioActualizado,
        UsuarioEliminado,
        UsuarioCargado,
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
            Debug.Log(Codes.Log_Excepcion.ToString() + "///" +tipo);
        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Debug.Log("Error al insertar el log/excepcion: " + ex);
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

            if (!String.IsNullOrEmpty(GuidCargado))
            {
                return true;
            }

            conexion.Close();

        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(ex.ToString(), "Excepciones");
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
        string guid = itemData["guid"].ToString();

        return guid;
    }

    #endregion
}