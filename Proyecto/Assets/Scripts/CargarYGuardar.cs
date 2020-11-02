using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class CargarYGuardar : MonoBehaviour
{
    private String rutaArchivo;






    #region jugador
    [System.Serializable]
    class Jugador
    {
        public string Nombre;
        public int monedas;

        public Jugador()
        {
            this.Nombre = "";
            this.monedas = 0;
        }

    }
    #endregion
}