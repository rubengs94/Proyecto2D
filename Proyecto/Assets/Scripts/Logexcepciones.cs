using LitJson;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LogExcepciones : MonoBehaviour
{
    #region PROPIEDADES

    static string cadenaConexion = "Server=localhost;Database=servidorjuego;Uid=root;Pwd=";
    static MySqlConnection conexion = new MySqlConnection(cadenaConexion);
    MySqlCommand cmd;
    #endregion

    public void Publicar(string texto, DateTime fecha, string tipo)
    {

        try
        {
            string query = "INSERT INTO '" + tipo + "'(Texto, Fecha) VALUES('" + texto + "','" + fecha + "');";

            cmd = new MySqlCommand(query, conexion);

            conexion.Open();
            cmd.ExecuteNonQuery();
            Debug.Log("Se han insertado los datos con exito");
            conexion.Close();
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error al insertar el error: " + ex);
        }

    }

}
