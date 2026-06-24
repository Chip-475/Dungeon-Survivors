using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public abstract class enemyClass : MonoBehaviour, IDamageable
{
    [Header("Meta Data")]

    IDamageable IDamageable;
    public GameObject playerObj;
    public player player;
    public xpBar xpBar;
    public AudioClip deathSound;
    public AudioClip bossDeathSound;
    protected Rigidbody2D prb;
    protected Rigidbody2D rb;
    protected Collider2D _collider;
    protected NavMeshAgent _agent;
    protected EnemySpriteAnimator spriteAnimator;
    public AnimationCurve hpBarCurve;

    protected bool inRange;
    protected bool detecting;

    [Header("Stats")]
    [SerializeField] public float hp;
    public float hpMax;
    public float xpGiven;
    [SerializeField] public float atk;
    [SerializeField] public float spd;

    public float fovRange;
    [Range(0, 360)] public float fovAngle;

    private Vector3 _baseScale;
    // Virtuals
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<player>();
        xpBar = playerObj.GetComponent<xpBar>();
        prb = playerObj.GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _agent = GetComponent<NavMeshAgent>();
        spriteAnimator = GetComponent<EnemySpriteAnimator>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        if (swarmEffect.swarm)
        {
            hp /= 2;
        }
        hpMax = hp;
        _baseScale = transform.localScale;
    }
    protected virtual void FixedUpdate()
    {
        if (playerObj.transform.position.x < transform.position.x) transform.localScale = new Vector3(-Mathf.Abs(_baseScale.x), _baseScale.y, _baseScale.z);
        else transform.localScale = new Vector3(Mathf.Abs(_baseScale.x), _baseScale.y, _baseScale.z);
        
        /*if (playerObj.transform.position.x < transform.position.x) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = new Vector3(1, 1, 1);
        */
        _agent.speed = spd;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable))
        {
            collision.gameObject.GetComponent<IDamageable>().damage(atk);
            xpGiven=0;
            
        }
        if(gameObject.CompareTag("Boss"))
        {
            return;
        } 
            Destroy(gameObject);
        
    }
    protected virtual void OnDestroy()
    {
        data.killCount++;
        print(data.killCount);
        spawnManager.enemyCount--;
        data.xpQueue.Enqueue(xpGiven);
        if(!xpBar.queueing) xpBar.startMedium();
        //ogni 1o kill recuperi 10 di vita
        if(data.killCount%10==0)
        {
            float newHp = Mathf.Clamp(player.playerInstance.hp + 10f, 0, player.playerInstance.hpMax);
            //player.playerInstance.hp = newHp;
            player.playerInstance.StartCoroutine(player.playerInstance.hpBar.hpBarMovement(player.playerInstance.hp, newHp));
        }
        Vector3 deathPosition = transform.position;
        bool isBoss = TryGetComponent(out boss _);
        AudioClip clipToPlay= isBoss ? bossDeathSound : deathSound;
        if(audioManager.manager!=null&&clipToPlay!=null)audioManager.manager.playSFXAtPosition(clipToPlay,deathPosition,data.sfx);
        /*
        if(TryGetComponent(out boss _))
        {
            audioManager.manager.playSFX(bossDeathSound, transform, data.sfx);
        }
        else
        {
            audioManager.manager.playSFX(deathSound, transform, data.sfx);
        }*/
    }


    // Misc
    protected void onDamaged(float damage)
    {
        hp -= damage;
        hp = Mathf.Clamp(hp, 0, hpMax);
        if (hp == 0) { Destroy(gameObject); return; }
    }
    //protected void detect()
    //{
    //    // To do
    //}

    // Interface Methods

    
    public virtual void damage(float damage)
    {
        onDamaged(damage);
    }
    
}