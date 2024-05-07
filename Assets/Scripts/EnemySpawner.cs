using Photon.Pun;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviourPun
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public float spawnInterval = 50f; // Intervalo entre cada spawn
    public Transform[] spawnPoints; // Puntos de aparición
    public int enemiesPerRound = 5; // Número de enemigos por ronda
    private int currentRound = 0;

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
            yield return new WaitForSeconds(spawnInterval); // Espera entre rondas
            int enemiesSpawned = 0;
            while (enemiesSpawned < enemiesPerRound)
            {
                foreach (Transform spawnPoint in spawnPoints)
                {
                    if (enemiesSpawned >= enemiesPerRound) break;
                    PhotonNetwork.Instantiate(enemyPrefab.name, spawnPoint.position, Quaternion.identity);
                    enemiesSpawned++;
                    yield return new WaitForSeconds(1f); // Espera entre enemigos
                }
            }
            currentRound++;
            photonView.RPC("UpdateRoundOnClients", RpcTarget.Others, currentRound);
        }
    }

    [PunRPC]
    void UpdateRoundOnClients(int round)
    {
        currentRound = round; // Actualiza la ronda actual en todos los clientes
    }
}
