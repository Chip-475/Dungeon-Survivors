using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
public class electroAura : cardClass
{
    
    private List<GameObject> list = new List<GameObject>();

    private void OnEnable()
    {
        active = true;

        transform.localScale = new Vector3(radius * 2, radius * 2, 0);

        StartCoroutine(damage());
    }

    IEnumerator damage()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            foreach (GameObject enemy in list.ToArray())
            {
                if(enemy != null) enemy.GetComponent<IDamageable>().damage(player.atk / 2);
                else list.Remove(enemy);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy")) list.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) list.Remove(collision.gameObject);
    }
}
