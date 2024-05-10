using Photon.Pun;
using TMPro;
using UnityEngine;

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
        playerHealth -= 1;  
        Debug.Log("Player Health: " + playerHealth);
        text.text = int.Parse(text.text)-1 + "";
        if (playerHealth <= 0)
        {
            //  muerte del jugador
            Debug.Log("Player died!");
        }
    }
}
