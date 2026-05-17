using System.Collections.Generic;
using UnityEngine;

public class bladesMovement : cardClass, ICardEffect
{
    public List<GameObject> bladesObj = new List<GameObject>();

    private new void Start()
    {
        bladesObj[0].transform.localScale = Vector3.zero;
        bladesObj[1].transform.localScale = Vector3.zero;
    }

    private void LateUpdate()
    {
        transform.position = player.playerInstance.transform.position;
        transform.Rotate(new Vector3(0, 0, 90 * Time.deltaTime));
    }

    public void effect()
    {
        bladesObj[0].transform.localScale = Vector3.one * 2;
        bladesObj[1].transform.localScale = Vector3.one * 2;
    }
    public void cardEffect()
    {
        effect();
    }
}
