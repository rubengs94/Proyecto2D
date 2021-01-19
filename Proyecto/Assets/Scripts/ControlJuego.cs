using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LitJson;
using System.IO;
using UnityEngine.SceneManagement;

public class ControlJuego : MonoBehaviour
{
    #region PROPIEDADES

    private JsonData itemData;
    private string jsonText;
    public new AudioSource audio;
    public Sprite imagenAudioOn;
    public Sprite imagenAudioOff;
    public Button BotonAudio;
    public Image imagen;
    public GameObject pantallaCarga;
    private string juego;

    #endregion

    void Start()
    {
        pantallaCarga.SetActive(false);
        imagen.enabled = false;
        imagen.color = new Color32(41,112,224,0);
    }


    #region CambiarNivel

    /// <summary>
    /// pantalla negra para cargar
    /// </summary>
    /// <param name="escena"></param>
    public void PantallaCarga(string escena)
    {
        juego = escena;
        imagen.enabled = true;
        imagen.color = new Color32(41, 112, 224, 255);
        pantallaCarga.SetActive(true);
        Time.timeScale = 1f;
        Invoke("CargarEscena", 3f);
    }
    
    /// <summary>
    /// CArga de la escena
    /// </summary>
    private void CargarEscena()
    {
        SceneManager.LoadScene(juego);   
    }

    public void Exit()
    {
        Application.Quit();
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