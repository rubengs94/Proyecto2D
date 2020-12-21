using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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
    }
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
}