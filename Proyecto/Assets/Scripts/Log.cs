using MySql.Data.MySqlClient;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    SqlServer sql;
    private string log;
    private DateTime fechaLog;

    public Log(string log, DateTime fechaLog)
    {
        this.log = log;
        this.fechaLog = fechaLog;
    }

    public void PublicarLog()
    {

    }//PublicarLog()
}
