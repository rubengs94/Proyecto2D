using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ControlJuego : MonoBehaviour
{

    public AudioSource audio;
    public Sprite imagenAudioOn;
    public Sprite imagenAudioOff;
    public Button BotonAudio;


    #region CambiarNivel
    public void CambiarNivel (int nivel)
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


}