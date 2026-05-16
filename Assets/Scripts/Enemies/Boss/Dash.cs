using UnityEngine;
public class Dash : MonoBehaviour
{
    public BossController prop;
    public LayerMask layerPlayer;

    void OnTriggerEnter(Collider other)
    {
        // Controllo layer
        if ((layerPlayer & (1 << other.gameObject.layer)) == 0)return;
        // Controllo tag
        if (other.CompareTag("Player"))prop.colpito(other.gameObject);
    }
}
