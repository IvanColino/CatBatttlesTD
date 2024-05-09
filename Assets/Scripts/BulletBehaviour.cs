using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;

public class BulletBehaviour : MonoBehaviourPun
{
    private Transform target;
    public float speed = 70f;
    private float originalZ;  // Variable para almacenar la posición z original de la bala
    private int ownerActorNumber;
    private PhotonView photonView;
    public GameObject Funcionbotones;
    public void Start()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destruir la bala si el objetivo ya no existe
            return;
        }
      Funcionbotones = GameObject.Find("Funcionbotones");
      photonView = Funcionbotones.GetComponent<PhotonView>();
    }
    // Método para dirigir la bala hacia el enemigo
    public void Seek(Transform _target)
    {
        target = _target;
        originalZ = transform.position.z;  // Guarda la posición z original cuando el objetivo es establecido
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destruir la bala si el objetivo ya no existe
            return;
        }

        Vector2 direction = (new Vector2(target.position.x, target.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
        float distanceThisFrame = speed * Time.deltaTime;

        // Mover la bala hacia el objetivo
        Vector3 newTargetPos = new Vector3(target.position.x, target.position.y, originalZ); 
        transform.position = Vector3.MoveTowards(transform.position, newTargetPos, distanceThisFrame);

        // Verificar si la bala ha alcanzado al objetivo
        if (Vector3.Distance(transform.position, newTargetPos) < 0.1f)
        {
            
            Shoot();
           
        }
    }

    private void Shoot () {
        HitTarget();
        photonView.RPC("AddPoints", RpcTarget.AllBuffered, ownerActorNumber); }
    

    public void SetOwner(int actorNumber)
    {
        ownerActorNumber = actorNumber;
    }
    [PunRPC]
    void HitTarget()
    {


        Destroy(target.gameObject);
        Destroy(gameObject);

    }
}
