using UnityEngine;
using System.Collections;
using UnityEngine.AI;    // si usa quando un gameobject si muove secondo sistema di navigazione diverso

public class BossController : MonoBehaviour
{
    [Header("riferimenti")]
    public Transform player;  
    public GameObject prefNemico;     
    public Transform puntoLeft;
    public Transform puntoRight;      //dove spawn i nemici
    public LayerMask layerPlayer;   //layer per riconoscere il gioc
                                    //layer che servono anche per dare la priorita degli oggetti della scena

    [Header("movimento")]
    public bool usaNav = true;     //false=dritto alt..  si muove bene
    public NavMeshAgent agent;     //permette di farlo muovere secondo il percorso piu vicino alg A* (paura)
    public float raggio;           //tipo campo visivo
    public float speed;

    [Header("scatto")]
    public float velScatto;
    public float dur;
    public float delay;
    public float danno;

    private enum stato
    {
        inattivo,
        inseguimento,
        avvolgimento,
        scatto,
        recupero
    }
    private stato sta = stato.inattivo;
    private bool scattoDisp = true;
    private float ultimo = -1f;
    private Vector3 dir;
    private GameObject hitBox;

    void Awake()
    {
        if (usaNav && agent == null) agent = GetComponent<NavMeshAgent>();
       // creaHitbox();
    }

    void Update()
    {   //cerco il player
        if(player==null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.transform;
            else return;
        }
        float dist = Vector3.Distance(transform.position, player.position);
        switch(sta)
        {
            case stato.inattivo:
                if (dist <= raggio) sta = stato.inseguimento;
                break;
            case sta.inseguimento:
                //per inseguirlo
                if(usaNav&&agent!=null)
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                }
                else
                {
                    Vector3 dire=(player.position -transform.position).normalized;
                    transform.position += dire * speed * Time.deltaTime;
                    //ora ruota verso il gioc
                    if (dire.sqrMagnitude > 0.001f) transform.forward = Vector3.Lerp(transfrom.forward, dire, 8f, Time.deltaTime);
                }
                

        }
    }

}
