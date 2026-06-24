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
     [Header("corners")]
    public Transform topRight;
    public Transform topLeft;
    public Transform bottomRight;
    public Transform bottomLeft;

    public float skillTimer;
    public float skillCD;
    public bool timerLockout;

    private List<Vector3> puntiFissi = new();
    new void Start()
    {
        base.Start();
        skillTimer = 0;
        timerLockout = false;
        hpBar.fillAmount = 1f;
        puntiFissi.Clear();
        spawnManager sm = FindObjectOfType<spawnManager>();
        topRight = sm.topRight;
        bottomRight = sm.bottomRight;
        bottomLeft = sm.bottomLeft;
        topLeft= sm.topLeft;
        foreach(var p in points)
        {
            puntiFissi.Add(p.localPosition);
        }
        if (puntiFissi.Count != points.Count) Debug.Log("i punti non coincidono");
    }
    new void FixedUpdate()
    {
        base.FixedUpdate();
        float dis = Vector2.Distance(transform.position, playerObj.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerObj.transform.position - transform.position, dis, gameManager.instance.obstacle);

        if (!timerLockout) skillTimer += Time.deltaTime;

        //Debug.Log("skillTimer: " + skillTimer + " timerLockout " + timerLockout + " dis: " + dis + " hit: " + (hit.collider != null));
        //Debug.Log("FixedUpdate gira, skillTimer: " + skillTimer + " timerLockout: " + timerLockout + " deltaTime: " + Time.deltaTime);
        if (skillTimer > skillCD && dis < fovRange && !hit)
        {
            var x = Random.Range(0, 2);
            Debug.Log("Skill scelta: " + (x == 0 ? "dash" : "spawn"));
            if (x == 0) StartCoroutine(dash());
            else StartCoroutine(spawn());

            skillTimer = 0;
            timerLockout = false;
        }

        if(_agent.enabled) _agent.SetDestination(playerObj.transform.position);
    }

    public IEnumerator dash()
    {
        Debug.Log("dash iniziata");
        float duration = 1f;
        spriteAnimator?.PlayDash();
        _agent.enabled = false;
        transform.DOMove(player.transform.position, duration);
        yield return new WaitForSeconds(duration);
        _agent.enabled = true;

        timerLockout = false;
        Debug.Log("finito con timerLockaut " + timerLockout);
    }
    public IEnumerator spawn()
    {
        /*
        spriteAnimator?.PlaySummon();
        foreach (var point in points)
        {
            Vector3 spawnPoint= new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0f);
            spawnPoint = ClampToMapBounds(spawnPoint);//cosi limita dentro la mappa
            if (Physics2D.OverlapCircle(spawnPoint, 0.5f, gameManager.instance.obstacle)) spawnPoint = findFreePosition(spawnPoint);
            point.position = spawnPoint;
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
        }*/
        Debug.Log("spawn nemici");
        if(puntiFissi.Count!=points.Count)
        {
            Debug.Log("punti fiss non coincisiono in spwn");
            timerLockout = false;
            yield break;
        }
        spriteAnimator?.PlaySummon();
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 spawnPoint = transform.position + puntiFissi[i];
            spawnPoint = ClampToMapBounds(spawnPoint);
            if (Physics2D.OverlapCircle(spawnPoint, 0.5f, gameManager.instance.obstacle)) spawnPoint = findFreePosition(spawnPoint);
            StartCoroutine(playSpawnAnimation(spawnPoint));
            if (enemyToSpawn != null)
            {
                Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
                spawnManager.enemyCount++;
            }
            else Debug.Log("enemy � null");
        }
        Debug.Log("dopo il for");
        audioManager.manager.playSFX(spawnSound, transform, data.sfx);
        timerLockout = false;
        Debug.Log("finito con " + timerLockout);
        yield return null;
    }

    private Vector3 ClampToMapBounds(Vector3 pos)
    {
        if (topLeft == null || topRight == null || bottomLeft == null || bottomRight == null)
        {
            Debug.Log("un angolo � null");
            return pos;
        }
        float minX = bottomLeft.position.x;
        float maxX=bottomRight.position.x;
        float minY = bottomLeft.position.y;
        float maxY = topLeft.position.y;
        float clampedX=Mathf.Clamp(pos.x,minX, maxX);
        float clampedY=Mathf.Clamp(pos.y,minY, maxY);   
        return new Vector3(clampedX, clampedY,0f);
    }

    private Vector3 findFreePosition(Vector3 orgin)
    {
        for(int i=0;i<20;i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 2f;
            Vector3 prova=orgin+new Vector3(randomOffset.x,randomOffset.y,0);
            prova = ClampToMapBounds(prova);
            if (!Physics2D.OverlapCircle(prova, 0.5f, gameManager.instance.obstacle)) return prova;
        }
        return orgin;
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
   
    private void OnCollisionStay2D(Collision2D other) 
    {
        //Debug.Log("Trigger con: " + other.gameObject.name);
        if (!other.gameObject.CompareTag("Player")) return;
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.damage(atk);
        }
    }
   
    public override void damage(float damage)
    {
        base.damage(damage);
        hpBar.fillAmount = hp / hpMax;
    }
}