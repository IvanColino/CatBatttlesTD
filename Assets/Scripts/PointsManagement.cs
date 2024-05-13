using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsManagement : MonoBehaviourPun
{
    public int playerScore = 0;
    public int pointsPerKill = 10;

    public void UpdateScoreDisplay()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            GameObject.Find("Player1Score").GetComponent<TextMeshProUGUI>().text = playerScore+"";
        }
        else
        {
            GameObject.Find("Player2Score").GetComponent<TextMeshProUGUI>().text = playerScore+ "";
        }
    }

    [PunRPC]
    public void AddPoints(int actorNumber)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
           Debug.Log("Player " + actorNumber + " has  killed!");
            playerScore += pointsPerKill; // Asegúrate de tener una variable 'playerScore' y 'pointsPerKill'
            UpdateScoreDisplay(); // Actualiza cualquier UI o lógica relacionada con la puntuación
        }
    }
}

