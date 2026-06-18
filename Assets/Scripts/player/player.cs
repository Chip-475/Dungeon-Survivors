using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class player : MonoBehaviour, IDamageable
{
    [Header("Misc")]
    public static player playerInstance;

    public Rigidbody2D rb;
    public SpriteRenderer sr;

    public GameObject scytheTrf;
    public scythe scythe;

    public GameObject fireArea;

    public gameManager gameManager;
    public hpBar hpBar;
    public xpBar xpBar;

    public AudioSource lowHp;
    public AudioClip deathSound;

    public GameObject icearea;

    public bool isDead;
    public bool canAttack = true;
    public bool canLaunch = true;
    public bool onTenacity = false;

    public Vector3 mousePosition;
    public Vector3 mouseWorldPosition;

    [Header("Stats")]
    public float hp;
    public float hpMax;
    public float atk;
    public float spd;
    public float aspd;
    public float range;

    private Vector2 moveInput;

    private void Start()
    {
        playerInstance = this;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        scythe = GetComponentInChildren<scythe>();
        scytheTrf.SetActive(true);

        hpBar = GetComponent<hpBar>();
        xpBar = GetComponent<xpBar>();

        hpMax = hp;
    }
    void FixedUpdate()
    {
        hp = Mathf.Clamp(hp, 0, hpMax);
        // Mouse Positions Assignment
        mousePosition = Mouse.current.position.ReadValue();
        mouseWorldPosition = new Vector3(Camera.main.ScreenToWorldPoint(mousePosition).x, Camera.main.ScreenToWorldPoint(mousePosition).y, 0);

        // Player Rotation
        var x = mouseWorldPosition.x >= transform.position.x ? transform.localScale = new Vector3(1, 1, 1) : transform.localScale = new Vector3(-1, 1, 1);

        // Player Movement
        rb.linearVelocity = moveInput * spd;

        //sounds
        if (hp <= hpMax / 5) lowHp.volume = data.sfx;
        else lowHp.volume = 0;
        if (isDead) audioManager.manager.playSFX(deathSound, player.playerInstance.transform, data.sfx);
    }

    // Player Controls
    public void move(InputAction.CallbackContext context)
    {
        if(data.isPaused || isDead == true) return;

        moveInput = context.ReadValue<Vector2>();
    }
    public void attack(InputAction.CallbackContext context)
    {
        if (!context.performed || !canAttack || data.isPaused || isDead == true) return;

        StartCoroutine(scythe.swing());
    }
    public void togglePause(InputAction.CallbackContext context)
    {
        if (!context.performed || isDead == true) return;

        gameManager.togglePause();
    }

    // Couroutines
    

    // Player Misc
    public void onDamaged(float damage)
    {
        float currentHp = hp;
        float nextHp = hp - damage;

        if(damage > hp)
        {
            if(Random.Range(0f, 10f) == 10f)
            {
                nextHp = 0.1f;
            }
        }

        StartCoroutine(hpBar.hpBarMovement(currentHp, nextHp));

        if (hp < hpMax * 0.3f && !onTenacity && tenacityEffect.instance.tenacity)
        {
            onTenacity = true;
            atk *= 2;
        }
        else if(onTenacity && tenacityEffect.instance.tenacity)
        {
            onTenacity = false;
            atk /= 2;
        }
        if (hp <= 0)
        { 
            sr.enabled = false;
            isDead = true;
            gameManager.instance.startDeath();
            
        }
    }

    // Interface Methods
    public void damage(float damage)
    {
        onDamaged(damage);
    }
}
