using System.Collections;
using UnityEngine;

public class camera : MonoBehaviour
{
    private GameObject playerObj;
    private Player player;

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player>();
        StartCoroutine(cameraFollow());
    }
    IEnumerator cameraFollow()
    {
        while (true)
        {
            if (player.isDead == true) break; 
            transform.position = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, -10);
            yield return null;
        }
    }
}
