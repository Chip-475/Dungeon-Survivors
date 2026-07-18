using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml.Linq;

public class XpBar : MonoBehaviour  // ATTACHED TO PLAYER
{
    [Header("XP Bar")]
    public static XpBar instance;

    public Image xpBarObject;
    public AnimationCurve xpBarCurve;
    public float animTime;
    public AudioClip lvlUP;
    public bool queueing;
    public float queueTimer;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void StartXpGain()
    {
        StartCoroutine(xpBarSetGain());
    }

    public IEnumerator xpBarSetGain()
    {
        queueing = true;

        float t = 0f;
        float totGain = 0f;
        while (t < queueTimer)
        {
            if (Data.xpQueue.Count > 0)
            {
                t = 0f;
                totGain += Data.xpQueue.Dequeue();
            }
            t += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(xpBarMovement(totGain));
        if (Data.xpQueue.Count > 0) yield return StartCoroutine(xpBarSetGain());

        queueing = false;
    }
    public IEnumerator xpBarMovement(float totGain)
    {
        float startXp = xpBarObject.fillAmount * Data.xpMax;
        Data.xp = startXp;

        float toGain = Mathf.Min(totGain, Data.xpMax - startXp);
        float targetXp = startXp + toGain;
        float targetFill = targetXp / Data.xpMax;
        float overflow = totGain - toGain;

        xpBarCurve = AnimationCurve.EaseInOut(0, xpBarObject.fillAmount, animTime, targetFill);

        var t = 0f;
        while (t < animTime)
        {
            xpBarObject.fillAmount = xpBarCurve.Evaluate(t);
            Data.xp = xpBarObject.fillAmount * Data.xpMax;
            t += Time.deltaTime;
            yield return null;
        }
        xpBarObject.fillAmount = targetFill;
        Data.xp = targetXp;

        if (xpBarObject.fillAmount >= 1) yield return StartCoroutine(levelUp());
        if (overflow > 0f) yield return StartCoroutine(xpBarMovement(overflow));
    }

    public IEnumerator levelUp()
    {
        xpBarObject.fillAmount = 0;
        Data.level++;
        Data.xp = 0;
        cardManager.instance.spawnCards();
        audioManager.manager.playSFX(lvlUP,Player.instance.transform,Data.sfx);
        if(Data.level >= 20) 
        {
            Data.xpMax += Data.xpMax * 0.8f;
            yield return null;
        }
        else if(Data.level >15)  
        {
            Data.xpMax += Data.xpMax * 0.4f;
            yield return null;
        }
        else  
        {
            Data.xpMax += Data.xpMax * 0.2f;
            yield return null;
        }
    }
}
