using UnityEngine;

public class EnemySpriteAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer targetRenderer;

    [Header("Clips")]
    [SerializeField] private Sprite[] walkSprites;
    [SerializeField] private Sprite[] attackSprites;
    [SerializeField] private Sprite[] dashSprites;
    [SerializeField] private Sprite[] summonSprites;

    [Header("Timing")]
    [SerializeField] private float framesPerSecond = 8f;

    private Sprite[] currentSprites;
    private float frameTimer;
    private int frameIndex;
    private bool returnToWalk;

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void OnEnable()
    {
        PlayWalk();
    }

    private void Update()
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
        frameIndex++;

        if (frameIndex >= currentSprites.Length)
        {
            if (returnToWalk)
            {
                PlayWalk();
                return;
            }

            frameIndex = 0;
        }

        targetRenderer.sprite = currentSprites[frameIndex];
    }

    public void PlayWalk()
    {
        PlayLoop(walkSprites);
    }

    public void PlayAttack()
    {
        PlayOnce(attackSprites);
    }

    public void PlayDash()
    {
        PlayOnce(dashSprites);
    }

    public void PlaySummon()
    {
        PlayOnce(summonSprites);
    }

    private void PlayLoop(Sprite[] sprites)
    {
        if (!HasSprites(sprites))
        {
            return;
        }

        currentSprites = sprites;
        frameIndex = 0;
        frameTimer = 0f;
        returnToWalk = false;
        targetRenderer.sprite = currentSprites[frameIndex];
    }

    private void PlayOnce(Sprite[] sprites)
    {
        if (!HasSprites(sprites))
        {
            return;
        }

        currentSprites = sprites;
        frameIndex = 0;
        frameTimer = 0f;
        returnToWalk = true;
        targetRenderer.sprite = currentSprites[frameIndex];
    }

    private static bool HasSprites(Sprite[] sprites)
    {
        return sprites != null && sprites.Length > 0;
    }
}
