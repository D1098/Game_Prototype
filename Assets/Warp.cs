using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp : MonoBehaviour
{
    public int targetX, targetY;
    public GameObject transition;
    public bool isOn = true;
    public bool levelTrans = false;
    public string levelName = "Level2";
    GameObject multiMedia;
    AudioSource sfxNextLevel;
    // Start is called before the first frame update
    void Start()
    {
        SoundInit();
    }

    void SoundInit()
    {
        multiMedia = GameObject.FindWithTag("MainCamera");
        sfxNextLevel = multiMedia.AddComponent<AudioSource>();
        sfxNextLevel.clip = Resources.Load<AudioClip>("SFX/win1");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 realCoords = new Vector2();
        Grid grid = FindObjectOfType<Grid>();
        if (grid != null)
            realCoords = grid.CellToWorld(new Vector3Int(targetX, targetY));
        if (collision.gameObject.tag == "Player" && isOn)
        {
            if (levelTrans)
            {
                sfxNextLevel.Play();
                StartCoroutine(NextLevel());
                Debug.Log($"Win! Next Level...");
            }
            else
            {
                StartCoroutine(Transition(collision, realCoords));
                Debug.Log($"WARP to {targetX}:{targetY}");
            }
        }
    }

    IEnumerator Transition(Collider2D collision, Vector2 realCoords)
    {
        transition = Instantiate(Resources.Load<GameObject>("TransitionAnim"));
        Animator animator = transition.GetComponentInChildren<Animator>();
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        collision.gameObject.transform.position = realCoords;
        GameObject.Find("Main Camera").transform.position = realCoords;
        animator.SetTrigger("Continue");
        Destroy(transition, 1.5f);
    }

    IEnumerator NextLevel()
    {
        transition = Instantiate(Resources.Load<GameObject>("TransitionAnim"));
        Animator animator = transition.GetComponentInChildren<Animator>();
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelName);
    }
}
