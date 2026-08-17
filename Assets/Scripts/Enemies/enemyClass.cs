using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyClass : MonoBehaviour, IDamageable
{
    #region Declarations
    public Rigidbody2D rb;
    public Collider2D collider_;
    public NavMeshAgent agent;
    public EnemySpriteAnimator spriteAnimator;

    [SerializeField] EnemyMeta baseInfo;
    public EnemyMeta info;
    public AnimationCurve hpBarCurve;

    protected bool inRange;
    protected bool detecting;
    private Vector3 _baseScale;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        info = Instantiate(baseInfo);

        rb = GetComponent<Rigidbody2D>();
        collider_ = GetComponent<Collider2D>();
        agent = GetComponent<NavMeshAgent>();
        spriteAnimator = GetComponent<EnemySpriteAnimator>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = info.spd;
        _baseScale = transform.localScale;
    }
    protected virtual void Start()
    {
        info.hp = info.hpMax;

        if (Swarm.isActive)
        {
            info.hpMax /= 2;
            Mathf.Clamp(info.hp, 0, info.hpMax);
        }
    }
    protected virtual void FixedUpdate()
    {
        var playerPosition = Player.instance.gameObject.transform.position;

        if(playerPosition.x >= transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(_baseScale.x),_baseScale.y,1);
        }else
        {
            transform.localScale = new Vector3(-Mathf.Abs(_baseScale.x), _baseScale.y, 1);
        }
    }
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Player.instance.ChangeHealth(info.hp);
        Destroy(gameObject); 
    }
    protected virtual void OnDestroy()
    {
        Data.killCount++;
        SpawnManager.enemyCount--;
        Data.xpQueue.Enqueue(info.xpGiven);
        XpBar xpBar = Player.instance?.GetComponent<XpBar>();
        if (xpBar != null &&!xpBar.queueing) xpBar.StartXpGain();
        // Heal every 10 kills
        if(Data.killCount % 10 == 0)
        {
            float newHp = Mathf.Clamp(Player.instance.hp + 10f, 0, Player.instance.hpMax);
            //player.playerInstance.hp = newHp;
            Player.instance.StartCoroutine(Player.instance.hpBar.hpBarMovement(Player.instance.hp, newHp));
        }

        // death sfx
    }
    #endregion

    public void ChangeHealth(float damage)
    {
        info.hp -= damage;
        info.hp = Mathf.Clamp(info.hp, 0, info.hpMax);

        if (info.hp == 0) Destroy(gameObject);
    }
}