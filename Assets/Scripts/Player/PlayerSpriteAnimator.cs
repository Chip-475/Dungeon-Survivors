using UnityEngine;

public class PlayerSpriteAnimator : MonoBehaviour
{
    private enum Direction { Down, Up, Left, Right }

    [Header("References")]
    [SerializeField] private SpriteRenderer targetRenderer;
    [SerializeField] private Rigidbody2D rb;

    [Header("Idle Clips")]
    [SerializeField] private Sprite[] idleDownSprites;  // facing camera / down
    [SerializeField] private Sprite[] idleUpSprites;    // optional, leave empty if you don't have one
    [SerializeField] private Sprite[] idleSideSprites;  // side-facing idle, flipped via flipX for right

    [Header("Run Clips")]
    [SerializeField] private Sprite[] runDownSprites;
    [SerializeField] private Sprite[] runUpSprites;
    [SerializeField] private Sprite[] runSideSprites;   // side-facing run, flipped via flipX for right

    [Header("Timing")]
    [SerializeField] private float idleFramesPerSecond = 8f;
    [SerializeField] private float runFramesPerSecond = 10f;

    [Header("Facing")]
    [SerializeField] private float directionDeadzone = 0.05f;

    [Header("Scale Fix")]
    [Tooltip("Multiplier applied when facing Up or Down, to compensate for those sheets being drawn smaller than the side sheets. 1 = no change.")]
    [SerializeField] private float verticalDirectionScaleMultiplier = 1f;

    private Sprite[] currentSprites;
    private float frameTimer;
    private int frameIndex;
    private bool wasMoving;
    private Direction currentDirection = Direction.Down;
    private Vector3 baseLocalScale;

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

        if (targetRenderer != null)
        {
            baseLocalScale = targetRenderer.transform.localScale;
        }
    }

    private void OnEnable()
    {
        currentDirection = Direction.Down;
        PlayIdle();
    }

    private void Update()
    {
        Vector2 velocity = rb != null ? rb.linearVelocity : Vector2.zero;
        bool isMoving = velocity.sqrMagnitude > 0.01f;

        Direction newDirection = currentDirection;
        if (isMoving)
        {
            newDirection = GetDirectionFromVelocity(velocity);
        }

        bool directionChanged = newDirection != currentDirection;
        bool movementChanged = isMoving != wasMoving;

        if (directionChanged || movementChanged)
        {
            currentDirection = newDirection;

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

        ApplyDirectionScale();
        Tick(isMoving ? runFramesPerSecond : idleFramesPerSecond);
    }

    private Direction GetDirectionFromVelocity(Vector2 velocity)
    {
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y) + directionDeadzone)
        {
            return velocity.x > 0f ? Direction.Right : Direction.Left;
        }

        return velocity.y > 0f ? Direction.Up : Direction.Down;
    }

    // Left/Right mirroring is NOT handled here: Player.cs already flips the whole
    // GameObject's transform.localScale.x based on mouse aim, which also mirrors
    // the weapon (child of Player) automatically. We only ever adjust the magnitude
    // of the scale (for the vertical size fix) while preserving whatever sign
    // Player.cs has currently set, so we never fight with it.
    private void ApplyDirectionScale()
    {
        if (targetRenderer == null)
        {
            return;
        }

        bool isVertical = currentDirection == Direction.Up || currentDirection == Direction.Down;
        float multiplier = isVertical ? verticalDirectionScaleMultiplier : 1f;

        Vector3 current = targetRenderer.transform.localScale;
        float signX = current.x < 0f ? -1f : 1f;

        targetRenderer.transform.localScale = new Vector3(
            Mathf.Abs(baseLocalScale.x) * multiplier * signX,
            Mathf.Abs(baseLocalScale.y) * multiplier,
            baseLocalScale.z);
    }

    private void PlayIdle()
    {
        Play(GetClip(idleDownSprites, idleUpSprites, idleSideSprites));
    }

    private void PlayRun()
    {
        Play(GetClip(runDownSprites, runUpSprites, runSideSprites));
    }

    private Sprite[] GetClip(Sprite[] down, Sprite[] up, Sprite[] side)
    {
        switch (currentDirection)
        {
            case Direction.Up:
                return (up != null && up.Length > 0) ? up : down;
            case Direction.Left:
            case Direction.Right:
                return (side != null && side.Length > 0) ? side : down;
            case Direction.Down:
            default:
                return down;
        }
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
