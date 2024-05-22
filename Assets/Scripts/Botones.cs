using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Botones : MonoBehaviour
{
    public TMP_InputField inputCrear,inputUnirse;
    public TextMeshProUGUI textoCrear,textoUnirse;
    public Button botonCrear, botonUnirse,crear,unirse,volver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CrearSala()
    {
        botonCrear.gameObject.SetActive(false);
        botonUnirse.gameObject.SetActive(false);
        textoUnirse.gameObject.SetActive(false);
        inputUnirse.gameObject.SetActive(false);
        unirse.gameObject.SetActive(false);
        textoCrear.gameObject.SetActive(true);
        inputCrear.gameObject.SetActive(true);
        crear.gameObject.SetActive(true);
        volver.gameObject.SetActive(true);
    } public void UnirseSala()
    {
        volver.gameObject.SetActive(true);
        botonCrear.gameObject.SetActive(false);
        botonUnirse.gameObject.SetActive(false);
        textoCrear.gameObject.SetActive(false);
        inputCrear.gameObject.SetActive(false);
        crear.gameObject.SetActive(false); 
        textoUnirse.gameObject.SetActive(true);
        inputUnirse.gameObject.SetActive(true);
        unirse.gameObject.SetActive(true);
    }
    public void Volver()
    {
        botonCrear.gameObject.SetActive(true);
        botonUnirse.gameObject.SetActive(true);
        textoUnirse.gameObject.SetActive(false);
        inputUnirse.gameObject.SetActive(false);
        unirse.gameObject.SetActive(false);
        textoCrear.gameObject.SetActive(false);
        inputCrear.gameObject.SetActive(false);
        volver.gameObject.SetActive(false);
        crear.gameObject.SetActive(false);

    }

}
