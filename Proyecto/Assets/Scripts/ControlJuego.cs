using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;


public class ControlJuego : MonoBehaviour, IPointerClickHandler
{
    #region PROPIEDADES
    public AudioSource audio;
    public Sprite imagenAudioOn;
    public Sprite imagenAudioOff;
    public Button BotonAudio;
    public Escenas cargarEscena;
    public enum Escenas
    {
        MenuPrincipal,
        Nivel1,
        Nivel2
    }
    public int monedas = 0;
    public double tiempo = 0;
    #endregion

    #region CambiarNivel
    /*
    public void CambiarNivel (int nivel)
    {
            switch (nivel)
            {
                case -1: Application.Quit(); break;
                case 0: SceneManager.LoadScene("MenuPrincipal"); break;
                case 1: SceneManager.LoadScene("Nivel1"); break;
            }
    }//cambiarnivel
    */

    public void OnPointerClick(PointerEventData eventData)
    {
        PantallaDeCarga.Instancia.CargarEscena(cargarEscena.ToString());
    }
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

    #region CARGARYGUARDAR

    CargarYGuardar cargaryguardar = null;

    public void Guardar()
    {
        cargaryguardar = new CargarYGuardar();
        cargaryguardar.Guardar(monedas,tiempo);
    }

    public void Cargar()
    {

    }
    #endregion
}