using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom room;
    private PhotonView PV;

    //public bool isGameLoaded; //For delay start mechanics
    public int currentScene;
    public int multiplayerScene;

    //Player attributes
    Player[] photonPlayers;

    private void Awake()
    {
        //initialize singleton room
        if (PhotonRoom.room == null)
            PhotonRoom.room = this;
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
        //The game object will not be destroyed on load of new scene
    }

    // Start is called before the first frame update

    void Start()
    {
        PV = GetComponent<PhotonView>();
        //Can also be put directly in Awake() function at very end
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);

        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are now in a room");

        StartGame();
    }

    void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        Debug.Log("Starting game (Loading level)");
        PhotonNetwork.LoadLevel(multiplayerScene);
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;   //index of scene in Build settings

        if(currentScene == multiplayerScene)
            CreatePlayer();
    }

    private void CreatePlayer()
    {
        //creates player network controller and not player character
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"),
            transform.position, Quaternion.identity, 0);

        // Recent change 21/04/20: "PhotonNetwork.Instantiate -> PhotonNetwork.InstantiateSceneObject
        // Tuto Info Gamer 11

        /*Quaternion: is used to represent rotations (it is used because the
         * 3rd parameter of PhotonNetwork.Instantiate must be a rotation)*/
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + "has left the game");
        // Check video Info Gamer Part 11 for missing elements
    }

}
