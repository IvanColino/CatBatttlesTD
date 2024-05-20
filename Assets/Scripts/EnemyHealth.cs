using UnityEngine;
using Photon.Pun;
using System.Collections;

public class EnemyHealth : MonoBehaviourPun
{
    public int maxHealth = 1;
    private int currentHealth;
    private GameObject pointsManager;
    private Animator animator;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pointsManager = GameObject.Find("Funcionbotones");
           
        }
      
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    [PunRPC]
    public void TakeDamage(int damage,int actornumber)
    {
        currentHealth -= damage;

        if (currentHealth <= 0&& PhotonNetwork.IsMasterClient)
        {
            pointsManager.GetComponent<PhotonView>().RPC("AddPoints", RpcTarget.All, actornumber);
            photonView.RPC("Die", RpcTarget.All);
        }
    }
    [PunRPC]
    void Die()
    {
        animator.SetTrigger("dead"); // Activa la animación de muerte
        StartCoroutine(DestroyAfterAnimation()); // Inicia la coroutine para destruir el objeto
    }
    private IEnumerator DestroyAfterAnimation()
    {
        // Espera hasta que la animación de muerte termine
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject); // Destruye el objeto
        }
       
    }
}
