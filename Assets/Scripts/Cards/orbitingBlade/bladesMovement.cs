using System.Collections.Generic;
using UnityEngine;

public class bladesMovement : cardClass, ICardEffect
{
    public List<GameObject> bladesObj = new List<GameObject>();

    private new void Start()
    {
        bladesObj[0].SetActive(false);
        bladesObj[1].SetActive(false);
    }

    private void LateUpdate()
    {
        transform.position = Player.instance.transform.position;
        transform.Rotate(new Vector3(0, 0, 90 * Time.deltaTime));
        bladesObj[0].transform.Rotate(new Vector3(0, 0, 720 * Time.deltaTime));
        bladesObj[1].transform.Rotate(new Vector3(0, 0, 720 * Time.deltaTime));
    }

    public void effect()
    {
        bladesObj[0].SetActive(true);
        bladesObj[1].SetActive(true);
    }
    public void CardEffect()
    {
        effect();
    }
}
