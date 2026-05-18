using UnityEngine;

[RequireComponent (typeof(CircleCollider2D))]
public class fireArea : cardClass
{
    public CircleCollider2D circleCollider;

    protected new void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        Destroy(gameObject, duration);

        transform.localScale = new Vector2(player.playerInstance.range * 5, player.playerInstance.range * 5);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && !collision.gameObject.TryGetComponent<DoT>(out _))
        {
            DoT dot = collision.gameObject.AddComponent<DoT>();
            dot.damage = player.playerInstance.atk * data.fireAreaLvl / 5;
            dot.duration = 5f;
            dot.tick = 1 / 3f;
        }
    }
}
