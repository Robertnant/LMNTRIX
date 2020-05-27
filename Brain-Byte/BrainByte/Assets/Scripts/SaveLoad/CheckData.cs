using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckData : MonoBehaviour
{
    public GameObject soloCharactersSelection;
    public GameObject loadGameMenu;

    public void OnCheckButtonClicked() // singleplayer button
    {
        if (SaveSystem.LoadPayer() != null)
            loadGameMenu.SetActive(true);
        else
            soloCharactersSelection.SetActive(true);
    }
}
