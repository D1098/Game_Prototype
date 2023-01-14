using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 1f, viewRange = 0.5f, attackRange = 0.1f;
    public Rigidbody2D body;
    public LayerMask playerTarget;
    //Animator animator;
    public Vector2 move;
    Transform target;
    public bool targetInRange, targetInAttackRange;
    float cooldownTimer;
    Vector2 wayPoint;
    GameObject question, exclamation;
    GameObject wayp;
    //ƒл€ открытых пространств врагу нужно понимать, где он находитс€ и нужно ли ему вернутьс€ в зону где он должен быть

    //—делать состо€ни€ типа: бродит, патрулирует, сторожит, спит...
    //и запускать остальное на их основе

    // Start is called before the first frame update

    public enum State
    {
        Idle, Wandering, Chasing, Patrolling
    }

    public State state = State.Wandering;

    bool patrolRunning = false, wanderRunnung = false;

    void StartIcons()
    {
        question = Instantiate((GameObject)Resources.Load("question", typeof(GameObject)), transform.position + new Vector3(0.04f, 0f), Quaternion.identity, transform);
        exclamation = Instantiate((GameObject)Resources.Load("danger", typeof(GameObject)), transform.position + new Vector3(0.04f, 0f), Quaternion.identity, transform);
        question.SetActive(false);
        exclamation.SetActive(false);
    }

    void Start()
    {
        // ѕо идее нужно 3 области - дл€ опознани€, преследовани€ и атаки. ќбойдусь двум€...
        target = GameObject.FindWithTag("Player").transform;
        body = GetComponent<Rigidbody2D>();
        wayp = new GameObject(name + "'s waypoint");
        StartIcons();
    }

    void ChangeState()
    {
        IEnumerator patrol = Patrol(2f);
        switch (state)
        {
            case State.Idle:
                if (targetInRange)
                    state = State.Chasing;
                question.SetActive(false);
                exclamation.SetActive(false);
                break;
            case State.Wandering:
                if (targetInRange)
                    state = State.Chasing;
                if (!wanderRunnung)
                    StartCoroutine(Wander(3f));
                question.SetActive(false);
                exclamation.SetActive(false);
                break;
            case State.Chasing:
                if (!targetInRange)
                    state = State.Patrolling;
                question.SetActive(false);
                exclamation.SetActive(true);
                break;
            case State.Patrolling:
                if (targetInRange)
                {
                    StopAllCoroutines();
                    patrolRunning = false;
                    wanderRunnung = false;
                    state = State.Chasing;
                }
                if (!patrolRunning)
                    StartCoroutine(patrol);
                question.SetActive(true);
                exclamation.SetActive(false);
                break;
            default:
                break;
        }
    }

    private IEnumerator Patrol(float patrolDuration)
    {
        Debug.Log("Starting New cr");
        patrolRunning = true;
        yield return new WaitForSeconds(patrolDuration);
        if (state == State.Patrolling)
            state = State.Wandering;
        patrolRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeState();
        targetInRange = Physics2D.OverlapCircle(transform.position, viewRange, playerTarget);
        if (targetInRange)
            target = GameObject.FindWithTag("Player").transform;
        if (target == null)
            return;
        targetInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, playerTarget);
        float distance = Vector3.Distance(target.position, transform.position);
        move = distance > 0.02f ? target.position - transform.position : new Vector2(0, 0);
        //float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
        move.Normalize();
        // ѕоворот спрайта
        GetComponent<SpriteRenderer>().flipX = move.x < 0;
    }

    private void FixedUpdate()
    {
        if (targetInRange && cooldownTimer <= Time.time)
        {
            if (targetInAttackRange)
            {
                cooldownTimer = Time.time + 2f;
                GameObject.Find("Canvas").GetComponent<NoteDist>().ProvideEnemyAttack(4, gameObject);//Debug.Log("ATTACK");
            }
            else
                body.velocity = move * speed;
            //body.MovePosition((Vector2)transform.position + speed * move * Time.deltaTime);
        }
        else if (state == State.Wandering)
        {
            body.velocity = move * speed * 0.5f;
        }
        else if(state == State.Patrolling)
        {
            body.velocity = move * speed * 0.5f;
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }

    //Ѕродим
    private IEnumerator Wander(float delay)
    {
        wanderRunnung = true;
        wayPoint = new Vector2(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f));
        //Debug.Log(wayPoint);
        wayp.transform.position = wayPoint;
        target = wayp.transform;
        yield return new WaitForSeconds(delay * Random.Range(1f, 2f));
        wanderRunnung = false;
    }
}

// (ƒолжен быть)  ласс дл€ возвращени€ данных по статусу атаки и урону по игроку/цели
class AtackResult
{
    bool success;
    float damageNum;

    public AtackResult(bool success, float damageNum)
    {
        this.success = success;
        this.damageNum = damageNum;
    }
}
