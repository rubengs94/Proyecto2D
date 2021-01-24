using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Report : MonoBehaviour
{

    #region PROPIEDADES

    public InputField emailReport;
    public InputField tituloReport;
    public InputField textoReport;
    SqlServer sql;

    #endregion

    #region ENVIO DE DATOS

    public void Enviar()
    {
        sql = new SqlServer();
        sql.Report(emailReport.text, tituloReport.text, textoReport.text);

        emailReport.text = "";
        tituloReport.text = "";
        textoReport.text = "";
    }


    #endregion


}
