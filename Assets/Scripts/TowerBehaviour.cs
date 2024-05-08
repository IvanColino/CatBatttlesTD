using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TowerBehaviour : MonoBehaviourPun
{
    public float detectionRadius = 5f;
    public LayerMask enemyLayer;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float fireCountdown = 0f;
    public Transform currentTarget;
    public GameObject panel;

    void Start()
    {
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
            
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            DetectEnemies();
            ManageShooting();
        }
    }

    void DetectEnemies()
    {
        if (currentTarget != null && Vector2.Distance(transform.position, currentTarget.position) > detectionRadius)
        {
            currentTarget = null;
        }

        if (currentTarget == null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    currentTarget = hit.transform;
                    break;
                }
            }
        }
    }

    void ManageShooting()
    {
        if (currentTarget != null)
        {
            if (fireCountdown <= 0f)
            {
                photonView.RPC("Shoot", RpcTarget.All, currentTarget.position);
                fireCountdown = 1f / fireRate; // Reset fire countdown
            }
            fireCountdown -= Time.deltaTime; // Decrement countdown
        }
    }

    [PunRPC]
    void Shoot(Vector3 enemyPosition)
    {
        if (Vector2.Distance(transform.position, enemyPosition) <= detectionRadius)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
          
            BulletBehaviour bullet = bulletGO.GetComponent<BulletBehaviour>();
            if (bullet != null)
            {
                Transform enemyTransform = ((Collider2D)Physics2D.OverlapPoint(enemyPosition, enemyLayer)).transform;
                bullet.Seek(enemyTransform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
