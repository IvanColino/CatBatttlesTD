using Photon.Pun;
using UnityEngine;

public class BulletBehaviour : MonoBehaviourPun
{
    public float speed = 70f;
    private Transform target;
    private float originalZ;
    public int ownerActorNumber;
    void Update()
    {
        if (target == null)
        {
            // Verificar si este cliente es el propietario antes de destruir el objeto.
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            return;
        }

        MoveTowardsTarget();
        CheckIfHitTarget();
    }

    void MoveTowardsTarget()
    {
        if (target != null)  // Verificar si el objetivo sigue siendo válido.
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, originalZ);
            Vector3 direction = targetPos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); 
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    void CheckIfHitTarget()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        // Asegurar que solo el propietario o el MasterClient ejecute este código.
        if (photonView.IsMine)
        {
            if (target.gameObject.GetComponent<PhotonView>() != null)
            {
                target.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 1,ownerActorNumber);
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void Seek(Transform _target)
    {
        target = _target;
        originalZ = transform.position.z;
    }
}
