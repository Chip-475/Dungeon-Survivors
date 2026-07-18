using UnityEngine;

public class cardClass : MonoBehaviour
{
    protected Player player;
    public enum cardType
    {
        damageUp,
        electroAura,
        iceAura,
        orbitingBlades
    }
    public cardType type;


    public int lvl;
    public int lvlMax;
    protected bool active = false;

    public float duration;

    protected void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
}
