using UnityEngine;
using Photon.Pun;

public class EnemyHealth : MonoBehaviourPun
{
    public int maxHealth = 1;
    private int currentHealth;
    private GameObject pointsManager;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pointsManager = GameObject.Find("Funcionbotones");
           
        }
      
        currentHealth = maxHealth;
      
    }

    [PunRPC]
    public void TakeDamage(int damage,int actornumber)
    {
        currentHealth -= damage;

        if (currentHealth <= 0&& PhotonNetwork.IsMasterClient)
        {
            pointsManager.GetComponent<PhotonView>().RPC("AddPoints", RpcTarget.All, actornumber);
            PhotonNetwork.Destroy(gameObject); // Destruir el enemigo en todos los clientes
        }
    }
}
