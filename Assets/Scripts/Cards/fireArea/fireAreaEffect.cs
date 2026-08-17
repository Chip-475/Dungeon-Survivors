using System.Collections;
using UnityEngine;

public class fireAreaEffect : cardClass, ICardEffect
{
    public GameObject area;

    IEnumerator spawnArea()
    {
        Instantiate(area, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(10);
        StartCoroutine(spawnArea());
    }
    public void effect()
    {
        StartCoroutine(spawnArea());
        data.fireAreaLvl++;
    }
    public void cardEffect()
    {
        effect();
    }
}
