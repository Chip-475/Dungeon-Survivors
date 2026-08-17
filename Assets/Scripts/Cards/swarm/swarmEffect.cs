using UnityEngine;

public class swarmEffect : cardClass,ICardEffect
{
    public static bool swarm = false;
    public void setSwarmEffect()
    {
        swarm = true;
    }
    public void cardEffect()
    {
        setSwarmEffect();
    }
}
