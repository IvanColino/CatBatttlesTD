using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FuncionamientoBoton : MonoBehaviour
{
    public GameObject confirmationPanel;
    public GameObject aplicacionPrincipalPanel;
    public GameObject loginpanel;
    public GameObject registropanel;
    public GameObject BotonCerrarSesion;
    public GameObject BotonIniciarSesion;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Verifica si el evento táctil es un toque en este objeto
        if (eventData.pointerPress == gameObject)
        {
            // Llama a la función correspondiente según el nombre del botón
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

        }
        else
        {

        }
    }

    public void AccionShop()
    {
        bool sessionActive = PlayerPrefs.HasKey("UserID");
        if (sessionActive)
        {

        }
        else
        {

        }
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
