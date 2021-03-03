using LitJson;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SqlServer : MonoBehaviour
{

    #region PROPIEDADES

    //static readonly string cadenaConexion = "Server=localhost;Database=servidorjuego;Uid=root;Pwd=";
    static readonly string cadenaConexion = "Server=sql168.main-hosting.eu;Port=3306;Database=u195758784_Principal;Uid=u195758784_samu;Pwd=Samu1997";
    static MySqlConnection conexion = new MySqlConnection(cadenaConexion);
    static readonly string rutaPath = Application.dataPath + "/Guid.json";
    MySqlCommand cmd;
    MySqlDataAdapter adaptador;
    DataTable dt;
    string query;
    string jsonText;
    private JsonData itemData;
    public string GuidCargado;
    public string NombreCargado;
    public int MonedasCargadas;
    public string MinutosCargados;
    public string SegundosCargados;
    public int PartidasCargadas;
    public int BanCargado;

    #endregion


    #region MANEJO DE DATOS

    /// <summary>
    /// Obtener informacion del usuario
    /// </summary>
    public void CargarDatosUsuario(string guid)
    {
        try
        {
            query = "SELECT * FROM datosjugador WHERE Guid like '" + guid + "';";
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
                MinutosCargados = (dt.Rows[0]["Minutos"].ToString());
                SegundosCargados = (dt.Rows[0]["Segundos"].ToString());
                PartidasCargadas = int.Parse(dt.Rows[0]["Partidas"].ToString());//Fatalba cargar las partidas
				BanCargado = int.Parse(dt.Rows[0]["Baneado"].ToString());//He cambiado a int la propiedad
            }

            conexion.Close();

        }
        catch (Exception ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(GuidCargado + Codes.ErrorAlCargar.ToString() + ex.ToString(), "excepcion");
        }
    }//CargarDatosUsuario();



    /// <summary>
    /// Insertart datos del usuario
    /// </summary>
    /// <param name="guid">Clave principal del usuario</param>
    /// <param name="nombre">Nombre del usuario</param>
    /// <param name="monedas">Monedas en la cuenta</param>
    /// <param name="tiempo">Tiempo record del juego</param>
    public void InsertarDatos(string guid, string nombre, int monedas, string Minutos, string Segundos, int Partidas, int Ban)
    {

        try
        {

            cmd = new MySqlCommand("Player_Insert", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Guid", guid);
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@Monedas", monedas);
            cmd.Parameters.AddWithValue("@Minutos", Minutos);
            cmd.Parameters.AddWithValue("@Segundos", Segundos);
            cmd.Parameters.AddWithValue("@Partidas", Partidas);
            cmd.Parameters.AddWithValue("@Baneado", Ban);

            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();

            Publicar(Codes.UsuarioCreado.ToString() + "///" + guid + "///" + nombre, "log");

        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(guid + Codes.ErrorAlInsertar.ToString() + ex.ToString(), "excepcion");
        }
    }//InsertarDatos();

    /// <summary>
    /// Actualizar datos del usuario
    /// </summary>
    /// <param name="monedas"></param>
    /// <param name="tiempo"></param>
    public void ActualizarDatos(int monedas, string minutos, string segundos)
    {

        string guid = LeerGuidJson();

        try
        {
            //obtenemos el tiempo para comprobar si lo ha superado
            CargarDatosUsuario(guid);
            int monedaTotal = MonedasCargadas + monedas;
            cmd = new MySqlCommand("UpdatePlayer_Coins", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@guidUpdate", GuidCargado);
            cmd.Parameters.AddWithValue("@coins", monedaTotal);

            //Si el tiempo nuevo es mayor que en la base de datos
            //guardamos el tiempo y actualizamos
            //Si es menor, actualizamos solo las monedas
            if (float.Parse(minutos) >= float.Parse(MinutosCargados) &&
                float.Parse(segundos) >= float.Parse(SegundosCargados))
            {

                cmd.Parameters.AddWithValue("@minutes", minutos);
                cmd.Parameters.AddWithValue("@seconds", segundos);

            }
            else
            {

                cmd.Parameters.AddWithValue("@minutes", MinutosCargados);
                cmd.Parameters.AddWithValue("@seconds", SegundosCargados);

            }

            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();
            Publicar(Codes.UsuarioActualizado.ToString() + "///" + guid, "log");

        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(GuidCargado + Codes.ErrorAlActualizar.ToString() + ex.ToString(), "excepcion");
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
            if (ComprobarGuid(guidJson))
            {

                conexion = new MySqlConnection(cadenaConexion);
                cmd = new MySqlCommand("Player_Delete", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@guidBorrado", GuidCargado);

                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();

                File.Delete(rutaPath);
                File.Delete(rutaPath + ".meta");//Borrar al tener el .exe

                Publicar(Codes.UsuarioEliminado.ToString() + "///" + GuidCargado, "log");

            }
        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(GuidCargado + Codes.ErrorAlEliminar.ToString() + ex.ToString(), "excepcion");
        }
    }//EliminarDatos();

    #endregion


    #region LOG/EXCEPCIONES

    /// <summary>
    /// Coleccion de log/errores
    /// </summary>
    public enum Codes
    {
        SinDeterminar,
        ReportInsertado,
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
        ErrorEnElGuid,
        ErrorReport
    }

    /// <summary>
    /// Guardar log/excepciones en Base de Datos
    /// </summary>
    /// <param name="texto"></param>
    /// <param name="tipo"></param>
    public void Publicar(string texto, string tipo)
    {

        string cadenaExcepcion = "Server=sql168.main-hosting.eu;Port=3306;Database=u195758784_Secundaria; Uid=u195758784_samu2;Pwd=Samu1997";
        MySqlConnection conexionExcepcion = new MySqlConnection(cadenaExcepcion);
        DateTime fecha = DateTime.Now;
        fecha.ToString("yyyy-MM-dd H:mm:ss");

        try
        {
            if (tipo.Equals("log"))
            {

                cmd = new MySqlCommand("Insert_Log", conexionExcepcion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@text", texto);
                cmd.Parameters.AddWithValue("@date", fecha);

            }
            else
            {

                cmd = new MySqlCommand("Insert_Excepcion", conexionExcepcion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@text", texto);
                cmd.Parameters.AddWithValue("@date", fecha);

            }

            conexionExcepcion.Open();
            cmd.ExecuteNonQuery();
            conexionExcepcion.Close();
            Debug.Log(Codes.Log_Excepcion.ToString() + "///" + tipo);

        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Debug.Log("Error al insertar el log/excepcion: " + ex);
        }

    }

    #endregion


    #region SYSTEM REPORT


    /// <summary>
    /// Reporte en la base de datos
    /// </summary>
    /// <param name="tituloReport"></param>
    /// <param name="textoReport"></param>
    public void Report(string tituloReport, string textoReport)
    {

        string guid = LeerGuidJson();
        DateTime fecha = DateTime.Now;
        fecha.ToString("yyyy-MM-dd H:mm:ss");

        try
        {

            CargarDatosUsuario(guid);

            conexion = new MySqlConnection(cadenaConexion);
            cmd = new MySqlCommand("Insert_Report", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@guidReport", GuidCargado);
            cmd.Parameters.AddWithValue("@tituloReport", tituloReport);
            cmd.Parameters.AddWithValue("@textoReport", textoReport);
            cmd.Parameters.AddWithValue("@Fecha", fecha);

            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();

            Publicar(Codes.ReportInsertado.ToString() + "///" + GuidCargado, "log");



        }
        catch (MySqlException ex)
        {
            Publicar(Codes.ErrorReport + ex.ToString(), "excepcion");
        }

    }


    /// <summary>
    /// Envio de correo del usuario
    /// </summary>
    /// <param name="emailReport"></param>
    /// <param name="tituloReport"></param>
    /// <param name="textoReport"></param>
    public void Report(string emailReport, string tituloReport, string textoReport)
    {

        MailMessage message = new MailMessage();
        message.IsBodyHtml = true;
        message.Priority = MailPriority.Normal;
        message.From = new MailAddress("rubengonsi94@gmail.com", "Soporte Jumping Pixels");
        message.To.Add(new MailAddress("jumpingsupp@gmail.com"));
        message.Subject = tituloReport;
        message.Body = "<html><body><br>Correo: " + emailReport + "<br><br>" + textoReport + "</body></html>";

        SmtpClient smtp = new SmtpClient();
        smtp.Host = "smtp.gmail.com";
        smtp.Port = 25;
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential("rubengonsi94@gmail.com", "Rubencillo94");

        try
        {
            smtp.Send(message);
        }
        catch (SmtpException ex)
        {
            Report(tituloReport, textoReport);
            Publicar("Error al enviar reporte" + ex, "excepcion");
        }

    }

    #endregion

    #region BANEAR

    public void Banear(string GuidBan)
    {

        try
        {

            conexion = new MySqlConnection(cadenaConexion);
            cmd = new MySqlCommand("Ban_Player", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GuidBan", GuidBan);

            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();


        }
        catch (MySqlException ex)
        {
            cmd.Cancel();
            conexion.Close();
            Publicar(GuidCargado + Codes.ErrorAlEliminar.ToString() + ex.ToString(), "excepcion");
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

        string nuevoGuid = Guid.NewGuid().ToString().Substring(0, 13);
        try
        {
            if (!ComprobarGuid(nuevoGuid))
            {
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