using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryReference : MonoBehaviour
{
    private static VictoryReference instance;
    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);

        instance = this;
    }

}
