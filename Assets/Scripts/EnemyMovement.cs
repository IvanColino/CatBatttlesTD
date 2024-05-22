using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static System.Net.WebRequestMethods;

public class MoveEnemies : MonoBehaviourPun
{
    public Transform[] waypoints;
    public float speed;
    private int waypointIndex = 0;
    private GameObject[] waypointsObjects = new GameObject[12];
    public int playerHealth = 0;
    private TextMeshProUGUI text;
    private GameObject panelganJ2, panelperJ1,paneljuego1,paneljuego2;



    void Start()
    {
        paneljuego1 = GameObject.Find("PanelManager").GetComponent<panelMaangement>().paneljuego1;
        paneljuego2 = GameObject.Find("PanelManager").GetComponent<panelMaangement>().paneljuego2;
        panelganJ2 = GameObject.Find("PanelManager").GetComponent<panelMaangement>().panelganJ2;
        panelperJ1 = GameObject.Find("PanelManager").GetComponent<panelMaangement>().panelperJ1;
        text = GameObject.Find("Player" + 1 + "HP").GetComponent<TextMeshProUGUI>();
        if (!photonView.IsMine)
        {
            enabled = false;
        }

        for (int i = 0; i < 12; i++)
        {
            waypointsObjects[i] = GameObject.Find("Waypoint" + i);
           
        }

        waypoints = new Transform[waypointsObjects.Length];
        for (int i = 0; i < waypointsObjects.Length; i++)
        {
            waypoints[i] = waypointsObjects[i].transform;
            
            
        }
    }

    void Update()
    {
        MoveTowardsWaypoint();
        CheckIfLastWaypointReached();
     playerHealth = int.Parse(text.text);
    }

    void MoveTowardsWaypoint()
    {
        if (waypointIndex >= waypoints.Length) return;

        Transform targetWaypoint = waypoints[waypointIndex];
        float step = speed * Time.deltaTime;  // Calcula el paso basado en la velocidad constante
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

        // Verifica si el objeto ha alcanzado el waypoint usando un umbral pequeño para la proximidad
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            waypointIndex++;
        }
    }
    void CheckIfLastWaypointReached()
    {
        // Verificar si el enemigo ha llegado al último waypoint
        if (waypointIndex == waypoints.Length)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("DamagePlayer", RpcTarget.AllBuffered);
                PhotonNetwork.Destroy(gameObject);  // Destruir el enemigo en todos los clientes
            }
        }
    }

    [PunRPC]
    void DamagePlayer()
    {
        
        playerHealth =playerHealth-gameObject.GetComponent<EnemyHealth>().maxHealth;
        text.text = playerHealth.ToString(); // Actualiza la UI solo aquí

        if ( playerHealth <= 0)
        {
            photonView.RPC("GameOver1", RpcTarget.AllBuffered);
            Time.timeScale = 0;
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                StartCoroutine(SendRequest());
            }
            
        }
    }



               
              
            
        
    

    IEnumerator SendRequest()
    {
        int playerid = PlayerPrefs.GetInt("UserID");
        Debug.Log("Enviando solicitud de victoria para el jugador con ID: " + playerid);
        string url = "https://catbattle.duckdns.org/api/win";
        string json = "{\"user_id\": "+playerid+"}"; // Formato correcto de JSON como string
        UnityWebRequest request = UnityWebRequest.Put(url, json); // Usar PUT si la API lo requiere, o cambiar a POST si es necesario
        Debug.Log("Enviando solicitud a " + url + " con JSON: " + json);
        request.SetRequestHeader("Content-Type", "application/json"); // Establecer el encabezado Content-Type como application/json
        request.method = "POST"; // Asegúrate de utilizar el método HTTP correcto que la API espera (POST, en este caso)

        yield return request.SendWebRequest(); // Enviar la solicitud y esperar a que se complete

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error al enviar la solicitud: " + request.error);
        }
        else
        {
            Debug.Log("Respuesta de la solicitud: " + request.downloadHandler.text);
        }
    }

    [PunRPC]
    void GameOver1()
    {
        GameObject.Find("Funcionbotones").GetComponent<TowerManagement>().partidainiciada = false;
        paneljuego1.SetActive(false);
        paneljuego2.SetActive(false);
        panelganJ2.SetActive(true);
        panelperJ1.SetActive(true);
        GameManager.Instance.LoadMenuAfterDelay(5.0f);
    }

   
}

