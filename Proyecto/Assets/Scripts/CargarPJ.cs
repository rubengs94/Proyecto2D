using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CargarPJ : MonoBehaviour
{
    private string guid;
    string jsonText;
    private JsonData itemData;
    private string rutaPath;
    public GameObject[] personajes;

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
        if (File.Exists(rutaPath))
        {
            jsonText = File.ReadAllText(rutaPath);
            itemData = JsonMapper.ToObject(jsonText);
            guid = itemData["guid"].ToString();

            SqlServer sql = new SqlServer();

            int skin = sql.CargarSkin(guid);
            personajes[skin].SetActive(true);
        }
    }

}
