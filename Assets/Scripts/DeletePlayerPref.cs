using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlayerPref : MonoBehaviour
{
    void OnApplicationQuit()
    {
        // Aquí borramos todas las claves de PlayerPrefs
        PlayerPrefs.DeleteAll();
        // Guardamos los cambios para asegurarnos de que se apliquen
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs borrados al cerrar la aplicación.");
    }
}
