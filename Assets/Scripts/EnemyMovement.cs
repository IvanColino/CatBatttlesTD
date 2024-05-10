using Photon.Pun;
using TMPro;
using UnityEngine;

public class MoveEnemies : MonoBehaviourPun
{
    public Transform[] waypoints;
    public float speed;
    private int waypointIndex = 0;
    private GameObject[] waypointsObjects = new GameObject[12];
    public int playerHealth = 0;
    private TextMeshProUGUI text;

    void Start()
    {
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
        playerHealth =playerHealth- 1;  
        Debug.Log("Player Health: " + playerHealth);
        text.text = int.Parse(text.text)-1 + "";
        if (playerHealth <= 0)
        {
            //muerte del jugador
            Debug.Log("Player died!");
        }
    }
}
    
