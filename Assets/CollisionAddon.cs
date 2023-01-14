using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAddon : MonoBehaviour
{
    //public bool fail;
    public int failCount = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetComponentInParent<CollisionTest>().St);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Note")
        {
            Debug.Log("Fail");
            GetComponentInParent<CollisionTest>().St = CollisionTest.state.tooLate;
            Destroy(collision.gameObject);
            //Debug.Log("End: " + Time.fixedTime);
            //fail = true;
            GameObject.FindWithTag("Player")?.GetComponent<HealthSystem>().ChangeHealth(-1f); // Для тестов
            failCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //fail = false;
        GetComponentInParent<CollisionTest>().St = CollisionTest.state.tooSoon;
    }
}
