using UnityEngine;
using System.Collections;
public class attackSpeedUp : cardClass, ICardEffect
{
    public float baseAspd;
    private new void Start()
    {
        baseAspd = Player.instance.aspd;
    }
    public void effect()
    {
        Player.instance.aspd += baseAspd * 0.2f;
    }

    public void CardEffect()
    {
        effect();
    }
}
