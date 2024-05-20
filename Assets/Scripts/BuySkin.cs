using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BuySkin : MonoBehaviour
{
    public Button btnCompra1;
    public int skinId = 1;
    public Text buttonText;
    public GameObject tiendapanel;


    private void Start()
    {
        btnCompra1.onClick.AddListener(OnBuyButtonClicked);
    }

    //private void Update()
    //{
    //    if (tiendapanel.activeSelf)
    //    {
    //        CheckIfSkinPurchased();
    //    }
    //    else
    //    {
    //        Debug.LogError("tiendaPanel no ha sido asignado en el Inspector.");
    //    }
    //}

    private void OnBuyButtonClicked()
    {
        StartCoroutine(Comprar());
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
            SkinSearchResponse apiResponse = JsonUtility.FromJson<SkinSearchResponse>(responseText);
            
        }
    }

    private IEnumerator Comprar()
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
            // Aquí puedes manejar la respuesta de la API
        }
    }

    [System.Serializable]
    public class SkinSearchResponse
    {
        public string jsonrpc;
        public int id;
        public Skin[] result;
    }

    [System.Serializable]
    public class Skin
    {
        public int skin_id;
        // Otros campos según sea necesario
    }
}
