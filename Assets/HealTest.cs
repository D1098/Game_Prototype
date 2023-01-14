using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HealthSystem hs = collision.GetComponent<HealthSystem>();
            if (hs.currentHealth < hs.maxHealth)
            {
                if (hs.maxHealth - hs.currentHealth > 2)
                    hs.ChangeHealth(2);
                else
                    hs.ChangeHealth(hs.maxHealth - hs.currentHealth);
            }
        }
    }
}
