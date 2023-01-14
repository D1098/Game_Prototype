using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntAi : MonoBehaviour
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
    Animator animator;
    //Для открытых пространств врагу нужно понимать, где он находится и нужно ли ему вернуться в зону где он должен быть

    //Сделать состояния типа: бродит, патрулирует, сторожит, спит...
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
        question = Instantiate((GameObject)Resources.Load("question", typeof(GameObject)), transform.position + new Vector3(0.05f, 0f), Quaternion.identity, transform);
        exclamation = Instantiate((GameObject)Resources.Load("danger", typeof(GameObject)), transform.position + new Vector3(0.05f, 0f), Quaternion.identity, transform);
        question.SetActive(false);
        exclamation.SetActive(false);
    }

    void Start()
    {
        // По идее нужно 3 области - для опознания, преследования и атаки. Обойдусь двумя...
        target = GameObject.FindWithTag("Player").transform;
        body = GetComponent<Rigidbody2D>();
        wayp = new GameObject(name + "'s waypoint");
        //wayp.AddComponent<BoxCollider2D>();
        //wayp.GetComponent<BoxCollider2D>().size = new Vector2(0.01f, 0.01f);
        //wayp.GetComponent<BoxCollider2D>().isTrigger = true;
        animator = GetComponent<Animator>();
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
        move = distance > 0.04f ? target.position - transform.position : new Vector2(0, 0);
        if (distance > 0.04f)
            move = target.position - transform.position;
        else
        {
            move = new Vector2(0, 0);
            animator.SetBool("isWalking", false);
        }
        //float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
        move.Normalize();
        // Поворот спрайта
        GetComponent<SpriteRenderer>().flipX = move.x < 0;
    }

    private void FixedUpdate()
    {
        if (targetInRange && cooldownTimer <= Time.time)
        {
            if (targetInAttackRange)
            {
                cooldownTimer = Time.time + 2f;
                GameObject.Find("Canvas").GetComponent<NoteDist>().ProvideEnemyAttack(5, gameObject);//Debug.Log("ATTACK");
                animator.SetBool("isWalking", false);
            }
            else
            {
                body.velocity = move * speed;
                animator.speed = 1f;
                animator.SetBool("isWalking", true);
            }

            //body.MovePosition((Vector2)transform.position + speed * move * Time.deltaTime);
        }
        else if (state == State.Wandering)
        {
            body.velocity = move * speed * 0.5f;
            animator.speed = 0.5f;
            animator.SetBool("isWalking", true);
        }
        else if (state == State.Patrolling)
        {
            body.velocity = move * speed * 0.5f;
            animator.speed = 0.5f;
            animator.SetBool("isWalking", true);
        }
        else
        {
            body.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
        if (Mathf.Abs(body.velocity.x) <= 0.001f && Mathf.Abs(body.velocity.y) <= 0.001f)
            animator.SetBool("isWalking", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, wayPoint);
    }

    //Бродим
    private IEnumerator Wander(float delay)
    {
        int debugCount = 0;
        wanderRunnung = true;
        // Оно почему-то все еще задевает коллайдер самого врага
        int layermask = ~LayerMask.GetMask("UI", "IgnoreRaycast");
        // Поиск возможной точки назначения
        //do
        //{
        //    wayPoint = new Vector2(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f));
        //    // Если оно по какой-то причине не сможет выйти из этого цикла за 20 проходов
        //    debugCount++;
        //    if (debugCount >= 20)
        //        break;
        //}
        //while (Physics2D.Linecast((Vector2)transform.position, wayPoint, layermask));
        while (true)
        {
            wayPoint = new Vector2(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f));
            if (!Physics2D.Linecast((Vector2)transform.position, wayPoint, layermask))
                break;
            // Если оно по какой-то причине не сможет выйти из этого цикла за 20 проходов
            debugCount++;
            if (debugCount >= 20)
                break;
            //if (Physics2D.Linecast((Vector2)transform.position, wayPoint, layermask).collider.name == "Ent_test")
            //    break;
            Debug.Log("Cant spawn waypoint here!\nCollider: " + Physics2D.Linecast((Vector2)transform.position, wayPoint, layermask).collider.name);
        }
        wayp.transform.position = wayPoint;
        target = wayp.transform;
        yield return new WaitForSeconds(delay * Random.Range(1f, 2f));
        wanderRunnung = false;
    }
}