using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI; 

public class Boss : EnemyClass
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
        SpawnManager sm = SpawnManager.instance;
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
        float dis = Vector2.Distance(transform.position, Player.instance.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.instance.transform.position - transform.position, dis, GameManager.instance.obstacle);

        if (!timerLockout) skillTimer += Time.deltaTime;

        //Debug.Log("skillTimer: " + skillTimer + " timerLockout " + timerLockout + " dis: " + dis + " hit: " + (hit.collider != null));
        //Debug.Log("FixedUpdate gira, skillTimer: " + skillTimer + " timerLockout: " + timerLockout + " deltaTime: " + Time.deltaTime);
        if (skillTimer > skillCD && dis < info.fovRange && !hit)
        {
            var x = Random.Range(0, 2);
            Debug.Log("Skill scelta: " + (x == 0 ? "dash" : "spawn"));
            if (x == 0) StartCoroutine(dash());
            else StartCoroutine(spawn());

            skillTimer = 0;
            timerLockout = false;
        }

        if(agent.enabled) agent.SetDestination(Player.instance.transform.position);
    }

    public IEnumerator dash()
    {
        Debug.Log("dash iniziata");
        float duration = 1f;
        spriteAnimator?.PlayDash();
        agent.enabled = false;
        transform.DOMove(Player.instance.transform.position, duration);
        yield return new WaitForSeconds(duration);
        agent.enabled = true;

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
        List<Vector3> spawnPoints = new List<Vector3>();
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
            Vector3 spawnPoint = points[i].position;
            spawnPoint = ClampToMapBounds(spawnPoint);
            if (Physics2D.OverlapCircle(spawnPoint, 0.5f, GameManager.instance.obstacle)) spawnPoint = findFreePosition(spawnPoint);
            spawnPoints.Add(spawnPoint);
            StartCoroutine(playSpawnAnimation(spawnPoint));
        }
        // Debug.Log("dopo il for");
        if(spawnAnimationSprites!=null&& spawnAnimationSprites.Length>0)
        {
            float totalAnimTime = spawnAnimationSprites.Length / Mathf.Max(1f, spawnAnimationFPS);
            yield return new WaitForSeconds(totalAnimTime);
        }
        foreach(Vector3 spawnPoint in spawnPoints)
        {
            if (enemyToSpawn != null)
            {
                Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
                SpawnManager.enemyCount++;
            }
            else Debug.Log("nemico non spawnato");
        }
        audioManager.manager.playSFX(spawnSound, transform, Data.sfx);
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
            if (!Physics2D.OverlapCircle(prova, 0.5f, GameManager.instance.obstacle)) return prova;
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

    protected new void OnCollisionEnter2D(Collision2D collision)
    {
        return;
    }
    private void OnCollisionStay2D(Collision2D collision) 
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Player.instance.ChangeHealth(info.atk);
    }
}