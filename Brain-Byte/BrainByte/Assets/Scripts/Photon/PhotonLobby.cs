using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public static PhotonLobby lobby;

    public GameObject battleButton;
    public GameObject cancelButton;
    public GameObject offlineButton;    //recently added on 26/02/20

    private void Awake()
    {
        lobby = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the " + PhotonNetwork.CloudRegion + " server");
        PhotonNetwork.AutomaticallySyncScene = true;

        //Recently added lines (below) 26/02/20
        offlineButton.GetComponentInChildren<Text>().text = "online";
        offlineButton.GetComponentInChildren<Text>().color = Color.green;

        /*Why GetComponentInChildren is used instead of GetComponent:
         * because the Text object of a button its child*/

        //base.OnConnectedToMaster();       Line automatically added by Visual Studio but not in Tutorial
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("Battle button was clicked");
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join random game but failed. There must be no open games available");
        CreateRoom();
    }

    void CreateRoom() 
        //These line were not added directly to OnJoinRandomFailed just to make the code more visible
    {
        Debug.Log("Trying to create a new Room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte) MultiplayerSettings.multiplayerSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a new room with the same name");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        PhotonNetwork.LeaveLobby();
        cancelButton.SetActive(false);
        //battleButton.SetActive(true);
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
   
}
