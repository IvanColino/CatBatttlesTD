using UnityEngine;
using Photon.Pun;
public class BulletBehaviour : MonoBehaviourPun
{
    private Transform target;
    public float speed = 70f;


    // Método para dirigir la bala hacia el enemigo
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destruir la bala si el objetivo ya no existe
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;

        // Mover la bala hacia el objetivo
        transform.position = Vector2.MoveTowards(transform.position, target.position, distanceThisFrame);

        // Verificar si la bala ha alcanzado al objetivo
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            HitTarget();
        }
    }

    [PunRPC]
    void HitTarget()
    {
       

       
        Destroy(target.gameObject); // Destruir el objeto del enemigo
        Destroy(gameObject); // Destruir la bala
    }
}
