using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Security.Cryptography;

public class hpBar : MonoBehaviour // ATTACHED TO PLAYER
{
    [Header("HP Bar")]
    public Image hpBarObject;
    public AnimationCurve hpBarCurve;
    public float animTime;
    public bool running;

    public IEnumerator hpBarMovement(float currentHp, float nextHp)
    {
        Debug.Log("cr started");
        player.playerInstance.hp = nextHp;
        hpBarCurve = AnimationCurve.EaseInOut(0, currentHp / player.playerInstance.hpMax, animTime, nextHp / player.playerInstance.hpMax);
        running = true;
        var x = 0f;
        while (x < animTime)
        {
            hpBarObject.fillAmount = hpBarCurve.Evaluate(x);
            x += Time.deltaTime;
            yield return null;
        }
        running = false;
    }
}
