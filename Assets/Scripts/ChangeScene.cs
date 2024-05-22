using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    void Awake()
    {
        // Asegurarse de que solo haya una instancia de GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMenuAfterDelay(float waitTime)
    {
        StartCoroutine(WaitAndLoadMenu(waitTime));
    }

    private IEnumerator WaitAndLoadMenu(float waitTime)
    {
        Debug.Log("Started coroutine to load menu after delay");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Time to load the menu scene now");
        photonView.RPC("ChangeScene", RpcTarget.All, "MenuPrincipal");
    }

    [PunRPC]
    void ChangeScene(string sceneName)
    {
        Debug.Log("Changing scene to: " + sceneName);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(sceneName);
    }
}
