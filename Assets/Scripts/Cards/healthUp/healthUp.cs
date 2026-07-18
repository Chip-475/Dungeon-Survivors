using UnityEngine;

public class healthUp : cardClass, ICardEffect
{
    public void effect()
    {
        Player.instance.hpMax += Player.instance.hpMax * 0.25f;
        StartCoroutine(Player.instance.hpBar.hpBarMovement(Player.instance.hp, Player.instance.hp + Player.instance.hp * 0.25f));
    }
    public void CardEffect()
    {
        effect();
    }
}
