using UnityEngine;

public class PlayerSpriteAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer targetRenderer;
    [SerializeField] private Rigidbody2D rb;

    [Header("Clips")]
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] runSprites;

    [Header("Directional Clips")]
    [SerializeField] private Sprite[] runUpSprites;
    [SerializeField] private Sprite[] runDownSprites;

    [Header("Timing")]
    [SerializeField] private float idleFramesPerSecond = 8f;
    [SerializeField] private float runFramesPerSecond = 10f;

    private Sprite[] currentSprites;
    private float frameTimer;
    private int frameIndex;

    private const string VerticalSpritesResourcesPath = "PlayerAnimations";
    private const int DirectionalFrameCount = 3;
    private const float DirectionalSpritesPixelsPerUnit = 85f;
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

        if (runUpSprites == null || runUpSprites.Length == 0)
        {
            runUpSprites = CreateDirectionalSprites("player_walk_up");
        }

        if (runDownSprites == null || runDownSprites.Length == 0)
        {
            runDownSprites = CreateDirectionalSprites("player_walk_down");
        }
    }

    private void OnEnable()
    {
        Play(idleSprites);
    }

    private void Update()
    {
        Vector2 velocity = rb != null ? rb.linearVelocity : Vector2.zero;
        bool isMoving = velocity.sqrMagnitude > 0.01f;
        Sprite[] desiredSprites = GetSpritesFor(velocity, isMoving);

        if (desiredSprites != currentSprites)
        {
            Play(desiredSprites);
        }

        Tick(isMoving ? runFramesPerSecond : idleFramesPerSecond);
    }

    private Sprite[] GetSpritesFor(Vector2 velocity, bool isMoving)
    {
        if (!isMoving)
        {
            return idleSprites;
        }

        if (Mathf.Abs(velocity.y) <= Mathf.Abs(velocity.x))
        {
            return runSprites;
        }

        Sprite[] directionalSprites = velocity.y > 0f ? runUpSprites : runDownSprites;
        return directionalSprites != null && directionalSprites.Length > 0 ? directionalSprites : runSprites;
    }

    private Sprite[] CreateDirectionalSprites(string spriteSheetName)
    {
        string resourcePath = $"{VerticalSpritesResourcesPath}/{spriteSheetName}";
        Texture2D spriteSheet = Resources.Load<Texture2D>(resourcePath);
        if (spriteSheet == null)
        {
            Sprite sourceSprite = Resources.Load<Sprite>(resourcePath);
            spriteSheet = sourceSprite != null ? sourceSprite.texture : null;
        }

        if (spriteSheet == null)
        {
            Debug.LogWarning($"Player directional sprite sheet not found at Resources/{resourcePath}.", this);
            return System.Array.Empty<Sprite>();
        }

        spriteSheet.filterMode = FilterMode.Point;
        Sprite[] sprites = new Sprite[DirectionalFrameCount];
        for (int frame = 0; frame < DirectionalFrameCount; frame++)
        {
            int left = Mathf.RoundToInt(frame * spriteSheet.width / (float)DirectionalFrameCount);
            int right = Mathf.RoundToInt((frame + 1) * spriteSheet.width / (float)DirectionalFrameCount);
            Rect frameRect = new Rect(left, 0f, right - left, spriteSheet.height);
            sprites[frame] = Sprite.Create(spriteSheet, frameRect, new Vector2(0.5f, 0f), DirectionalSpritesPixelsPerUnit);
        }

        return sprites;
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
