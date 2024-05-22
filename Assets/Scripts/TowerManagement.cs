using UnityEngine;
using Photon.Pun;
using TMPro;  // Asegúrate de incluir el namespace para Photon

public class TowerManagement : MonoBehaviour
{
    public GameObject[] towerPrefab; // Prefab de la torre
    public GameObject currentTowerInstance; // Instancia actual de la torre en movimiento
    public GameObject panel;
    private bool isPlacingTower = false;
    private PhotonView photonView;
    private TextMeshProUGUI text;
    private int playerscore;
    public bool partidainiciada = false;

    // Llamado al presionar un botón UI para iniciar el proceso de arrastre de la torre
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    
        
    }

    public void OnButtonPressPlayer1()
    {
        // Comprobar si el jugador actual es el Jugador 1
        if (towerPrefab != null && currentTowerInstance == null && PhotonNetwork.LocalPlayer.ActorNumber == 1&&playerscore>=300)
        {
            
           
            CreateTower(PlayerPrefs.GetInt("SkinId", 0));
            playerscore =playerscore-300;
            text.text = playerscore + "";
        }
    }
    public void OnButtonPressPlayer2()
    {
        // Comprobar si el jugador actual es el Jugador 1
        if (towerPrefab != null && currentTowerInstance == null && PhotonNetwork.LocalPlayer.ActorNumber == 2&& playerscore>=300)
        {


            CreateTower(PlayerPrefs.GetInt("SkinId", 0));
            playerscore = playerscore - 300;
            text.text = playerscore + "";
        }
    }
    

    private void CreateTower(int SkinId)
    {
        Vector3 spawnPosition = GetMouseWorldPosition();
        currentTowerInstance = PhotonNetwork.Instantiate(towerPrefab[SkinId].name, spawnPosition, Quaternion.identity);
        isPlacingTower = true; // Iniciar el proceso de colocación
    }
    void Update()
    {
       if (partidainiciada)
        {
            text = GameObject.Find("Player" + PhotonNetwork.LocalPlayer.ActorNumber + "Score").GetComponent<TextMeshProUGUI>();
            playerscore = int.Parse(text.text);
        }
        if (isPlacingTower && currentTowerInstance != null && currentTowerInstance.GetComponent<PhotonView>().IsMine)
        {
            // Mover la torre a la posición del cursor
           
            currentTowerInstance.transform.position = GetMouseWorldPosition();

            // Si el usuario hace clic en el lugar deseado, confirmar la posición
            if (Input.GetMouseButtonDown(0)) // 0 es el botón izquierdo del ratón
            {

                Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log(clickPosition);
                if ((clickPosition.x < 538 && PhotonNetwork.LocalPlayer.ActorNumber == 1) || (clickPosition.x > 541 && PhotonNetwork.LocalPlayer.ActorNumber == 2))
                {
                    Debug.Log("HOla entre");
                    FinalizeTowerPlacement();
                }
                else
                {
                    Debug.Log("Acción no permitida en esta área.");
                }
            }
        }
    }


    private void FinalizeTowerPlacement()
    {
        Vector3 finalPosition = new Vector3(currentTowerInstance.transform.position.x, currentTowerInstance.transform.position.y, panel.transform.position.z);
        
        isPlacingTower = false; // Dejar de mover la torre
                               
        PhotonView towerPhotonView = currentTowerInstance.GetComponent<PhotonView>();
        currentTowerInstance = null; // Liberar la referencia para permitir nuevas instancias
        if (towerPhotonView != null)
        {
            towerPhotonView.RPC("FinalizePosition", RpcTarget.AllBuffered, finalPosition);
        }
    }
    // Método para obtener la posición del mundo basada en la posición del ratón
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = panel.transform.position.z - Camera.main.transform.position.z; // Ajustar la profundidad para que se ajuste a tu configuración de cámara/escena
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
