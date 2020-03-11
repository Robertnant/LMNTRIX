using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnCollisionEnter()
    {
        Debug.Log("Object was hit");
    }
}
