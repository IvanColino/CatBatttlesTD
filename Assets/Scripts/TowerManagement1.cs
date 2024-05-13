using UnityEngine;
using Photon.Pun;  // Aseg�rate de incluir el namespace para Photon

public class TowerManagement1 : MonoBehaviour
{
    public GameObject towerPrefab; // Prefab de la torre
    public GameObject currentTowerInstance; // Instancia actual de la torre en movimiento
    public GameObject panel;
    private bool isPlacingTower = false;
    private PhotonView photonView;

    // Llamado al presionar un bot�n UI para iniciar el proceso de arrastre de la torre
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    
        public void OnButtonPress()
        {
            if (towerPrefab != null && currentTowerInstance == null && PhotonNetwork.LocalPlayer.ActorNumber==2) // Asegurarse de no crear m�s de una torre a la vez
            {
                Vector3 spawnPosition = GetMouseWorldPosition();
                // Usar PhotonNetwork.Instantiate para crear la torre en todos los clientes
                currentTowerInstance = PhotonNetwork.Instantiate(towerPrefab.name, spawnPosition, Quaternion.identity);
           
                isPlacingTower = true; // Iniciar el proceso de colocaci�n
            }
    }

    void Update()
    {
        if (isPlacingTower && currentTowerInstance != null)
        {
            // Mover la torre a la posici�n del cursor
            if (!photonView.IsMine) // Solo el master client mueve la torre hasta que se confirma la posici�n
            {
                return;
            }
            currentTowerInstance.transform.position = GetMouseWorldPosition();

            // Si el usuario hace clic en el lugar deseado, confirmar la posici�n
            if (Input.GetMouseButtonDown(0)) // 0 es el bot�n izquierdo del rat�n
            {
                Vector3 finalPosition = new Vector3(currentTowerInstance.transform.position.x, currentTowerInstance.transform.position.y, panel.transform.position.z);
                currentTowerInstance = null; // Liberar la referencia para permitir nuevas instancias
                isPlacingTower = false; // Dejar de mover la torre
                photonView.RPC("FinalizePosition", RpcTarget.AllBuffered, finalPosition);
                
            }
        }
    }

    

    // M�todo para obtener la posici�n del mundo basada en la posici�n del rat�n
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = panel.transform.position.z - Camera.main.transform.position.z; // Ajustar la profundidad para que se ajuste a tu configuraci�n de c�mara/escena
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    [PunRPC]
    void FinalizePosition(Vector3 finalPosition)
    {
        Debug.Log("RPC FinalizePosition called with: " + finalPosition);
        if (currentTowerInstance != null)
        {
            Debug.Log("Buenas");
            currentTowerInstance.transform.position = finalPosition;
        }
    }
}
