/*using UnityEngine;

public class PlayerSpriteAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer targetRenderer;
    [SerializeField] private Rigidbody2D rb;

    [Header("Clips")]
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] runSprites;

    [Header("Timing")]
    [SerializeField] private float idleFramesPerSecond = 8f;
    [SerializeField] private float runFramesPerSecond = 10f;

    private Sprite[] currentSprites;
    private float frameTimer;
    private int frameIndex;
    private bool wasMoving;

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<SpriteRenderer>();
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void OnEnable()
    {
        PlayIdle();
    }

    private void Update()
    {
        bool isMoving = rb != null && rb.linearVelocity.sqrMagnitude > 0.01f;
        if (isMoving != wasMoving)
        {
            if (isMoving)
            {
                PlayRun();
            }
            else
            {
                PlayIdle();
            }

            wasMoving = isMoving;
        }

        Tick(isMoving ? runFramesPerSecond : idleFramesPerSecond);
    }

    private void PlayIdle()
    {
        Play(idleSprites);
    }

    private void PlayRun()
    {
        Play(runSprites);
    }

    private void Play(Sprite[] sprites)
    {
        if (targetRenderer == null || sprites == null || sprites.Length == 0)
        {
            return;
        }

        currentSprites = sprites;
        frameIndex = 0;
        frameTimer = 0f;
        targetRenderer.sprite = currentSprites[frameIndex];
    }

    private void Tick(float framesPerSecond)
    {
        if (targetRenderer == null || currentSprites == null || currentSprites.Length == 0)
        {
            return;
        }

        frameTimer += Time.deltaTime;
        float frameDuration = 1f / Mathf.Max(1f, framesPerSecond);
        if (frameTimer < frameDuration)
        {
            return;
        }

        frameTimer -= frameDuration;
        frameIndex = (frameIndex + 1) % currentSprites.Length;
        targetRenderer.sprite = currentSprites[frameIndex];
    }
}
*/

