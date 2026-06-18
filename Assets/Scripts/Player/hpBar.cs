using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hpBar : MonoBehaviour // ATTACHED TO PLAYER
{
    [Header("HP Bar")]
    public Image hpBarObject;
    public AnimationCurve hpBarCurve;
    public float animTime;
    public bool running;
    private int movementId;

    public IEnumerator hpBarMovement(float currentHp, float nextHp)
    {
        movementId++;
        int currentMovementId = movementId;

        nextHp = Mathf.Clamp(nextHp, 0, player.playerInstance.hpMax);
        player.playerInstance.hp = nextHp;

        float currentFill = hpBarObject != null ? hpBarObject.fillAmount : currentHp / player.playerInstance.hpMax;
        float nextFill = player.playerInstance.hpMax > 0 ? nextHp / player.playerInstance.hpMax : 0f;
        hpBarCurve = AnimationCurve.EaseInOut(0, currentFill, animTime, nextFill);

        running = true;
        var x = 0f;
        while (x < animTime)
        {
            if (currentMovementId != movementId)
            {
                yield break;
            }

            hpBarObject.fillAmount = hpBarCurve.Evaluate(x);
            x += Time.unscaledDeltaTime;
            yield return null;
        }

        hpBarObject.fillAmount = nextFill;
        running = false;
    }
}
