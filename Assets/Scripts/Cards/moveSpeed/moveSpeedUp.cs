using UnityEngine;

public class moveSpeedUp : cardClass, ICardEffect
{
    public void effect()
    {
        Player.instance.spd += Player.instance.spd * 0.2f;
    }
    public void CardEffect()
    {
        effect();
    }
}
