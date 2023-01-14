using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    TextMeshPro tmp;
    Rigidbody2D rb;
    float lifeTime = 0.7f;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tmp = GetComponent<TextMeshPro>();
        Destroy(gameObject, lifeTime);
    }

    public void GetDamageAmount(int damageAmount)
    {
        tmp.SetText(damageAmount.ToString());
    }


    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(0, 0.5f);
    }
}
