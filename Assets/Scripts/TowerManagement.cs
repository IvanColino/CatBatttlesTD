using UnityEngine;
using Photon.Pun;
using TMPro;  // Aseg�rate de incluir el namespace para Photon

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

    // Llamado al presionar un bot�n UI para iniciar el proceso de arrastre de la torre
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
        isPlacingTower = true; // Iniciar el proceso de colocaci�n
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
            // Mover la torre a la posici�n del cursor
           
            currentTowerInstance.transform.position = GetMouseWorldPosition();

            // Si el usuario hace clic en el lugar deseado, confirmar la posici�n
            if (Input.GetMouseButtonDown(0)) // 0 es el bot�n izquierdo del rat�n
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
                    Debug.Log("Acci�n no permitida en esta �rea.");
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
