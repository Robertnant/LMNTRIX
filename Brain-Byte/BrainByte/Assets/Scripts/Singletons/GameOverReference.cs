using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverReference : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameOverReference instance;
    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);

        instance = this;
    }
}
