using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.ParticleSystem;
using Coffee.UIExtensions;

public class CollisionTest : MonoBehaviour
{
    public GameObject ripple;
    //bool inPlace = false;
    GameObject note;
    //Queue<GameObject> notes;
    //float successRate = -1f;
    // Несколько коллайдеров для проверок и уничтожения упавших?

    public KeyCode key = KeyCode.X;

    // Надо сделать через эти состояния триггер
    public enum state { tooSoon, Good, Perfect, tooLate }
    state st = state.tooSoon;
    public state St { get => st; set => st = value; }

    public new AudioClip audio;

    AudioSource sfxSuccess;
    GameObject multiMedia;

    // Start is called before the first frame update
    void Start()
    {
        SoundInit();
        ripple = (GameObject)Resources.Load("Particle System 2 Variant", typeof(GameObject));
        //st = state.tooSoon;
    }

    void SoundInit()
    {
        multiMedia = GameObject.FindWithTag("MainCamera");
        sfxSuccess = multiMedia.AddComponent<AudioSource>();
        sfxSuccess.clip = audio;//Resources.Load<AudioClip>("SFX/press1");
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log(St);
        if (Input.GetKeyDown(key))
        {
            //StartCoroutine(RippleEffect());
            if (note != null)//inPlace)
            {
                if (GetComponentInParent<NoteDist>().dmgSrc != null)
                    GetComponentInParent<NoteDist>().dmgSrc.GetComponent<EnemyHealth>().ChangeHealth(-1f);
                StartCoroutine(RippleEffect());
                sfxSuccess.Play();
                Destroy(note);
                note = null;
            }
            else
            {
                //Debug.Log("meh");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Note")
        {
            note = collision.gameObject;
            St = state.Good;
            //note.name = notes.Count.ToString();
            //inPlace = true;
            //Debug.Log("COLF");
        }
    }

    private IEnumerator RippleEffect()
    {
        UIParticle particle = GameObject.Find("UIParticle").GetComponent<UIParticle>();
        //GameObject r = Instantiate(ripple, transform.position, transform.rotation, transform);
        GameObject r = Instantiate(ripple, transform.position / 10f, transform.rotation, GameObject.Find("UIParticle").GetComponent<UIParticle>().transform);
        particle.particles.Add(r.GetComponent<ParticleSystem>());
        //
        particle.RefreshParticles();
        //GameObject.Find("UIParticle").GetComponent<UIParticle>().SetParticleSystemPrefab(r);
        //Debug.Log(GameObject.Find("UIParticle").GetComponent<UIParticle>().particles[0].ToString());
        //GameObject.Find("UIParticle").GetComponent<UIParticle>().Play();
        r.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(2f);
        Destroy(r);
        Destroy(GameObject.Find("UIParticleRenderer"));
        particle.RefreshParticles();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(GetComponent<Collider2D>().tag);
        if (collision.gameObject.tag == "Note")
        {
            //inPlace = false;
            note = null;
            st = state.tooSoon;
        }
    }
}
