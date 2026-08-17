using UnityEngine;
using System.Collections;
public class attackSpeedUp : cardClass, ICardEffect
{
    public float baseAspd;
    private new void Start()
    {
        baseAspd = player.playerInstance.aspd;
    }
    public void effect()
    {
        player.playerInstance.aspd += baseAspd * 0.2f;
    }

    public void cardEffect()
    {
        effect();
    }
}
