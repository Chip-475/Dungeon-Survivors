using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;

public class boss : enemyClass
{
    public List<Transform> points = new();
    public GameObject enemyToSpawn;

    public float skillTimer;
    public float skillCD;
    public bool timerLockout;

    new void Start()
    {
        base.Start();
        skillTimer = 0;
        timerLockout = false;
    }
    new void FixedUpdate()
    {
        base.FixedUpdate();
        float dis = Vector2.Distance(transform.position, playerObj.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerObj.transform.position - transform.position, dis, gameManager.instance.obstacle);

        if (!timerLockout) skillTimer += Time.deltaTime;

        if (skillTimer > skillCD && dis < fovRange && !hit)
        {
            var x = Random.Range(0, 2);
            if (x == 0) StartCoroutine(dash());
            else StartCoroutine(spawn());

            skillTimer = 0;
            timerLockout = true;
        }

        if(_agent.enabled) _agent.SetDestination(playerObj.transform.position);
    }

    public IEnumerator dash()
    {
        float duration = 1f;
        _agent.enabled = false;
        transform.DOMove(player.transform.position, duration);
        yield return new WaitForSeconds(duration);
        _agent.enabled = true;

        timerLockout = false;
    }
    public IEnumerator spawn()
    {
        foreach(var point in points)
        {
            Instantiate(enemyToSpawn, point.position, Quaternion.identity);
        }

        yield return null;
        timerLockout = false;
    }
}
