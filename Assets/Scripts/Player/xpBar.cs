using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class xpBar : MonoBehaviour  // ATTACHED TO PLAYER
{
    [Header("XP Bar")]
    public Image xpBarObject;
    public AnimationCurve xpBarCurve;
    public float animTime;

    public bool queueing;
    public float queueTimer;

    public AudioSource audioSource;
    public AudioClip sfxLevelUp;
    public AudioClip sfxCard;

    public void startMedium()
    {
        if (queueing)return; //evita doppie coroutine
        StartCoroutine(xpBarSetGain());
    }
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
    }
    public IEnumerator xpBarSetGain()
    {
        queueing = true;

        float t = 0f;
        float totGain = 0f;
        while (t < queueTimer)
        {
            if (data.xpQueue.Count > 0)
            {
                t = 0f;
                totGain += data.xpQueue.Dequeue();
            }
            t += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(xpBarMovement(totGain));
        if (data.xpQueue.Count > 0) yield return StartCoroutine(xpBarSetGain());

        queueing = false;
    }
    public IEnumerator xpBarMovement(float totGain)
    {
        float startXp = xpBarObject.fillAmount * data.xpMax;
        data.xp = startXp;

        float toGain = Mathf.Min(totGain, data.xpMax - startXp);
        float targetXp = startXp + toGain;
        float targetFill = targetXp / data.xpMax;
        float overflow = totGain - toGain;

        xpBarCurve = AnimationCurve.EaseInOut(0, xpBarObject.fillAmount, animTime, targetFill);

        var t = 0f;
        while (t < animTime)
        {
            xpBarObject.fillAmount = xpBarCurve.Evaluate(t);
            data.xp = xpBarObject.fillAmount * data.xpMax;
            t += Time.deltaTime;
            yield return null;
        }
        xpBarObject.fillAmount = targetFill;
        data.xp = targetXp;

        if (xpBarObject.fillAmount >= 1) yield return StartCoroutine(levelUp());
        if (overflow > 0f) yield return StartCoroutine(xpBarMovement(overflow));
    }

    public IEnumerator levelUp()
    {
        xpBarObject.fillAmount = 0;
        data.level++;
        data.xp = 0;
        // nel levelUp():
        audioSource.clip = sfxLevelUp;
        audioSource.Play();
        cardManager.instance.spawnCards();
        audioSource.clip = sfxCard;
        audioSource.Play();
        data.xpMax += data.xpMax * 0.2f;
        yield return null;
    }
}
