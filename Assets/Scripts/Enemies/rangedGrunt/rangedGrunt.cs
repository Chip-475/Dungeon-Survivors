using Unity.VisualScripting;
using UnityEngine;

public class rangedGrunt : enemyClass
{
    public GameObject projectile;
    public Transform shootPoint;
    public float shootCD = 0;
    private float sinceShoot;
    private bool canShoot;

    private new void Start()
    {
        base.Start();
        canShoot = false;
        sinceShoot = 0;
    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();
        float dis = Vector2.Distance(transform.position, playerObj.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerObj.transform.position - transform.position, dis, gameManager.instance.obstacle);
        if(_agent!=null)_agent.SetDestination(transform.position);
        if ( dis > 10)
        {
            canShoot = false;
        }
        else if(dis < 10 && !hit) 
        {
            if(_agent!=null)_agent.SetDestination(transform.position);
            canShoot = true; 
        }

        if (canShoot && sinceShoot >= shootCD) shoot();
        sinceShoot += Time.deltaTime;
    }

    public void shoot()
    {
        Instantiate(projectile, shootPoint.position, Quaternion.identity, transform);
        sinceShoot = 0;
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
