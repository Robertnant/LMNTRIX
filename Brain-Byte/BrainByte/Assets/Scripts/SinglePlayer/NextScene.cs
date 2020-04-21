using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public GameObject avatar;
    public string singlePlayerScene;
    
    public void LoadSinglePlayer()
    {
        SceneManager.LoadScene(singlePlayerScene);
    }

}
