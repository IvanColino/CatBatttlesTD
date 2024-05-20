using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConectMaster : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {

        //PhotonNetwork.NickName = PlayerPrefs.GetString("Username"); 
        PhotonNetwork.NickName = "Player"+PhotonNetwork.LocalPlayer.ActorNumber;
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Se va a conectar al Master");

    }

   
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al Master");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }
    

}
