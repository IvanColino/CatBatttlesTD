using UnityEngine;
using Photon.Pun;

public class BulletBehaviour : MonoBehaviourPun
{
    private Transform target;
    public float speed = 70f;
    private float originalZ;  // Variable para almacenar la posici�n z original de la bala

    // M�todo para dirigir la bala hacia el enemigo
    public void Seek(Transform _target)
    {
        target = _target;
        originalZ = transform.position.z;  // Guarda la posici�n z original cuando el objetivo es establecido
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
            HitTarget();
        }
    }

    [PunRPC]
    void HitTarget()
    {
         Destroy(target.gameObject); // Destruir el objeto del enemigo (opcional)
         Destroy(gameObject); // Destruir la bala
    }
}
