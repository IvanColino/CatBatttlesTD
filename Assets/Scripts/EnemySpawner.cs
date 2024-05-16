using Photon.Pun;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviourPun
{
    public RoundData[] rounds;
 
    public float spawnInterval = 200f; // Intervalo entre cada spawn
    public Transform[] spawnPoints; // Puntos de aparición
    public int enemiesPerRound = 5; // Número de enemigos por ronda
    private int currentRound = 1;
    public GameObject texto_Ronda;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            foreach (var round in rounds)
            {
                yield return new WaitForSeconds(spawnInterval); // Espera entre rondas
                for (int i = 0; i < round.enemyCount; i++)
                {
                    Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
                    PhotonNetwork.Instantiate(round.enemyPrefab.name, spawnPoint.position, Quaternion.identity);
                    yield return new WaitForSeconds(1f); // Espera entre enemigos
                }
                currentRound++;
                photonView.RPC("UpdateRoundOnClients", RpcTarget.All, currentRound);
                if (currentRound >= rounds.Length)
                    break;  
            }
        }
    }

    [PunRPC]
    void UpdateRoundOnClients(int round)
    {
        currentRound = round; // Actualiza la ronda actual en todos los clientes
        texto_Ronda.GetComponent<TextMeshProUGUI>().text = "Ronda: " + currentRound+"/15";
    }
    [System.Serializable]
    public class RoundData
    {
        public GameObject enemyPrefab;  // Puedes tener diferentes prefabs para diferentes rondas
        public int enemyCount;          // Cantidad de enemigos en esta ronda
    }
}
