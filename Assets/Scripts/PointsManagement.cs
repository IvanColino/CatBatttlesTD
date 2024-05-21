using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsManagement : MonoBehaviourPun
{
   
    public int pointsPerKill = 10;
    public int score = 0;
    

    public void UpdateScoreDisplay()
    {
        // Obtener la referencia al componente TextMeshProUGUI correspondiente según el número de actor
        TextMeshProUGUI scoreText = null;

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            scoreText = GameObject.Find("Player1Score").GetComponent<TextMeshProUGUI>();
            score=int.Parse(scoreText.text);
            scoreText.text = (score+pointsPerKill)+"";
        }
        else
        {
            scoreText = GameObject.Find("Player2Score").GetComponent<TextMeshProUGUI>();
            score = int.Parse(scoreText.text);
            scoreText.text = (score + pointsPerKill) + "";
        }

        // Actualizar el texto de la puntuación
       
    }
    [PunRPC]
    public void AddPoints(int actorNumber)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
           Debug.Log("Player " + actorNumber + " has  killed!");
            
            UpdateScoreDisplay(); // Actualiza cualquier UI o lógica relacionada con la puntuación
        }
    }
}

