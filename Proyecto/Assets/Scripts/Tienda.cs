using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Tienda : MonoBehaviour
{

    SqlServer sql;
    private JsonData itemData;
    private string jsonText;
    private string guid;
    private string rutaPath;
    public Button [] botones;
    public Text[] textos;
    private List<int> SkinsJugador;
    public Text error;

    // Start is called before the first frame update
    void Start()
    {
        Cargar();   
    }

    private void Awake()
    {
        rutaPath = Application.dataPath + "/Guid.json";
    }


    private void Cargar()
    {

        sql = new SqlServer();

        if (File.Exists(rutaPath))
        {
            jsonText = File.ReadAllText(rutaPath);
            itemData = JsonMapper.ToObject(jsonText);
            guid = itemData["guid"].ToString();

            sql.CargarDatosUsuario(guid);

            SkinsJugador = sql.SkinsCompradas(guid);
            foreach (int idSkin in SkinsJugador)
            {
                botones[idSkin].interactable = true;
                textos[idSkin].text = "Equipar";
            }


            for (int i = 0; i < botones.Length; i++)
                if(sql.SkinCargada == i)
                {
                    botones[i].interactable = false;
                    textos[i].text = "Equipada";
                }
        }
    }


    /// <summary>
    /// Equipar la skin desde la tienda
    /// </summary>
    /// <param name="idSkin"></param>
    public void Equipar_Comprar(int idSkin)
    {

        error.text = "";
        if (textos[idSkin].text != "Equipar" && textos[idSkin].text != "Equipada")
        {
            if (sql.MonedasCargadas >= idSkin * 100)
                sql.Comprar(guid, idSkin, idSkin * 100);
            else
                error.text = "No tienes suficientes monedas";
        }
        else
            sql.Equipar(guid, idSkin);

        Cargar();
    }

    

}
