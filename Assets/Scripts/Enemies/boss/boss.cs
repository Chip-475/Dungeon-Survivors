using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI; 

public class boss : enemyClass
{
    public Image hpBar;
    public AudioClip spawnSound;
    public List<Transform> points = new();
    public GameObject enemyToSpawn;
    public Sprite[] spawnAnimationSprites;
    public float spawnAnimationFPS = 8f;
    public float spawnAnimationScale = 1f;

    public float skillTimer;
    public float skillCD;
    public bool timerLockout;

    private bool isDamaging=false;
    new void Start()
    {
        base.Start();
        skillTimer = 0;
        timerLockout = false;
        hpBar.fillAmount = 1f;
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
        spriteAnimator?.PlayDash();
        _agent.enabled = false;
        transform.DOMove(player.transform.position, duration);
        yield return new WaitForSeconds(duration);
        _agent.enabled = true;

        timerLockout = false;
    }
    public IEnumerator spawn()
    {
        spriteAnimator?.PlaySummon();
        foreach (var point in points)
        {
            StartCoroutine(playSpawnAnimation(point.position));
        }

        if (spawnAnimationSprites != null && spawnAnimationSprites.Length > 0)
        {
            float frameDuration = 1f / Mathf.Max(1f, spawnAnimationFPS);
            yield return new WaitForSeconds(frameDuration * spawnAnimationSprites.Length);
        }

        foreach(var point in points)
        {
            Instantiate(enemyToSpawn, point.position, Quaternion.identity);
            spawnManager.enemyCount++;
            
        }
        audioManager.manager.playSFX(spawnSound, transform, data.sfx);

        timerLockout = false;
    }

    private IEnumerator playSpawnAnimation(Vector3 position)
    {
        if (spawnAnimationSprites == null || spawnAnimationSprites.Length == 0)
        {
            yield break;
        }

        GameObject spawnEffect = new("Spawn Ranged Skeleton Animation");
        spawnEffect.transform.position = position;
        spawnEffect.transform.localScale = Vector3.one * spawnAnimationScale;

        SpriteRenderer spriteRenderer = spawnEffect.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;

        float frameDuration = 1f / Mathf.Max(1f, spawnAnimationFPS);
        foreach (Sprite sprite in spawnAnimationSprites)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(frameDuration);
        }

        Destroy(spawnEffect);
    }
    /*
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collisione boss"+collision.gameObject.name+collision.gameObject.tag+collision.gameObject.layer);
        if (!collision.gameObject.CompareTag("Player")) return;
        //Debug.Log("Palyer trovato");
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Debug.Log("posso applicare il danno");
            damageable.damage(5f);
        }
        else Debug.Log("danno no " + collision.gameObject.name);
    }
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger con: " + other.gameObject.name);
        if (!other.gameObject.CompareTag("Player")) return;
        /*
        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.damage(5f);
            lastDamageTime = Time.time;
        }*/
        if (!isDamaging)
        {
            isDamaging = true;
            StartCoroutine(damageOverTime(other));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))isDamaging=false;
    }

    private IEnumerator damageOverTime(Collider2D other)
    {
        while (isDamaging)
        {
            if (other.TryGetComponent<IDamageable>(out IDamageable damageable)) damageable.damage(5f);
            yield return new WaitForSeconds(1f);
        }
    }
    public override void damage(float damage)
    {
        base.damage(damage);
        hpBar.fillAmount = hp / hpMax;
    }
}
