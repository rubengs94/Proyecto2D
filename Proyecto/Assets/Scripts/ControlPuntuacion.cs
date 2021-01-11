using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPuntuacion : MonoBehaviour
{
    #region PROPIEDADES

    SqlServer sql;
    [Tooltip("Tiempo iniciar en Segundos")]
    public int tiempoinicial;
    [Tooltip("Escala del Tiempo del Reloj")]
    [Range(-10.0f, 10.0f)]
    private int monedas = 0;
    public Text textoMonedas;
    public float escalaDeTiempo = 1;
    private TextMesh Texto;
    private float TiempoFrameConTiempoScale = 0f;
    private float tiempoMostrarEnSegundos = 0F;
    private float escalaDeTiempoInicial;
    private bool EstaPausado = false;

    #endregion

    #region CARGAR Y GUARDAR

    public void guardarDatos()
    {

        sql = new SqlServer();
        string [] separar = Texto.text.Split(':');
        string minuto = separar[0];
        string segundo = separar[1];

        sql.ActualizarDatos(this.monedas, minuto, segundo);
    }


    #endregion


    #region TIEMPO


    void Start()
    {
        Texto = GameObject.Find("Reloj").GetComponent<TextMesh>();
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


    #region COINS

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "coins")
        {
            Destroy(collision.gameObject);
            monedas++;
            textoMonedas.text = "Monedas: " + monedas;
        }
    }
    #endregion


}
