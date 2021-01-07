using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPuntuacion : MonoBehaviour
{
    #region PROPIEDADES

    SqlServer sql;
    private int monedasCargadas;
    public static int Monedas { get; set; }
    public double Tiempo { get; set; }
    [Tooltip("Tiempo iniciar en Segundos")]
    public int tiempoinicial;
    [Tooltip("Escala del Tiempo del Reloj")]
    [Range(-10.0f, 10.0f)]
    public float escalaDeTiempo = 1;
    private Text Texto;
    private float TiempoFrameConTiempoScale = 0f;
    private float tiempoMostrarEnSegundos = 0F;
    private float escalaDeTiempoInicial;
    private bool EstaPausado = false;

    #endregion

    #region CARGAR Y GUARDAR

    /// <summary>
    /// Cargamos las monedas de la base de datos
    /// </summary>
    private void CargarMonedas()
    {
        sql = new SqlServer();
        monedasCargadas = sql.MonedasCargadas;
    }

    public void guardarDatos()
    {

        sql = new SqlServer();
        string [] separar = Texto.text.Split(':');
        float minuto = float.Parse(separar[0]);
        float segundo = float.Parse(separar[1]);

        sql.ActualizarDatos(Monedas, minuto, segundo);
    }


    #endregion


    #region TIEMPO


    void Start()
    {
        //cargamos las monedas
        CargarMonedas();

        Texto = GetComponent<Text>();
        tiempoMostrarEnSegundos = tiempoinicial;

        ActualizarReloj(tiempoinicial);
    }

    void Update()
    {
        TiempoFrameConTiempoScale = Time.deltaTime * escalaDeTiempo;
        tiempoMostrarEnSegundos += TiempoFrameConTiempoScale;
        ActualizarReloj(tiempoMostrarEnSegundos);
    }

    /// <summary>
    /// Actualiza el reloj de la pantalla
    /// </summary>
    /// <param name="tiempoEnSegundos"></param>
    public void ActualizarReloj(float tiempoEnSegundos)
    {
        int minutos = 0;
        int segundos = 0;
        // int milisegundos = 0;
        string textoDelReloj;

        if (tiempoEnSegundos < 0) tiempoEnSegundos = 0;

        minutos = (int)tiempoEnSegundos / 60;
        segundos = (int)tiempoEnSegundos % 60;
        //milisegundos = (int)tiempoEnSegundos / 1000;

        textoDelReloj = minutos.ToString("00") + ":" + segundos.ToString("00"); //+ ":" + milisegundos.ToString("00");
        Texto.text = textoDelReloj;
    }

    /// <summary>
    /// Pausar o reanudar el tiempo
    /// </summary>
    public void Pausar()
    {
        if (!EstaPausado)
        {
            EstaPausado = true;
            escalaDeTiempo = 0;
        }
        else
        {
            EstaPausado = false;
            escalaDeTiempo = 1;
        }
    }

    #endregion

}
