using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrades : MonoBehaviour
{
    public static TowerUpgrades Instance;

    private TowerBehaviour selectedTower;
    public GameObject Upgradepanel;
    public GameObject Upgradepanel2;
    public TextMeshProUGUI pointsJ1;
    public TextMeshProUGUI pointsJ2;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        
    }

    // Método para seleccionar una torre
    public void SelectTower(TowerBehaviour tower)
    {
       
       
        selectedTower = tower;
        
       
        if (selectedTower.photonView.IsMine && PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            Upgradepanel.SetActive(true);
            for (int i = 1; i <= 6; i++)
            {
                Toggle toogle = GameObject.Find("Nivel " + i).GetComponent<Toggle>();
                toogle.isOn = false;
            }
            if (selectedTower.nivel >= 6)
            {
                GameObject.Find("btnUpgrade").SetActive(false);

            }
            else {                 GameObject.Find("btnUpgrade").SetActive(true);
                       }
            for (int i = 1; i <= selectedTower.nivel; i++)
            {
                Toggle toogle = GameObject.Find("Nivel " + i).GetComponent<Toggle>();
                toogle.isOn = true;
            }

            Debug.Log("Torre seleccionada: Nivel " + selectedTower.nivel);
        }
        else if (selectedTower.photonView.IsMine && PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
           
            Upgradepanel2.SetActive(true);
            if (selectedTower.nivel >= 6)
            {
                GameObject.Find("btnUpgrade2").SetActive(false);

            }
            else
            {
                GameObject.Find("btnUpgrade2").SetActive(true);
            }
            for (int i = 1; i <= selectedTower.nivel; i++)
            {
                Toggle toogle = GameObject.Find("Nivel2 " + i).GetComponent<Toggle>();
                toogle.isOn = true;
            }

            Debug.Log("Torre seleccionada: Nivel " + selectedTower.nivel);
        }
    }

    // Método para subir de nivel la torre seleccionada
    public void UpgradeSelectedTower()
    {
        int points1 = int.Parse(pointsJ1.text);
        int points2 = int.Parse(pointsJ2.text);
        if (selectedTower != null && selectedTower.photonView.IsMine )
        {
            
            // Aquí podrías actualizar el panel UI con la nueva información de la torre
            Debug.Log("Torre subida de nivel: Nuevo nivel " + selectedTower.nivel);
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 &&points1>200)
            {
                pointsJ1.text = (points1 - 200).ToString();
                selectedTower.photonView.RPC("LevelUp", RpcTarget.AllBuffered);
                Upgradepanel.SetActive(true);
                for (int i = 1; i <= selectedTower.nivel; i++)
                {
                    Toggle toogle = GameObject.Find("Nivel " + i).GetComponent<Toggle>();
                    toogle.isOn = true;
                }
                if (selectedTower.nivel >= 6)
                {
                    GameObject.Find("btnUpgrade2").SetActive(false);

                }
                else
                {
                    GameObject.Find("btnUpgrade").SetActive(true);
                }
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && points2 > 200)
            {
                pointsJ2.text = (points2 - 200).ToString();
                Upgradepanel2.SetActive(true);
                for (int i = 1; i <= selectedTower.nivel; i++)
                {
                    selectedTower.photonView.RPC("LevelUp", RpcTarget.AllBuffered);
                    Toggle toogle = GameObject.Find("Nivel2 " + i).GetComponent<Toggle>();
                    toogle.isOn = true;
                }
                if (selectedTower.nivel >= 6)
                {
                    GameObject.Find("btnUpgrade2").SetActive(false);

                }
                else
                {
                    GameObject.Find("btnUpgrade2").SetActive(true);
                }
            }
          

        }
    }
    
}
