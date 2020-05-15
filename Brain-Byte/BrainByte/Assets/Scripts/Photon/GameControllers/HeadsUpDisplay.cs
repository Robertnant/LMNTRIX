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

    private PhotonView PV;
    public bool valsSet = false;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if (PV.IsMine)
        {
            healthBar.SetHealth(playerHealth);
        }
    }
    public void WasHit()
    {
        Debug.Log("PV is not recognized as mine " + PV.ViewID + " but still gonna try to set damage");
        PV.RPC("TakeDamage", RpcTarget.All);
    }

    [PunRPC]
    void TakeDamage()
    {
        playerHealth -= playerHealth - 25 <= 0 ? playerHealth : 25;
        healthBar.SetHealth(playerHealth);
        Debug.Log($"Enemy's remote health is now: {playerHealth}");
    }

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

}
