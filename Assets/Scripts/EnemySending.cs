using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySending : MonoBehaviourPun
{
    // Prefabs de los enemigos
    public GameObject enemyType1;
    public GameObject enemyType2;

    // Posiciones de spawn de los enemigos
    public GameObject spawner;
    public GameObject spawner2;

    void Start()
    {

    }

    void Update()
    {

    }

    // Método que será llamado cuando se presione el botón
    public void OnButtonPress()
    {
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            // Jugador 1 instancia el enemigo directamente y sabe quién presionó el botón

            PhotonNetwork.Instantiate(enemyType2.name, spawner2.transform.position, Quaternion.identity);
        }
        else
        {
           Debug.Log("SPAWNEAAAAAAAAAAA");
            photonView.RPC("RequestEnemySpawn", RpcTarget.MasterClient);
        }
        }


        [PunRPC]
        void RequestEnemySpawn()
        {
           
                    PhotonNetwork.Instantiate(enemyType1.name, spawner.transform.position, Quaternion.identity);
                

            
        }
    }


