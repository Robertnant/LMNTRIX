using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public GameObject avatar;
    public void OnClick()
    {
        SceneManager.LoadScene(2);  //In the future, create variable for nextSceneIndex and use it here
        /*Instantiate(avatar, avatar.transform.position, avatar.transform.rotation,
            avatar.transform);*/
    }

}
