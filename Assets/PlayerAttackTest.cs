using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTest : MonoBehaviour
{
    bool attackKeyPressed = false;
    public GameObject slash;
    public float attackCooldown = 2f, cooldownTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackKeyPressed = Input.GetKeyDown(KeyCode.Space);
        if(attackKeyPressed && cooldownTimer <= Time.time)
        {
            cooldownTimer = Time.time + attackCooldown;
            StartCoroutine(Attack());
            //Это тест анимации, так что пока никакой атаки :)
        }
    }

    IEnumerator Attack()
    {
        GameObject s = Instantiate(slash, transform);
        yield return new WaitForSeconds(0.12f);
        Destroy(s);
    }
}
