using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    public float currentHealth;
    bool isFlicking = false;
    GameObject popup;
    RectTransform healthBar;
    GameObject multiMedia;
    AudioSource sfxDeath;
    void Start()
    {
        SoundInit();
        popup = (GameObject)Resources.Load("DamagePopup", typeof(GameObject));
        currentHealth = maxHealth;
        if (GetComponentInChildren<RectTransform>() != null)
            healthBar = GetComponentInChildren<RectTransform>();
    }

    void SoundInit()
    {
        multiMedia = GameObject.FindWithTag("MainCamera");
        sfxDeath = multiMedia.AddComponent<AudioSource>();
        sfxDeath.clip = Resources.Load<AudioClip>("SFX/hurt2");
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar != null)
        {
            healthBar.localScale = new Vector3(currentHealth / maxHealth, 1);
        }
            
    }

    // Отрицательные значения для урона, положительные для восстановления
    public void ChangeHealth(float amount)
    {
        if (amount < 0)
            StartCoroutine(Popup());
        if (currentHealth + amount <= 0f)
        {
            currentHealth += amount;
            Destroy(gameObject);

            sfxDeath.Play();

            Debug.Log("Вражина " + gameObject.name + " помер");
            GameObject.Find("Canvas").GetComponent<NoteDist>().dmgSrc = null;
        }
        else
        {
            currentHealth += amount;
            if (!isFlicking)
                StartCoroutine(Flick());
        }

    }

    IEnumerator Flick()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        isFlicking = true;
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            isFlicking = false;
        }
    }

    IEnumerator Popup()
    {
        GameObject p = Instantiate(popup, transform.position + new Vector3(0.04f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Destroy(p);
    }
}
