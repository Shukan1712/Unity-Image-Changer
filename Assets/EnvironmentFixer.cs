using UnityEngine;

public class EnvironmentFixer : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        transform.position = new Vector3(0, -20, 0) + player.transform.position;
    }
}
