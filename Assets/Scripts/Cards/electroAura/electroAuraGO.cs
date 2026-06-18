using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

[RequireComponent(typeof(CircleCollider2D))]
public class electroAuraGO : cardClass, ICardEffect
{
    IEnumerator damage()
    {
        transform.localScale = new Vector2(player.playerInstance.range * 5, player.playerInstance.range * 5);

        var enemiesInRange = Physics2D.OverlapCircleAll(transform.position, player.playerInstance.range * 5 / 2, gameManager.instance.enemy).ToList();

        for(int i = 0; i < enemiesInRange.Count; i++)
        {
            enemiesInRange[i].gameObject.TryGetComponent(out IDamageable x);
            if(x != null) x.damage(player.playerInstance.atk / 3);
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(damage());
    }

    public void effect()
    {
        transform.localScale = new Vector2(player.playerInstance.range * 5, player.playerInstance.range * 5);
        StartCoroutine(damage());
        print("electroAura picked");
    }
    public void cardEffect()
    {
        effect();
    }
}
