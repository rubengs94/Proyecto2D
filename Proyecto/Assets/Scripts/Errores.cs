using LitJson;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Errores : MonoBehaviour
{
    SqlServer sql;
    private string error;
    private DateTime fecha;

    public Errores(string error, DateTime fecha)
    {
        this.error = error;
        this.fecha = DateTime.UtcNow;
    }

    public void PublicarError()
    {
        sql = new SqlServer();

        try
        {

        }
        catch (MySqlException ex)
        {
            Debug.Log("Error al insertar el error: " + ex);
        }

    }

}
