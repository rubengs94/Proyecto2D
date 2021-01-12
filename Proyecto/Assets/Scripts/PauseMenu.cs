using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    #region PROPIEDADES

    public static bool juegoPausado = false;
    public GameObject pauseMenuUI;
    public GameObject ConfirmarUI;

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }

    }


    void Reanudar()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    /// <summary>
    /// Pausar la partida
    /// </summary>
    public void Pausar()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    /// <summary>
    /// Metodo para reaundar la partida desde boton
    /// </summary>
    public void Continuar()
    {
        Reanudar();
    }


    #region CONFIRMAR SALIDA

    /// <summary>
    /// Activa el panel de confirmacion
    /// </summary>
    public void BotonSalida()
    {
        pauseMenuUI.SetActive(false);
        ConfirmarUI.SetActive(true);
    }

    /// <summary>
    /// Cancela la salida y carga el panel de pausa
    /// </summary>
    public void No()
    {
        ConfirmarUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }



    #endregion
}
