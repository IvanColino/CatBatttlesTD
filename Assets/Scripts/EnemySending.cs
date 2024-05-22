using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySending : MonoBehaviourPun
{
    // Prefabs de los enemigos
    public GameObject enemyType1;
    public GameObject enemyType2;
    public GameObject enemyType21;
    public GameObject enemyType22;
    public GameObject enemyType31;
    public GameObject enemyType32;

    // Posiciones de spawn de los enemigos
    public GameObject spawner;
    public GameObject spawner2;
    public TextMeshProUGUI pointsJ1;
    public TextMeshProUGUI pointsJ2;


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
       
       
        int points1 = int.Parse(pointsJ1.text);
        int points2 = int.Parse(pointsJ2.text);

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1&& points1 >=100 )
        {
            pointsJ1.text = (points1 - 100).ToString();
            PhotonNetwork.Instantiate(enemyType2.name, spawner2.transform.position, Quaternion.identity);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2&& points2 >=100 )
        {
          pointsJ2.text = (points2 - 100).ToString();
            Debug.Log("Se va a enviar un enemigo");
            photonView.RPC("RequestEnemySpawn", RpcTarget.MasterClient);
        }
        }
    public void OnButtonPress2()
    {
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;


        int points1 = int.Parse(pointsJ1.text);
        int points2 = int.Parse(pointsJ2.text);

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && points1 >= 200)
        {
            pointsJ1.text = (points1 - 200).ToString();
            PhotonNetwork.Instantiate(enemyType22.name, spawner2.transform.position, Quaternion.identity);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && points2 >= 200)
        {
            pointsJ2.text = (points2 - 200).ToString();
            Debug.Log("Se va a enviar un enemigo");
            photonView.RPC("RequestEnemySpawn2", RpcTarget.MasterClient);
        }
    }
    public void OnButtonPress3()
    {
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;


        int points1 = int.Parse(pointsJ1.text);
        int points2 = int.Parse(pointsJ2.text);

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && points1 >= 2000)
        {
            pointsJ1.text = (points1 - 2000).ToString();
            PhotonNetwork.Instantiate(enemyType32.name, spawner2.transform.position, Quaternion.identity);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && points2 >= 2000)
        {
            pointsJ2.text = (points2 - 2000).ToString();
            Debug.Log("Se va a enviar un enemigo");
            photonView.RPC("RequestEnemySpawn3", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
        void RequestEnemySpawn()
        {
           
                    PhotonNetwork.Instantiate(enemyType1.name, spawner.transform.position, Quaternion.identity);
                

            
        }
    [PunRPC]
    void RequestEnemySpawn2()
    {

        PhotonNetwork.Instantiate(enemyType21.name, spawner.transform.position, Quaternion.identity);



    }
    [PunRPC]
    void RequestEnemySpawn3()
    {

        PhotonNetwork.Instantiate(enemyType31.name, spawner.transform.position, Quaternion.identity);



    }
}

    



