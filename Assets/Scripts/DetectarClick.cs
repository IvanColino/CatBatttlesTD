using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarClick : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {  // Si el jugador hace clic
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Click en la posición: " + clickPosition);
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && clickPosition.x < 540)
            {
                Debug.Log( PhotonNetwork.NickName+"hizo clic en el lado izquierdo");
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && clickPosition.x > 541)
            {
                Debug.Log(PhotonNetwork.NickName+"hizo clic en el lado derecho");
            }
            else
            {
                Debug.Log("Acción no permitida en esta área.");
            }
        }
    }
    }
