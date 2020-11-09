using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Diagnostics;

public class ControlJuego : MonoBehaviour
{
    public Button[] botonesMenu;
    public AudioSource audio;
    public Sprite imagenAudioOn;
    public Sprite imagenAudioOff;
    public Button BotonAudio;
    public int monedas;
    public Text cantidadMonedas;

    private string rutaArchivo;

    void Awake()
    {
        rutaArchivo = Application.persistentDataPath + "/data.dat";
    }

    void Start()
    {
        Guardar();
    }
    

    void Update()
    {
        if (cantidadMonedas!= null)
        {
            cantidadMonedas.text = "Monedas: "+monedas.ToString();
        }
    }

    #region SaveData
    public void Guardar()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(rutaArchivo);
        Jugador datos = new Jugador();
        datos.monedas = monedas;
        bf.Serialize(file, datos);
        file.Close();
        Cargar();
    }//guardar

    public void Cargar()
    {
        if (File.Exists(rutaArchivo))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(rutaArchivo, FileMode.Open);
            Jugador datos = (Jugador)bf.Deserialize(file);
            monedas = datos.monedas;
        }
        else
           monedas = 0;
    }//cargar

    #endregion





    #region NivelesYEscenas

    public void CambiarNivel(int nivel)
    {
        if (nivel == 0)
            SceneManager.LoadScene("Menú");
        else
            SceneManager.LoadScene("Nivel" + nivel);
    }

    public void Salir()
    {
        print("Saliste del juego");
        Application.Quit();
    }

    #endregion

    #region Musica

    private bool On = true;

    public void Reproducir()
    {
        if (On)
        {
            audio.Stop();
            On = false;
            BotonAudio.image.sprite = imagenAudioOff;
        }
        else
        {
            audio.Play();
            On = true;
            BotonAudio.image.sprite = imagenAudioOn;
        }
    }








    #endregion

    #region Jugador
    [Serializable]
    class Jugador
    {
        public int monedas;
    }
    #endregion
}