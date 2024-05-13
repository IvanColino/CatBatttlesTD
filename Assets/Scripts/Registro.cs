using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Text;

public class Registro : MonoBehaviour
{
    [System.Serializable]
    public class RegisterRequest
    {
        public RegisterParams @params;

        public RegisterRequest(string model, string db, string adminLogin, string adminPassword, string name, string userLogin, string userPassword)
        {
            @params = new RegisterParams
            {
                model = model,
                db = db,
                login = adminLogin,
                password = adminPassword,
                vals = new RegisterValues
                {
                    name = name,
                    login = userLogin,
                    password = userPassword
                }
            };
        }
    }

    [System.Serializable]
    public class RegisterParams
    {
        public string model;
        public string db;
        public string login;
        public string password;
        public RegisterValues vals;
    }

    [System.Serializable]
    public class RegisterValues
    {
        public string name;
        public string login;
        public string password;
    }

    public TMP_InputField nameInputField;
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public GameObject registroPanel;
    public GameObject loginPanel;

    private string apiUrl = "https://catbattle.duckdns.org/odoo-api/object/create";

    public void AttemptRegister()
    {
        string name = nameInputField.text;
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        RegisterRequest requestObject = new RegisterRequest("res.users", "CatsBattles", "admin", "Almi123", name, username, password);
        StartCoroutine(RegisterCoroutine(requestObject));
    }

    IEnumerator RegisterCoroutine(RegisterRequest requestObject)
    {
        string json = JsonUtility.ToJson(requestObject);
        Debug.Log("Sending JSON: " + json);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error de conexión o protocolo: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            // Aquí puedes agregar el código para verificar la respuesta del servidor y actuar en consecuencia
            registroPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
    }
}






