using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class MoveEnemies1 : MonoBehaviourPun
{
    public Transform[] waypoints;
    public float speed;
    public int waypointIndex = 0;
    private GameObject[] waypointsObjects = new GameObject[12];
    public TextMeshProUGUI text;
    private int playerHealth = 100;
    void Start()
    {
        text = GameObject.Find("Player" + 2 + "HP").GetComponent<TextMeshProUGUI>();
        if (!photonView.IsMine)
        {
            enabled = false;
        }

        for (int i = 0; i < 12; i++)
        {
            waypointsObjects[i] = GameObject.Find("Waypoint2" + i);
           
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
                Debug.Log("Enemy reached the end!");
                photonView.RPC("DamagePlayer2", RpcTarget.AllBuffered);
                PhotonNetwork.Destroy(gameObject);  // Destruir el enemigo en todos los clientes
            }
        }
    }

    [PunRPC]
    void DamagePlayer2()
    {
        playerHealth = playerHealth- gameObject.GetComponent<EnemyHealth>().maxHealth;  
        Debug.Log("Player Health: " + playerHealth);
        text.text = playerHealth.ToString();
        if (playerHealth <= 0)
        {
            Time.timeScale = 0;
            if (PhotonNetwork.LocalPlayer.ActorNumber==1)
            {
                StartCoroutine(SendRequest());
            }
           
            //  muerte del jugador
            Debug.Log("Player died!");
        }
    }
    IEnumerator SendRequest()
    {
       
        int playerid = PlayerPrefs.GetInt("UserID");
        Debug.Log("Sending request to API" + playerid);
        string url = "https://catbattle.duckdns.org/api/win";
        string json = "{\"user_id\": " + playerid + "}"; // Formato correcto de JSON como string
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


}
