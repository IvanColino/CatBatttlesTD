using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class SalaManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputCrear, inputUnirse;
    public Button crear, unirse,empezar;
    public TextMeshProUGUI listadoJugadores, txtcrear, txtunirse, txtLobby;
    public List<string> Listajugadores = new List<string>();
    public GameObject panelmenu, paneljuego,paneljuego2,square,funcionbotones;
    public PhotonView photonView;
    private const string PropiedadJugadores = "JugadoresEnSala";


    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    public void Update()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                empezar.gameObject.SetActive(true);
            }
            else
            {
                empezar.gameObject.SetActive(false);
            }
            if (PhotonNetwork.CurrentRoom != null)
            {
                ActualizarListado();
            }

        }
    }
    public void CrearSala()
    {
        RoomOptions options = new RoomOptions() { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(inputCrear.text, options);

        txtLobby.gameObject.SetActive(true);
        txtLobby.text = "Lobby: " + inputCrear.text;
        txtcrear.gameObject.SetActive(false);
        crear.gameObject.SetActive(false);
        inputCrear.gameObject.SetActive(false);

        // Establecer la lista inicial de jugadores con el creador de la sala
      
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Creador"+PhotonNetwork.NickName);
        EstablecerListaJugadores(new List<string> { PhotonNetwork.NickName });
        listadoJugadores.gameObject.SetActive(true);
        ActualizarListado();
     
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
         Listajugadores = ObtenerListaJugadores(); // Obtener la lista actualizada
        Debug.Log("Nuevo jugador: " + newPlayer.NickName);
        Listajugadores.Add(newPlayer.NickName); // Añadir el nuevo jugador a la lista
        EstablecerListaJugadores(Listajugadores); // Establecer la propiedad personalizada

        ActualizarListado(); // Actualizar la lista en la interfaz
    }

    public override void OnJoinedRoom()
    {
        txtLobby.text = "Lobby: " + PhotonNetwork.CurrentRoom.Name; // Mostrar el nombre de la sala

        // Obtener la lista de la propiedad personalizada y mostrarla
        ActualizarListado();
    }
    public void UnirseASala()
    {
        PhotonNetwork.JoinRoom(inputUnirse.text); // Unirse a una sala específica
        txtunirse.gameObject.SetActive(false);

        unirse.gameObject.SetActive(false);
        txtLobby.gameObject.SetActive(true);
        inputUnirse.gameObject.SetActive(false);
        listadoJugadores.gameObject.SetActive(true);
    }
  
    private List<string> ObtenerListaJugadores()
    {
        if (PhotonNetwork.CurrentRoom != null &&
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PropiedadJugadores, out object value))
        {
            Debug.Log("Obteniendo lista de jugadores");
            return new List<string>((string[])value); // Convertir a lista
        }
        return new List<string>(); // Lista vacía si no hay propiedad
    }

    private void EstablecerListaJugadores(List<string> jugadores)
    {
        if (PhotonNetwork.CurrentRoom != null) // Verificar que la sala no es nula
        {
            var customProperties = new ExitGames.Client.Photon.Hashtable(); // Nuevo Hashtable
            customProperties[PropiedadJugadores] = jugadores.ToArray(); // Establecer propiedad personalizada
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties); // Sincronizar
        }
    }

    private void ActualizarListado()
    {
       
            Debug.Log("Actualizando listado de jugadores");

            var listaJugadores = ObtenerListaJugadores(); // Obtener lista actualizada
            listadoJugadores.text = string.Join(", ", listaJugadores); // Mostrar la lista en el texto
        
    }
    [PunRPC]
    public void EmpezarPartida()
    {
       
        panelmenu.SetActive(false);
        paneljuego.SetActive(true);
        paneljuego2.SetActive(true);
        square.SetActive(true);
        funcionbotones.GetComponent<TowerManagement>().partidainiciada = true;
    }
    public void OnEmpezarPartidaButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Llamar al RPC en todos los clientes, incluido el Master Client
            photonView.RPC("EmpezarPartida", RpcTarget.All);
        }
    }
}
