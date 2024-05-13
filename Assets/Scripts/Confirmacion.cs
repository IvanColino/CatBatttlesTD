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

        // Agrega un listener al botón de cancelar para que oculte el panel de confirmación
        cancelButton.onClick.AddListener(CancelarSalir);
    }

    // Implementa el método de la interfaz IPointerClickHandler
    public void OnPointerClick(PointerEventData eventData)
    {
        // Verifica si el evento táctil es un toque en este objeto
        if (eventData.pointerPress == gameObject)
        {
            // Llama a la función correspondiente según el nombre del botón
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
        // Cierra la aplicación
        Application.Quit();
    }

    void CancelarSalir()
    {
        // Oculta el panel de confirmación
        confirmationPanel.SetActive(false);
        aplicacionPrincipalPanel.SetActive(true);
    }
}
