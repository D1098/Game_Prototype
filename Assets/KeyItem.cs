using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public enum itemType { bronzeKey, silverKey, goldenKey, healPotion };
    public itemType type;
    float yPos;
    GameObject multiMedia;
    AudioSource sfxPickup;
    // Start is called before the first frame update
    void Start()
    {
        yPos = transform.position.y;
        SoundInit();
    }

    void SoundInit()
    {
        multiMedia = GameObject.FindWithTag("MainCamera");
        sfxPickup = multiMedia.AddComponent<AudioSource>();
        sfxPickup.clip = Resources.Load<AudioClip>("SFX/pickup1");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,
           yPos + Mathf.Sin(Time.time * 3f) * 0.04f,
           transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Key Pickup");
            TestCanvas canvasScript = GameObject.Find("Canvas").GetComponent<TestCanvas>();
            sfxPickup.Play();
            switch (type)
            {
                case itemType.bronzeKey:
                    collision.gameObject.GetComponent<InventorySimple>().hasBronzeKey = true;
                    canvasScript.toggleOnKeyIndicator((int)type);
                    break;
                case itemType.silverKey:
                    collision.gameObject.GetComponent<InventorySimple>().hasSilverKey = true;
                    canvasScript.toggleOnKeyIndicator((int)type);
                    break;
                case itemType.goldenKey:
                    collision.gameObject.GetComponent<InventorySimple>().hasGoldenKey = true;
                    canvasScript.toggleOnKeyIndicator((int)type);
                    break;
                case itemType.healPotion:
                    HealthSystem hs = collision.GetComponent<HealthSystem>();
                    if (hs.currentHealth < hs.maxHealth)
                    {
                        if (hs.maxHealth - hs.currentHealth > 2)
                            hs.ChangeHealth(2);
                        else
                            hs.ChangeHealth(hs.maxHealth - hs.currentHealth);
                    }
                    break;
            }
            Destroy(gameObject);
        }
    }
}
