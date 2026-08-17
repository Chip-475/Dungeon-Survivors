using System.Collections;
using UnityEngine;

public class rangedGruntProj : MonoBehaviour
{
    IDamageable IDamageable;

    public float spd;
    private float atk;
    private Rigidbody2D self;
    private rangedGrunt parent;
    private Vector3 dir;

    private void Start()
    {
        Destroy(gameObject, 10);
        
        self = GetComponent<Rigidbody2D>();
        parent = transform.parent.GetComponent<rangedGrunt>();

        dir = ((Vector2)(parent.playerObj.transform.position) - self.position).normalized;
        atk = parent.atk;

        transform.SetParent(null, true);

        self.linearVelocity = new Vector2(spd * dir.x, spd * dir.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Obstacle" || other.gameObject.tag == "Player")
        {
            if (other.TryGetComponent<IDamageable>(out IDamageable))
            {
                other.GetComponent<IDamageable>().damage(atk);
            }
            Destroy(gameObject);
        }
    }
}
