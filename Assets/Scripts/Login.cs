using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Text;

public class Login : MonoBehaviour
{
    // Definición de la clase para datos de usuario recibidos
    [System.Serializable]
    public class APIResponse
    {
        public string jsonrpc;
        public int? id;
        public Result result;
    }

    [System.Serializable]
    public class Result
    {
        public int user_id;
        public string login;
        public int monedas;
        public bool authentication;
    }

    // Definición de las clases para la estructura de la solicitud
    [System.Serializable]
    public class LoginRequest
    {
        public LoginParams @params;

        public LoginRequest(string model, string db, string adminLogin, string adminPassword, string userLogin, string userPassword)
        {
            @params = new LoginParams
            {
                model = model,
                db = db,
                login = adminLogin,
                password = adminPassword,
                vals = new LoginValues
                {
                    login = userLogin,
                    password = userPassword
                }
            };
        }
    }

    [System.Serializable]
    public class LoginParams
    {
        public string model;
        public string db;
        public string login;
        public string password;
        public LoginValues vals;
    }

    [System.Serializable]
    public class LoginValues
    {
        public string login;
        public string password;
    }

    // Campos de entrada para usuario y contraseña
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public GameObject loginpanel;
    public GameObject aplicacionPrincipalPanel;
    public GameObject BotonLogin;
    public GameObject BotonCerrarSesion;
    // URL de la API
    private const string apiUrl = "https://catbattle.duckdns.org/odoo-api/common/login";

    public void AttemptLogin()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        LoginRequest requestObject = new LoginRequest("res.users", "CatsBattles", "admin", "Almi123", username, password);
        StartCoroutine(LoginCoroutine(requestObject));
    }

    IEnumerator LoginCoroutine(LoginRequest requestObject)
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
            APIResponse apiResponse = JsonUtility.FromJson<APIResponse>(request.downloadHandler.text);
            if (apiResponse.result != null && apiResponse.result.authentication)
            {
                Debug.Log($"Login exitoso: Usuario ID {apiResponse.result.user_id}, Login {apiResponse.result.login}, Monedas {apiResponse.result.monedas}");
                PlayerPrefs.SetInt("UserID", apiResponse.result.user_id);
                PlayerPrefs.SetString("Username", apiResponse.result.login);
                PlayerPrefs.SetInt("Monedas", apiResponse.result.monedas);
                PlayerPrefs.Save();
                loginpanel.SetActive(false);
                BotonLogin.SetActive(false);
                usernameInputField.text = "";
                passwordInputField.text = "";
                aplicacionPrincipalPanel.SetActive(true);
                BotonCerrarSesion.SetActive(true);
                Debug.Log("UserID guardado: " + PlayerPrefs.GetInt("UserID"));
                Debug.Log("Username guardado: " + PlayerPrefs.GetString("Username"));
                Debug.Log("Monedas guardadas: " + PlayerPrefs.GetInt("Monedas"));
            }
            else
            {
                Debug.LogError("Autenticación fallida o respuesta inesperada del servidor.");
            }
        }
    }
}








