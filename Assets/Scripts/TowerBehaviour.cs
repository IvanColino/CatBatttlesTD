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
    public int nivel = 1;
    private float fireCountdown = 0f;
    public Transform currentTarget;
    public GameObject panel;
    
    //Diccionario para almacenar los niveles de la torre
    private Dictionary<int, TowerLevel> levels;
    void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
            
        }
        InitializeLevels();

      
        ApplyLevel(nivel);
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
                if (photonView.IsMine) // Asegurarse de que solo el due�o instancie la bala
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
    void ApplyLevel(int level)
    {
        if (levels.ContainsKey(level))
        {
            TowerLevel towerLevel = levels[level];
            fireRate = towerLevel.fireRate;
            detectionRadius = towerLevel.detectionRadius;
        }
    }
    private void OnMouseDown()
    {
        if (photonView.IsMine)
        {
            TowerUpgrades.Instance.SelectTower(this);
        }
    }
    [PunRPC]
    public void LevelUp()
    {
        nivel++;
        ApplyLevel(nivel);
    }
    void InitializeLevels()
    {
        levels = new Dictionary<int, TowerLevel>
        {
            { 1, new TowerLevel(0.5f, 1.75f) },
            { 2, new TowerLevel(0.75f, 1.80f) },
            { 3, new TowerLevel(1.0f, 1.85f) },
            { 4, new TowerLevel(1.25f,1.90f) },
            { 5, new TowerLevel(1.50f, 1.95f) },
            { 6, new TowerLevel(1.75f, 2f) },
           
        };
    }
    [System.Serializable]
    public class TowerLevel
    {
        public float fireRate;
        public float detectionRadius;

        public TowerLevel(float fireRate, float detectionRadius)
        {
            this.fireRate = fireRate;
            this.detectionRadius = detectionRadius;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
