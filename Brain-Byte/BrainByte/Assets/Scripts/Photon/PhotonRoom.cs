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

    public bool isGameLoaded; //For delay start mechanics
    public int currentScene;
    private LevelLoader levelLoader;

    //Player attributes: recently changed 07/05/20
    private Photon.Realtime.Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;
    public int playerInGame;

    // Deals with time before start of game
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    // New: check if player is loading into first map online
    public bool isFirtOnlineLevel = true;

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
        levelLoader = FindObjectOfType<LevelLoader>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
    }

    void Update()
    {
        if (levelLoader == null)
            FindObjectOfType<LevelLoader>();

        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            if(playersInRoom == 1)
            {
                RestartTimer();
            }
            if (!isGameLoaded)
            {
                if (readyToStart)
                {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                }
                else if (readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }

                Debug.Log("Display time to start to the players " + timeToStart);

                if (timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }
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

        // New: 07/05/20

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            Debug.Log("Max in-room players exceeded (" + playersInRoom + ":" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");

            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
            StartGame();

         // End of New

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;

        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            Debug.Log("Max in-room players exceeded (" + playersInRoom + ":" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");

            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    void StartGame()
    {
        isGameLoaded = true;

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        //PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiPlayerScene);
        MultiplayerSettings.multiplayerSettings.isMultiplayer = true;
        levelLoader.LoadLevel(MultiplayerSettings.multiplayerSettings.multiPlayerScene);

        Debug.Log("Starting game (Loading level)");
    }

    void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;   //index of scene in Build settings

        //Debug.Log("Loaded new scene " + currentScene);

        if (currentScene == MultiplayerSettings.multiplayerSettings.multiPlayerScene)    // basically first multiplayer scene
        {
            isGameLoaded = true;
            isFirtOnlineLevel = false;

            if (MultiplayerSettings.multiplayerSettings.delayStart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
            {
                RPC_CreatePlayer();
                /* this stopped working when PhotonNetwork.Instantiate in RPC_CreatePlayer() was
                 * replaced by PhotonNetwork.InstantiateSceneObject. Might need to call the rpc 
                 * on all targets like in the RPC_LoadedGameScene() method.
                 */
            }
        }
        else if (!isFirtOnlineLevel)
        {
            // the if check in else might be unecessary
            // if we're now on any multiplayer scene after the first multiplayer one

            foreach (PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
            {
                if (PV.IsMine)
                {
                    Debug.Log("Trying to spawn players in next multiplayer scene");

                    //PV.RPC("ChangePlayerPosition", RpcTarget.All);
                    player.GetComponent<PhotonView>().RPC("ChangePlayerPosition", RpcTarget.All);
                }
                //player.ChangePlayerPosition();
            }
            
        }
        
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        //creates player network controller and not player character
        /*PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"),
            transform.position, Quaternion.identity, 0);*/
        
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"),
            transform.position, Quaternion.identity, 0);
        
        // Uncomment previous line to try to fix null reference exception bug

        // Recent change 21/04/20: "PhotonNetwork.Instantiate -> PhotonNetwork.InstantiateSceneObject
        // Tuto Info Gamer 11

        /*Quaternion: is used to represent rotations (it is used because the
         * 3rd parameter of PhotonNetwork.Instantiate must be a rotation)*/
    }

    [PunRPC]

    private void RPC_LoadedGameScene()
    {
        playerInGame++;

        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + "has left the game");
        // Check video Info Gamer Part 11 for missing elements
    }

}
