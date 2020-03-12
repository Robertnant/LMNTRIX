using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloAvatarSetup : MonoBehaviour
{
    public GameObject myCharacter;
    public int characterValue;

    void Start()
    {
        characterValue = PlayerInfo.PI.selectedCharacter;
        myCharacter = Instantiate(PlayerInfo.PI.allCharacters[PlayerInfo.PI.selectedCharacter], transform.position, transform.rotation,
            transform);
    }

}
