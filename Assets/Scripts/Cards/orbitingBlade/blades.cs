using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class blades : MonoBehaviour
{
    public bladesMovement bladesMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) collision.gameObject.GetComponent<IDamageable>().damage(player.playerInstance.atk * 1.5f);
    }

    
}
