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

        dir = ((Vector2)(Player.instance.transform.position) - self.position).normalized;
        atk = parent.info.atk;
        //parte della freccia per essere dritta
        transform.SetParent(null, true);
        transform.localScale = Vector3.one;
        float angle=Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        Debug.Log("Angolo calcolato: " + angle);
        transform.rotation=Quaternion.Euler(0,0,angle-135f);

        transform.SetParent(null, true);

        self.linearVelocity = new Vector2(spd * dir.x, spd * dir.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Obstacle" || other.gameObject.tag == "Player")
        {
            if (other.TryGetComponent<IDamageable>(out IDamageable))
            {
                other.GetComponent<IDamageable>().ChangeHealth(atk);
            }
            Destroy(gameObject);
        }
    }
}
