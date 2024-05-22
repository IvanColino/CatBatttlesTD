using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FuncionamientoBoton : MonoBehaviour
{
    public GameObject confirmationPanel;
    public GameObject aplicacionPrincipalPanel;
    public GameObject loginpanel;
    public GameObject registropanel;
    public GameObject BotonCerrarSesion;
    public GameObject BotonIniciarSesion;
    public GameObject tiendapanel;
    public GameObject panelOK;
    public GameObject funcionBoton;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Verifica si el evento t�ctil es un toque en este objeto
        if (eventData.pointerPress == gameObject)
        {
            // Llama a la funci�n correspondiente seg�n el nombre del bot�n
            switch (gameObject.name)
            {
                case "BotonPlay":
                    AccionPlay();
                    break;
                case "BotonCerrarSesion":
                    CerrarSesion();
                    break;
                case "BotonShop":
                    AccionShop();
                    break;
                case "BotonExit":
                    AccionExit();
                    break;
                case "BotonLogin":
                    AccionLogin();
                    break;
                case "BotonPasaregistro":
                    AccionPasarRegistro();
                    break;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("SkinId", -1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CerrarSesion()
    {
        PlayerPrefs.DeleteKey("UserID");
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("Monedas");
        PlayerPrefs.Save();
        BotonCerrarSesion.SetActive(false);
        BotonIniciarSesion.SetActive(true);
    }

    public void AccionPlay()
    {
        bool sessionActive = PlayerPrefs.HasKey("UserID");
        if (sessionActive)
        {
            SceneManager.LoadScene("Juego");
        }
        else
        {
            aplicacionPrincipalPanel.SetActive(false);
            panelOK.SetActive(true);
        }
    }

    public void AccionShop()
    {
        bool sessionActive = PlayerPrefs.HasKey("UserID");
        if (sessionActive)
        {
            tiendapanel.SetActive(true);
            aplicacionPrincipalPanel.SetActive(false);
            funcionBoton.GetComponent<BuySkin>().CheckIfSkinPurchased();
        }
        else
        {
            aplicacionPrincipalPanel.SetActive(false);
            panelOK.SetActive(true);
        }
    }

    public void AccionOK()
    {
        panelOK.SetActive(false);
        aplicacionPrincipalPanel.SetActive(true);
    }

    public void AccionExit()
    {
        aplicacionPrincipalPanel.SetActive(false);
        confirmationPanel.SetActive(true);
        
    }

    public void AccionLogin()
    {
        aplicacionPrincipalPanel.SetActive(false);
        loginpanel.SetActive(true);
        
    }

    public void AccionBack()
    {
        loginpanel.SetActive(false);
        registropanel.SetActive(false);
        aplicacionPrincipalPanel.SetActive(true);
    }

    public void AccionPasarRegistro()
    {
        loginpanel.SetActive(false);
        registropanel.SetActive(true);
    }
}
