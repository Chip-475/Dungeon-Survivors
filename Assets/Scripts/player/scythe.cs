using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scythe : MonoBehaviour
{
    List<IDamageable> toDamage = new List<IDamageable>();

    public player player;
    public SpriteRenderer sr;
    public BoxCollider2D bc;
    public AnimationCurve curve;
    
    void Start()
    {
        player = GetComponentInParent<player>();
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;

        transform.localEulerAngles = new Vector3(0, 0, 45);
    }

    public IEnumerator swing()
    {
        Quaternion start = Quaternion.Euler(0, 0, 45);
        Quaternion destination = Quaternion.Euler(0, 0, -45);

        curve = AnimationCurve.EaseInOut(0, 0, player.aspd / 2, 180);
        curve.preWrapMode = WrapMode.PingPong;
        curve.postWrapMode = WrapMode.PingPong;

        player.canAttack = false;
        toDamage.Clear();

        sr.enabled = true;

        bc.enabled = false;
        yield return new WaitForSeconds(player.aspd / 2);

        // Swing
        float time = 0;
        bc.enabled = true;
        while (time < player.aspd)
        {
            var step = curve.Evaluate(time) * Time.deltaTime;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, destination, step);

            yield return null;
            time += Time.deltaTime;
        }
        bc.enabled = false;

        yield return new WaitForSeconds(player.aspd / 2);   
        
        sr.enabled = false;
        transform.localEulerAngles = new Vector3(0, 0, 45);
        toDamage.Clear();

        player.canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
