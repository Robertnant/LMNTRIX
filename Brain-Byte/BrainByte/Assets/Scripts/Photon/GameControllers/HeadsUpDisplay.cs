using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeadsUpDisplay : MonoBehaviourPunCallbacks, IPunObservable
{
    public int playerHealth;
    public int maxHealth;
    public int playerDamage;
    public HealthBar healthBar;
    public bool valsSet = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerHealth);
            Debug.Log("I am the local client: " + GetComponent<PhotonView>().ViewID);
        }
        else
        {
            playerHealth = (int)stream.ReceiveNext();
            Debug.Log("I am the remote client: " + GetComponent<PhotonView>().ViewID);
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
}
