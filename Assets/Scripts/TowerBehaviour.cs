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
        PhotonView photonView = GetComponent<PhotonView>();
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
                if (photonView.IsMine) // Asegurarse de que solo el dueño instancie la bala
                {
                    Shoot(currentTarget.position);
                }
                fireCountdown = 1f / fireRate; // Reset fire countdown
            }
            fireCountdown -= Time.deltaTime; // Decrement countdown
        }
    }

    void Shoot(Vector3 enemyPosition)
    {
        if (Vector2.Distance(transform.position, enemyPosition) <= detectionRadius)
        {
            GameObject bulletGO = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, Quaternion.identity);
            BulletBehaviour bullet = bulletGO.GetComponent<BulletBehaviour>();
            if (bullet != null)
            {
                bullet.photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
                bullet.Seek(((Collider2D)Physics2D.OverlapPoint(enemyPosition, enemyLayer)).transform);
                bullet.ownerActorNumber = photonView.OwnerActorNr;
            }
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
