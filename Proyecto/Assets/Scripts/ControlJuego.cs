using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ControlJuego : MonoBehaviour
{
    static public int monedas = 0;
    public Button[] botonesMenu;
    static public Text nombre;
    public AudioSource audio;
    public Sprite imagenAudioOn;
    public Sprite imagenAudioOff;
    public Button BotonAudio;


    #region NivelesYEscenas
    public void CambiarNivel(int nivel)
    {
        switch (nivel)
        {
            case -1: Application.Quit(); break;
            case 0: SceneManager.LoadScene("MenuPrincipal"); break;
            case 1: SceneManager.LoadScene("SeleccionarNivel"); break;
            case 2: SceneManager.LoadScene("Nivel1"); break;
            case 3: SceneManager.LoadScene("Nivel2"); break;
            case 4: SceneManager.LoadScene("Nivel3"); break;
            case 5: SceneManager.LoadScene("Nivel4"); break;
            case 6: SceneManager.LoadScene("Nivel5"); break;
            case 7: SceneManager.LoadScene("Nivel6"); break;
        }
    }//cambiarnivel

    #endregion

    #region Musica

    private bool On = false;

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
    /*
        #region CargarYGuardar
        class CargarYGuardar
        {
            private string rutaArchivo;
            static bool primeraVez = true;
            ControlJuego controljuego;

            void Awake()
            {
                rutaArchivo = Application.persistentDataPath + "/datos.dat";
                if (primeraVez)
                {
                    Cargar();
                    primeraVez = false;
                }
            }//awake


            public void Guardar()
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(rutaArchivo);
                Jugador datos = new Jugador(ControlJuego.nombre, ControlJuego.monedas);
                bf.Serialize(file, datos);
                bf.Serialize(file, datos);
                file.Close();
            }//guardar

            public void Cargar()
            {
                if (File.Exists(rutaArchivo))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(rutaArchivo, FileMode.Open);
                    Jugador datos = (Jugador)bf.Deserialize(file);
                    Jugador.nombre = nombre.ToString();
                    Jugador.monedas = monedas;
                }
                else
                    ControlJuego.monedas = 0;
            }//cargar

            }//class CargarYGuardar

        #endregion

        #region DatosJugador
        [System.Serializable]
        class Jugador
        {
            static public string nombre;
            static public int monedas;

            public Jugador(string nombre, int monedas)
            {
                this.nombre = nombre;
                this.monedas = monedas;
            }

        }//Class Jugador
        #endregion
    */
}