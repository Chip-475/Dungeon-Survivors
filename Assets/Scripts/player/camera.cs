using UnityEngine;

public class camera : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        // Player Fetch
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        // Camera Follow
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);

        //<3| >--------<| <3
    }
}
