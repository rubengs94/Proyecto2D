using LitJson;
using System.IO;
using UnityEngine;
using System;

public class CargarYGuardar : MonoBehaviour
{

    public void Guardar(int monedas, double tiempo)
    {
        DatosUsuario settings = new DatosUsuario(monedas, tiempo);

        File.WriteAllText(Application.persistentDataPath + "/Datos.dat", JsonMapper.ToJson(settings).ToString());


    }
}

#region DATOSUSUARIO
[Serializable]
public class DatosUsuario
{
    public int monedas;
    public double tiempo;

    public DatosUsuario(int monedas, double tiempo)
    {
        this.monedas = monedas;
        this.tiempo = tiempo;
    }
}//DatosUsuario
#endregion