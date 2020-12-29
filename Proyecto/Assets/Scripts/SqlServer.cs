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

    //static readonly string cadenaConexion = "Server=localhost;Database=servidorjuego;Uid=root;Pwd=";
    static readonly string cadenaConexion = "Server=sql144.main-hosting.eu;Port=3306;Database=u716344861_servidorjuego;Uid=u716344861_servidorunity;Pwd=Rubencillo94";
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
    public float MinutosCargados { get; set; }
    public float SegundosCargados { get; set; }

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
                MinutosCargados = float.Parse(dt.Rows[0]["Minutos"].ToString());
                SegundosCargados = float.Parse(dt.Rows[0]["Segundos"].ToString());
                Publicar(Codes.UsuarioCargado.ToString() + "///"+GuidCargado, "log");
            }

            conexion.Close();

        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(GuidCargado+Codes.ErrorAlCargar.ToString()+ex.ToString(), "excepcion");
        }
    }//CargarDatosUsuario();

    /// <summary>
    /// Insertart datos del usuario
    /// </summary>
    /// <param name="guid">Clave principal del usuario</param>
    /// <param name="nombre">Nombre del usuario</param>
    /// <param name="monedas">Monedas en la cuenta</param>
    /// <param name="tiempo">Tiempo record del juego</param>
    public void InsertarDatos(string guid, string nombre, int monedas, float Minutos, float Segundos)
    {

        try
        {
            query = "INSERT INTO datosjugador(Guid, Nombre, Monedas, Minutos, Segundos)VALUES('"+guid+"','"+nombre+"',"+monedas+","+Minutos+","+Segundos+");";

            cmd = new MySqlCommand(query, conexion);

            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();

            Publicar(Codes.UsuarioCreado.ToString() + "///" +guid+"///"+nombre, "log");
            
        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(guid+Codes.ErrorAlInsertar.ToString()+ex.ToString(), "excepcion");
        }
    }//InsertarDatos();

    /// <summary>
    /// Actualizar datos del usuario
    /// </summary>
    /// <param name="monedas"></param>
    /// <param name="tiempo"></param>
    public void ActualizarDatos(int monedas, float minutos, float segundos)
    {

        string guid = LeerGuidJson();

        try
        {
            //obtenemos el tiempo para comprobar si lo ha superado
            CargarDatosUsuario(guid);

            //Si el tiempo nuevo es mayor que en la base de datos
            //guardamos el tiempo y actualizamos
            //Si es menor, actualizamos solo las monedas
            if (minutos >= MinutosCargados)
            {
                if (segundos > SegundosCargados)
                {
                    query = "UPDATE datosjugador" +
                        " SET Monedas = " + monedas +
                        ",Minutos = " + minutos+
                        ",Segundos = " + segundos+
                        " WHERE Guid like '" + guid + "';";

                    cmd = new MySqlCommand(query, conexion);

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    conexion.Close();

                    Publicar(Codes.UsuarioActualizado.ToString() + "///" + guid, "log");
                }
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

                Publicar(Codes.UsuarioActualizado.ToString()+"///"+guid, "log");
            }
        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(GuidCargado+Codes.ErrorAlActualizar.ToString()+ex.ToString(), "excepcion");
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

                Publicar(Codes.UsuarioEliminado.ToString()+"///"+GuidCargado, "log");

                SceneManager.LoadScene("MenuPrincipal");
            }
        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(GuidCargado+Codes.ErrorAlEliminar.ToString()+ex.ToString(), "excepcion");
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
                Publicar("Guid generado correctamente", "log");
                return nuevoGuid;
            }
            else
            {
                GenerarGuid();
            }
        }
        catch (MySqlException ex)
        {
            Publicar(ex.ToString(), "excepcion");
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
            Publicar(ex.ToString(), "excepcion");
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