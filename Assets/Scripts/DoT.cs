using System.Collections;
using UnityEngine;

public class DoT : MonoBehaviour
{
    IDamageable damageable;

    public float damage;
    public float duration;
    public float tick;

    private void Start()
    {
        damageable = GetComponent<IDamageable>();
        StartCoroutine(tickDamage());
    }

    IEnumerator tickDamage()
    {
        for(int i = (int)(duration / tick); i >= 0; i--)
        {
            damageable.damage(damage);
            yield return new WaitForSeconds(tick);
        }
        Destroy(this);
    }
}
