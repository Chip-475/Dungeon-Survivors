using UnityEngine;

public class damageUp : cardClass, ICardEffect
{
    public void effect()
    {
        Player.instance.atk += Player.instance.atk * 0.2f;
    }

    public void CardEffect()
    {
        effect();
    }
}
