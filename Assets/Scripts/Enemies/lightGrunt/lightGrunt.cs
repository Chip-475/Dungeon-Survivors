using UnityEngine;

public class lightGrunt : EnemyClass
{
    private new void Start()
    {
        base.Start();

    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();

        agent.SetDestination(Player.instance.transform.position);
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
    private new void OnDestroy()
    {
        base.OnDestroy();
    }
}
