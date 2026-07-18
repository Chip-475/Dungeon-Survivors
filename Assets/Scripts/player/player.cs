using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    #region Declarations
    [Header("Misc")]
    public static Player instance;

    public Rigidbody2D rb;
    public SpriteRenderer sr;

    public GameObject scytheTrf;
    public scythe scythe;

    public GameObject fireArea;

    public GameManager gameManager;
    public hpBar hpBar;

    public AudioSource lowHp;
    public AudioClip deathSound;

    public GameObject icearea;

    public bool isDead;
    public bool canAttack = true;
    public bool canLaunch = true;

    public Vector3 mousePosition;
    public Vector3 mouseWorldPosition;

    [Header("Stats")]
    public float hp;
    public float hpMax;
    public float atk;
    public float baseAtk;
    public float spd;
    public float aspd;
    public float range;
    public bool isInvicible;
    public float iFrames;


    private Vector2 moveInput;
    #endregion

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        scythe = GetComponentInChildren<scythe>();
        scytheTrf.SetActive(true);

        hpBar = GetComponent<hpBar>();

        baseAtk = atk;
    }
    private void Start()
    {
        hpMax = hp;
    }
    void FixedUpdate()
    {
        
        // Mouse Positions Assignment
        mousePosition = Mouse.current.position.ReadValue();
        mouseWorldPosition = new Vector3(Camera.main.ScreenToWorldPoint(mousePosition).x, Camera.main.ScreenToWorldPoint(mousePosition).y, 0);

        // Player Rotation
        var x = mouseWorldPosition.x >= transform.position.x ? transform.localScale = new Vector3(1, 1, 1) : transform.localScale = new Vector3(-1, 1, 1);

        // Player Movement
        rb.linearVelocity = moveInput * spd;

        //sounds
        if (hp <= hpMax / 5) lowHp.volume = Data.sfx;
        else lowHp.volume = 0;
        if (isDead)
        {
            audioManager.manager.playSFX(deathSound, instance.transform, Data.sfx);
            lowHp.volume = 0;
        };
    }

    // Player Controls
    public void move(InputAction.CallbackContext context)
    {
        if(Data.isPaused || isDead == true) return;

        moveInput = context.ReadValue<Vector2>();
    }
    public void attack(InputAction.CallbackContext context)
    {
        if (!context.performed || !canAttack || Data.isPaused || isDead == true) return;

        StartCoroutine(scythe.swing());
    }
    public void togglePause(InputAction.CallbackContext context)
    {
        if (!context.performed || isDead == true) return;

        gameManager.togglePause();
    }

    public void ChangeHealth(float damage)
    {
        if (isInvicible) return;

        hp = Mathf.Clamp(hp, 0, hpMax);
        float nextHp = Mathf.Clamp(hp - damage, 0, hpMax);
        hp = nextHp;
        if (damage > hp)
        {
            if (Random.Range(0f, 10f) == 10f)
            {
                nextHp = 0.1f;
            }
        }

        StartCoroutine(hpBar.hpBarMovement(hp, nextHp));

        if (hp < hpMax * 0.4f && Tenacity.isActive)
        {
            atk = baseAtk * 2;
        }
        else if (Tenacity.isActive)
        {
            atk = baseAtk;
        }

        if (hp <= 0)
        {
            sr.enabled = false;
            isDead = true;
            GameManager.instance.startDeath();
        }
    }
}
