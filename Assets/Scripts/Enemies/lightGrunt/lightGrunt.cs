using UnityEngine;

public class lightGrunt : enemyClass
{
    private new void Start()
    {
        base.Start();

    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();

        _agent.SetDestination(playerObj.transform.position);
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
