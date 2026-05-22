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
    public bool usaNav=false;     //false=dritto alt..  si muove bene
    public NavMeshAgent agent;     //permette di farlo muovere secondo il percorso piu vicino alg A* (paura)
    public float raggio;           //tipo campo visivo
    public float speed;

    [Header("Spawn Nemici")]
    public float offsetSpawnLaterale;       // distanza laterale dal boss per spawn
    public float ritardoSpawn;          // piccolo delay tra spawn destro/sinistro
    public int quantitaPerLato;                // quanti spawn per lato

    [Header("scatto")]
    public float velScatto;
    public float cooldown;
    public float windup;
    public float dur;
    public float delay;
    public float raggioHitboxScatto = 1.2f;
    public float danno;

    public bool mostraGizmos = true;

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
    [SerializeField]private GameObject hitBox;

    void Awake()
    {
        if(usaNav)agent=GetComponent<NavMeshAgent>();
        Debug.Log("Boss");
    }
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }
    void Update()
    {   //cerco il player
        if (player == null) return;
        float dist = Vector3.Distance(transform.position, player.position);
        switch(sta)
        {
            case stato.inattivo:
                if (dist <= raggio) sta = stato.inseguimento;
                break;
            case stato.inseguimento:
                //per inseguirlo
                /*
                if (usaNav)
                {
                    Vector2 dir = (player.position - transform.position).normalized;
                    transform.position += (Vector3)dir * speed * Time.deltaTime;
                    //agent.isStopped = false;
                    //agent.SetDestination(player.position);
                }
                else
                {
                    Vector3 dire = (player.position - transform.position).normalized;
                    transform.position += dire * speed * Time.deltaTime;
                    //ora ruota verso il gioc
                    if (dire.sqrMagnitude > 0.001f)
                    {
                        float angolo = Mathf.Atan2(dire.y, dire.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angolo), 8f * Time.deltaTime);

                    }
                    if (dist <= raggio && scattoDisp && Time.time - ultimo >= cooldown) StartCoroutine(esegui());
                }*/
                Vector3 dire = (player.position - transform.position).normalized;
                if (dist > 1f) transform.position += dire * speed * Time.deltaTime;
                float angolo=Mathf.Atan2(dire.y, dire.x)*Mathf.Rad2Deg;
                transform.rotation=Quaternion.Euler(0,0,angolo);
                if (dist <= raggio && scattoDisp && Time.time - ultimo >= cooldown) StartCoroutine(esegui());
                break;
            
            case stato.avvolgimento:
            case stato.scatto:
            case stato.recupero:
                break;
        }
    }

    IEnumerator esegui()
    {
        sta = stato.inattivo;
        scattoDisp = false;
        ultimo = Time.time;
        yield return new WaitForSeconds(windup);
        //scatto
        sta = stato.scatto;
        //if(usaNav&&agent!=null)agent.isStopped=true;
        //metto la dire verso il player
        dir = (player.position - transform.position).normalized;
        float tempo = 0f;
        hitBox.SetActive(true);//la sua hitbox
        while(tempo<dur)
        {
            transform.position += dir * velScatto * Time.deltaTime;
            float angolo=Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
            //if (dir.sqrMagnitude > 0.001f) transform.rotation=Quaternion.Euler(0, 0f, angolo);
            transform.rotation = Quaternion.Euler(0, 0, angolo);
            tempo += Time.deltaTime;
            yield return null;
        }
        hitBox.SetActive(false);
        //nemici
        StartCoroutine(nemici());
        sta = stato.recupero;
        //if(usaNav&&agent != null)agent.isStopped=false;
        yield return new WaitForSeconds(0.4f);
        sta= stato.inseguimento;
        //attesa del cooldown
        float time=Time.time - ultimo;
        if (time < cooldown) yield return new WaitForSeconds(cooldown - time);
        scattoDisp = true;
    }

    IEnumerator nemici()
    {
        Vector3 duceBase = transform.position + transform.right * offsetSpawnLaterale;
        Vector3 marxBase = transform.position - transform.right * offsetSpawnLaterale;
        for (int i=0;i<quantitaPerLato;i++)
        {
            Vector3 posDestra =duceBase +(Vector3)Random.insideUnitCircle*0.2f;
            Vector3 posSin =marxBase +(Vector3)Random.insideUnitCircle * 0.2f;
            posDestra.y=transform.position.y;
            posSin.y = transform.position.y;
            spawnNemico(posDestra);
            yield return new WaitForSeconds(ritardoSpawn);
            spawnNemico(posSin);
            yield return new WaitForSeconds(ritardoSpawn);
        }
    }

    private void spawnNemico(Vector3 pos)
    {
        if (prefNemico == null) return;
        Instantiate(prefNemico, pos,Quaternion.identity);
    }

    public void colpito(GameObject play)
    {
        var salute = play.GetComponent<player>();
        if (salute != null) salute.damage(danno);
        var rb = play.GetComponent<Rigidbody>();
        if(rb!=null)
        {
            Vector3 so=(play.transform.position - transform.position).normalized * 6f;
            rb.AddForce(so + Vector3.up * 2f, ForceMode.Impulse);
        }
    }
    public void OnDrawGizmosSelected()
    {
        if (!mostraGizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raggio);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raggioHitboxScatto);
        if (puntoLeft != null) Gizmos.DrawSphere(puntoLeft.position, 0.15f);
        if (puntoRight != null) Gizmos.DrawSphere(puntoRight.position, 0.15f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player")) other.gameObject.GetComponent<player>()?.damage(danno);
    }
}
