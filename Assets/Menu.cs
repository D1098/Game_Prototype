using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Animator transition;
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        StartCoroutine(LoadLevel());//SceneManager.LoadScene("Level1");
    }

    public void StartTutorial()
    {
        StartCoroutine(LoadTutorial());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel()
    {
        transition = Instantiate(Resources.Load<GameObject>("TransitionAnim")).GetComponentInChildren<Animator>();
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level1");
    }

    IEnumerator LoadTutorial()
    {
        transition = Instantiate(Resources.Load<GameObject>("TransitionAnim")).GetComponentInChildren<Animator>();
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Tutorial");
    }
}
