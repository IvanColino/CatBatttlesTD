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
    private Transform currentTarget;



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
        // Reset current target if it moves out of range or is destroyed
        if (currentTarget != null && Vector2.Distance(transform.position, currentTarget.position) > detectionRadius)
        {
            currentTarget = null;
        }

        // Find new target if there is no current target
        if (currentTarget == null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    currentTarget = hit.transform;
                    break; // Assign the first enemy found as the target
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
                Shoot(currentTarget);
                fireCountdown = 1f / fireRate; // Reset fire countdown
            }
            fireCountdown -= Time.deltaTime; // Decrement countdown
        }
    }

    void Shoot(Transform enemy)
    {
        if (Vector2.Distance(transform.position, enemy.position) <= detectionRadius)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            BulletBehaviour bullet = bulletGO.GetComponent<BulletBehaviour>();
            if (bullet != null)
                bullet.Seek(enemy);
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize detection radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
