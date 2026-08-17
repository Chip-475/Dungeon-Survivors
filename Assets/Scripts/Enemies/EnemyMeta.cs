using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Meta", menuName = "Scriptable Objects/Enemy Meta")]
public class EnemyMeta : ScriptableObject
{
    public AudioClip deathSound;
    [Space]
    public float hp;
    public float hpMax;
    public float xpGiven;
    public float atk;
    public float spd;
    [Space]
    public float fovRange;
    [Range(0, 360)] public float fovAngle;
}
