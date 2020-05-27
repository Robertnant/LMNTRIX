using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGameReference : MonoBehaviour
{
    private static FinishGameReference instance;
    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);

        instance = this;
    }

}
