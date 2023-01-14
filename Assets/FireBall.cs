using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 1.5f;
    public float destroyTimer = 1.5f;
    GameObject canvas;
    Animator animator;
    Color color = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        ColorUtility.TryParseHtmlString("#CD2034", out color);
        animator = GetComponent<Animator>();
        canvas = GameObject.Find("Canvas");
        Debug.Log("Fireball here!");
        rb = GetComponent<Rigidbody2D>();
        //Destroy(gameObject, destroyTimer);
        StartCoroutine(DestByTime());
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canvas.GetComponent<NoteDist>().ProvideSimpleAttack(2, color);
            StartCoroutine(Dest());
        }

    }

    IEnumerator Dest(float timer = 0.2f)
    {
        animator.SetBool("Boom", true);
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }

    IEnumerator DestByTime(float timer = 0.2f)
    {
        yield return new WaitForSeconds(destroyTimer);
        animator.SetBool("Boom", true);
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
