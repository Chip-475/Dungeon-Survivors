using UnityEngine;

public class fireAspectCard : cardClass, ICardEffect
{
    private void effect()
    {
        data.fireAspectLvl++;
        print("fireAspect picked");
    }
    public void cardEffect()
    {
        effect();
    }
}
