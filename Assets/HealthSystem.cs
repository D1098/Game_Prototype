using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 5f;
    public float currentHealth;
    GameObject ds, popup;
    AudioSource sfxHurt, sfxLose, sfxHeal;
    GameObject multiMedia;
    public bool isTutorial = false;

    void Start()
    {
        SoundInit();
        popup = Resources.Load<GameObject>("DamagePopup");
        ds = GameObject.Find("DeathScreen");
        ds?.SetActive(false);
        currentHealth = maxHealth;
    }

    void SoundInit()
    {
        multiMedia = GameObject.FindWithTag("MainCamera");
        sfxHurt = multiMedia.AddComponent<AudioSource>();
        sfxLose = multiMedia.AddComponent<AudioSource>();
        sfxHeal = multiMedia.AddComponent<AudioSource>();
        sfxHurt.clip = Resources.Load<AudioClip>("SFX/hurt3");
        sfxLose.clip = Resources.Load<AudioClip>("SFX/lose3");
        sfxHeal.clip = Resources.Load<AudioClip>("SFX/heal1");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ќтрицательные значени€ дл€ урона, положительные дл€ восстановлени€
    public void ChangeHealth(float amount)
    {
        if (currentHealth < 0)
            return;
        if (amount < 0)
            StartCoroutine(Popup());
        if (currentHealth + amount <= 0f)
        {
            if (isTutorial)
            {
                sfxLose.Play();
                ChangeHealth(5 - currentHealth);
            }
            else
            {
                foreach (var item in multiMedia.GetComponents<AudioSource>())
                    item.Stop();
                sfxLose.Play();
                multiMedia.GetComponent<Camera>().orthographicSize = 0.6f;
                currentHealth += amount;
                Destroy(gameObject);
                ds.SetActive(true);
            }
        }
        else
        {
            if (amount < 0)
                sfxHurt.Play();
            else
                sfxHeal.Play();
            currentHealth += amount;
        }

    }

    // ¬сплывающее значение нанесенного урона
    IEnumerator Popup()
    {
        GameObject p = Instantiate(popup, transform.position + new Vector3(0.04f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Destroy(p);
    }

    //Ќе готово
    IEnumerator Popup(int amount)
    {
        GameObject p = Instantiate(popup, transform.position + new Vector3(0.04f, 0f), Quaternion.identity);
        p.GetComponent<Popup>().GetDamageAmount(amount);
        yield return new WaitForSeconds(0.5f);
        Destroy(p);
    }
}
