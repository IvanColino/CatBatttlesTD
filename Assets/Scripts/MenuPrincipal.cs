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

    public void AccionPlay()
    {

    }

    public void AccionShop()
    {

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