using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scythe : MonoBehaviour
{
    List<IDamageable> toDamage = new List<IDamageable>();

    public player player;
    public SpriteRenderer sr;
    public PolygonCollider2D bc;
    public AnimationCurve curve;
    public AudioClip attack;
   void Start()
{
    player = GetComponentInParent<player>();
    sr = GetComponent<SpriteRenderer>();
    sr.enabled = false;

    bc = GetComponent<PolygonCollider2D>();
    bc.enabled = false;

    transform.localEulerAngles = new Vector3(0, 0, 60);
}

    public IEnumerator swing()
    {
        audioManager.manager.playSFX(attack,player.transform,data.sfx);
        Quaternion start = Quaternion.Euler(0, 0, 60);
        Quaternion end = Quaternion.Euler(0, 0, -60);

        float swingDuration = 1f / player.aspd;
        float windup = swingDuration * 0.25f;
        float attackTime = swingDuration * 0.5f;
        float recovery = swingDuration * 0.25f;

        transform.localRotation = start;
        player.canAttack = false;
        toDamage.Clear();
        sr.enabled = true;

        bc.enabled = false;
        yield return new WaitForSeconds(windup);

        bc.enabled = true;
        float elapsed = 0f;

        while (elapsed < attackTime)
        {
            float t = elapsed / attackTime;
            transform.localRotation = Quaternion.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = end;
        bc.enabled = false;

        yield return new WaitForSeconds(recovery);

        sr.enabled = false;
        transform.localRotation = start;
        player.canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) return;
        if (other.TryGetComponent<IDamageable>(out IDamageable damageable) && !toDamage.Contains(damageable))
        {
            toDamage.Add(damageable);
            damageable.damage(player.atk);

            if (data.fireAspectLvl > 0 && !other.gameObject.TryGetComponent<DoT>(out _))
            {
                var dot = other.gameObject.AddComponent<DoT>();
                dot.damage = (player.atk * 0.2f) * data.fireAspectLvl;
                dot.duration = 3;
                dot.tick = 0.5f;
            }
        }
    }
}
