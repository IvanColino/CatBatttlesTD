using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static BuySkin;

public class BuySkin : MonoBehaviour
{
    public Button btnCompra1;
    public Button btnCompra2;
    public Button btnVolverMenu;
    public int skinId1 = 1;
    public int skinId2 = 2;
    public Text buttonText1;
    public Text buttonText2;
    public GameObject tiendapanel;
    public GameObject aplicacionPrincipalPanel;


    private void Start()
    {
        btnCompra1.onClick.AddListener(() => OnBuyButtonClicked(skinId1, buttonText1));
        btnCompra2.onClick.AddListener(() => OnBuyButtonClicked(skinId2, buttonText2));
        btnCompra1.onClick.AddListener(() => OnBuyButtonClicked());
        btnCompra2.onClick.AddListener(() => OnBuyButtonClicked2());
        
        btnVolverMenu.onClick.AddListener(VolverAlMenuPrincipal);
    }

    private void OnBuyButtonClicked(int skinId, Text buttonText)
    {
        if (buttonText.text == "Equip")
        {
            PlayerPrefs.SetInt("SkinId", skinId);
            buttonText.text = "Equipped";
            Debug.Log("Skin Equipada: " + PlayerPrefs.GetInt("SkinId"));
        }
        else { 
        StartCoroutine(Comprar(skinId, buttonText));
        }
    }
    private void OnBuyButtonClicked()
    {
        if (buttonText1.text == "Equipped")
        {
            buttonText2.text = "Equip";
        }
        
    }private void OnBuyButtonClicked2()
    {
        if (buttonText2.text == "Equipped")
        {
            buttonText1.text = "Equip";
        }
        
    }
    

    private void VolverAlMenuPrincipal()
    {
        tiendapanel.SetActive(false);
        aplicacionPrincipalPanel.SetActive(true);
    }

    public void CheckIfSkinPurchased()
    {
        StartCoroutine(CheckSkin());
    }

    private IEnumerator CheckSkin()
    {
        int userId = PlayerPrefs.GetInt("UserID", -1);
        if (userId == -1)
        {
            Debug.LogError("UserID no encontrado. El usuario no está autenticado.");
            yield break;
        }

        string url = "https://catbattle.duckdns.org/odoo-api/skin/search";
        string json = "{\"params\": {\"player_id\": " + userId + ", \"keys\": {\"limit\": 10, \"order\": \"name ASC\"}, \"db\": \"CatsBattles\", \"login\": \"admin\", \"password\": \"Almi123\"}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);
            if (!string.IsNullOrEmpty(responseText))
            {
                SkinSearchResponse apiResponse = JsonUtility.FromJson<SkinSearchResponse>(responseText);
                if (apiResponse != null && apiResponse.result != null && apiResponse.result.skins != null)
                {
                    Debug.Log("JSON deserializado correctamente. Número de skins: " + apiResponse.result.skins.Count);
                    foreach (var skin in apiResponse.result.skins)
                    {
                        if (skin != null && skin.id == skinId1)
                        {
                            btnCompra1.interactable = true; 
                            buttonText1.text = "Equip";
                        }
                        if (skin != null && skin.id == skinId2)
                        {
                            btnCompra2.interactable = true;
                            buttonText2.text = "Equip";
                        }
                    }
                }
                else
                {
                    Debug.LogError("Error deserializando el JSON o la lista skins es nula.");
                }
            }
            else
            {
                Debug.LogError("La respuesta JSON está vacía.");
            }


        }
    }

    private IEnumerator Comprar(int skinId, Text buttonText)
    {
        int userId = PlayerPrefs.GetInt("UserID", -1);
        
        if (userId == -1)
        {
            Debug.LogError("UserID no encontrado. El usuario no está autenticado.");
            yield break;
        }

  
        string url = "https://catbattle.duckdns.org/odoo-api/skin/buy";
        string json = "{\"params\": {\"user_id\": " + userId + ", \"skin_id\": " + skinId + ", \"db\": \"CatsBattles\", \"login\": \"admin\", \"password\": \"Almi123\"}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Compra realizada exitosamente: " + request.downloadHandler.text);
            buttonText.text = "Equip";
        }
    }



    [System.Serializable]
    public class SkinSearchResponse
    {
        public string jsonrpc;
        public int? id; 
        public Result result;
    }

    [System.Serializable]
    public class Result
    {
        public List<Skin> skins;
    }

    [System.Serializable]
    public class Skin
    {
        public int id;
        public string name;
    }
}
