
using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
//using Image = UnityEngine.UIElements.Image;

public class TestCanvas : MonoBehaviour
{
    GameObject canvas;
    bool check = true;
    public GameObject fullHeart, emptyHeart;
    public List<GameObject> hearts;
    float playerHearts;

    GameObject[] keyIndicators;

    // Start is called before the first frame update
    void Start()
    {
        keyIndicators = new GameObject[3] { GameObject.Find("BronzeKeyIndicator") , GameObject.Find("SilverKeyIndicator"), GameObject.Find("GoldenKeyIndicator") };
        foreach (var key in keyIndicators)
            key.SetActive(false);
        playerHearts = GameObject.FindWithTag("Player").GetComponent<HealthSystem>().currentHealth;
        Debug.Log(playerHearts);
        hearts = new List<GameObject>((int)GameObject.FindWithTag("Player").GetComponent<HealthSystem>().maxHealth);
        //Note = Instantiate((GameObject)Resources.Load("Prefabs/Note", typeof(GameObject)));
        //Trigger = Instantiate((GameObject)Resources.Load("Prefabs/Trigger", typeof(GameObject)));
        canvas = GameObject.Find("Canvas");
        for (int i = 0; i < hearts.Capacity; i++)
        {
            hearts.Add(Instantiate(fullHeart, transform));
            hearts[i].transform.position += new Vector3(i * 50, 0);
        }
    }

    void Test()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (check)
            {
                CreateTile(200, 0, false);
                check = false;
            }
            GameObject img = new GameObject();
            img.AddComponent<CanvasRenderer>();
            img.AddComponent<RectTransform>();
            img.AddComponent<UnityEngine.UI.Image>();
            img.GetComponent<UnityEngine.UI.Image>().color = Color.red;//Random.Range(0,2) == 1? Color.red:Color.blue;
            img.transform.position = new Vector2(200, 200);//Vector2.one;
            img.transform.SetParent(canvas.transform);
            img.AddComponent<Rigidbody2D>();
            //img.GetComponent<Rigidbody2D>().gravityScale = 5f;
            img.AddComponent<BoxCollider2D>();
            img.GetComponent<BoxCollider2D>().size = new Vector2(100, 100);
            //img.GetComponent<BoxCollider2D>().size = new Vector2(200, 200);
            img.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            img.tag = "Note";
            img.AddComponent<Note>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Test();
        // UI Здоровье (пробное)
        if (GameObject.FindWithTag("Player") == null)
            return;
        if (playerHearts != GameObject.FindWithTag("Player").GetComponent<HealthSystem>().currentHealth)
        {
            if (playerHearts > GameObject.FindWithTag("Player").GetComponent<HealthSystem>().currentHealth)
            {
                for (int i = 0; i < playerHearts - GameObject.FindWithTag("Player").GetComponent<HealthSystem>().currentHealth; i++)
                {
                    Destroy(hearts[hearts.Count - 1]);
                    hearts.RemoveAt(hearts.Count - 1);
                }
                playerHearts = GameObject.FindWithTag("Player").GetComponent<HealthSystem>().currentHealth;
            }
            else
            {
                for (int i = (int)playerHearts; i < (int)GameObject.FindWithTag("Player").GetComponent<HealthSystem>().currentHealth; i++)
                {
                    hearts.Add(Instantiate(fullHeart, transform));
                    hearts[i].transform.position += new Vector3(i * 50, 0);
                }
                playerHearts = GameObject.FindWithTag("Player").GetComponent<HealthSystem>().currentHealth;
            }
        }
    }

    public void Restart()
    {
        if (check)
        {
            StartCoroutine(LoadLevel());
            check = false;
        }
    }

    public void ToMenu()
    {
        if (check)
        {
            StartCoroutine(ExitToMenu());
            check = false;
        }
    }

    IEnumerator LoadLevel()
    {
        TranslateAnimation t = new TranslateAnimation();
        t.transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator ExitToMenu()
    {
        TranslateAnimation t = new TranslateAnimation();
        t.transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Menu");
    }

    void CreateTile(int x, int y, bool gravity)
    {
        GameObject img = new GameObject();
        img.AddComponent<CanvasRenderer>();
        img.AddComponent<RectTransform>();
        img.AddComponent<UnityEngine.UI.Image>();
        img.GetComponent<UnityEngine.UI.Image>().color = Color.blue;//Random.Range(0,2) == 1? Color.red:Color.blue;
        img.transform.position = new Vector2(x, y);
        img.transform.SetParent(canvas.transform);

        img.AddComponent<Rigidbody2D>();
        img.AddComponent<BoxCollider2D>();
        img.GetComponent<BoxCollider2D>().size = new Vector2(100, 100);
        img.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        img.GetComponent<BoxCollider2D>().isTrigger = true;
        img.tag = "colf";
        if (!gravity)
            img.GetComponent<Rigidbody2D>().gravityScale = 0f;
        img.AddComponent<CollisionTest>();

        GameObject imgAddon = new GameObject();
        imgAddon.transform.position = new Vector2(x, y - 150);
        imgAddon.AddComponent<Rigidbody2D>();
        imgAddon.AddComponent<BoxCollider2D>();
        imgAddon.GetComponent<BoxCollider2D>().size = new Vector2(100, 100);
        imgAddon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        imgAddon.GetComponent<BoxCollider2D>().isTrigger = true;
        imgAddon.transform.SetParent(img.transform);
        imgAddon.AddComponent<CollisionAddon>();
    }

    public void toggleKeyIndicator(int keyIndex) => keyIndicators[keyIndex].SetActive(!keyIndicators[keyIndex].activeInHierarchy);

    public void toggleOnKeyIndicator(int keyIndex) => keyIndicators[keyIndex].SetActive(true);

}
