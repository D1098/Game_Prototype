using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public enum itemType { bronzeLock, silverLock, goldenLock };
    public itemType type;
    [SerializeField]
    bool inRange;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Player") == null)
            return;
        InventorySimple inventory = GameObject.FindWithTag("Player").GetComponent<InventorySimple>();
        bool hasKey = false;
        switch (type)
        {
            case itemType.bronzeLock:
                hasKey = inventory.hasBronzeKey;
                break;
            case itemType.silverLock:
                hasKey = inventory.hasSilverKey;
                break;
            case itemType.goldenLock:
                hasKey = inventory.hasGoldenKey;
                break;
        }
        inRange = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Player"));

        if (inRange && hasKey && Input.GetKeyDown(KeyCode.E))
        {
            GetComponentInParent<GateManager>().OpenGate();
        }
    }
}
