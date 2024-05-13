using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class COnfirmacionAplicacion : MonoBehaviour
{
    public GameObject confirmationPanel;
    public GameObject aplicacionPrincipalPanel;
    public Button confirmButton;
    public Button cancelButton;

    // Start is called before the first frame update
    void Start()
    {

        confirmButton.onClick.AddListener(ConfirmarSalir);

        // Agrega un listener al bot�n de cancelar para que oculte el panel de confirmaci�n
        cancelButton.onClick.AddListener(CancelarSalir);
    }

    // Implementa el m�todo de la interfaz IPointerClickHandler
    public void OnPointerClick(PointerEventData eventData)
    {
        // Verifica si el evento t�ctil es un toque en este objeto
        if (eventData.pointerPress == gameObject)
        {
            // Llama a la funci�n correspondiente seg�n el nombre del bot�n
            switch (gameObject.name)
            {
                case "ConfirmButton":
                    ConfirmarSalir();
                    break;
                case "CancelButton":
                    CancelarSalir();
                    break;
            }
        }
    }




    void ConfirmarSalir()
    {
        // Cierra la aplicaci�n
        Application.Quit();
    }

    void CancelarSalir()
    {
        // Oculta el panel de confirmaci�n
        confirmationPanel.SetActive(false);
        aplicacionPrincipalPanel.SetActive(true);
    }
}
