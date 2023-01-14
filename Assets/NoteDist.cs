using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDist : MonoBehaviour
{
    //PlayerPrefs a;
    public GameObject note, trigger, fail;
    // Пока что int, потом надо будет сделать структуру с данными об атаке
    public Queue<int> AttackQueue = new Queue<int>();
    //
    public Queue<KeyValuePair<int, GameObject>> EnemyAttackQueue = new Queue<KeyValuePair<int, GameObject>>();
    GameObject canvas;
    public Sprite up, down, left, right;
    //Временно
    public GameObject dmgSrc = null;

    bool underAttack;

    // Start is called before the first frame update
    void Start()
    {
        underAttack = false;
        canvas = GameObject.Find("Canvas");
        CreateNT(25 + 0 * 60, 200, KeyCode.UpArrow, up, Resources.Load<AudioClip>("SFX/newnote1"));
        CreateNT(25 + 1 * 60, 200, KeyCode.LeftArrow, left, Resources.Load<AudioClip>("SFX/newnote2"));
        CreateNT(25 + 2 * 60, 200, KeyCode.RightArrow, right, Resources.Load<AudioClip>("SFX/newnote3"));
        CreateNT(25 + 3 * 60, 200, KeyCode.DownArrow, down, Resources.Load<AudioClip>("SFX/newnote4"));
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyAttackQueue.Count > 0 && !underAttack)
        {
            KeyValuePair<int, GameObject> kvp = EnemyAttackQueue.Dequeue();
            if (kvp.Value != null)
                ProvideEnemyAttack(kvp.Key, kvp.Value, true);
        }
        //if (Input.GetKeyUp(KeyCode.Escape))
        //{
        //    Instantiate(note, new Vector3(25, 200), note.transform.rotation, canvas.transform);
        //}
    }

    void CreateNT(int x, int y, KeyCode key, Sprite sp, AudioClip clip)
    {
        //Instantiate(note, new Vector3(x,y), note.transform.rotation, canvas.transform);
        GameObject t = Instantiate(trigger, new Vector3(x, y - 150), note.transform.rotation, canvas.transform);
        t.GetComponent<RectTransform>().anchorMax = Vector3.zero;
        t.GetComponent<RectTransform>().anchorMin = Vector3.zero;
        t.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y - 150);
        t.GetComponent<CollisionTest>().key = key;
        t.GetComponent<CollisionTest>().audio = clip;
        t.GetComponent<UnityEngine.UI.Image>().sprite = sp;
        Instantiate(fail, new Vector3(x, y - 220), note.transform.rotation, t.transform);
    }

    private IEnumerator Delay(float delay, int notes, Color? color = null)
    {
        int r;
        underAttack = true;
        //GameObject[] noteArray = new GameObject[notes];
        for (int i = 0; i < notes; i++)
        {
            //Debug.Log("Start: " + i + " " + Time.fixedTime);
            r = Random.Range(0, 4);
            //noteArray[i] = Instantiate(note, new Vector3(25 + r * 60, 200), note.transform.rotation, canvas.transform);

            note.GetComponent<Note>().fallSpeed = 300; // Сложность? Легко 150-200, Нормально 200-250, Сложно 250-300
            note.GetComponent<UnityEngine.UI.Image>().color = color ?? Color.white;

            Instantiate(note, new Vector3(25 + r * 60, 200), note.transform.rotation, canvas.transform);
            yield return new WaitForSeconds(delay);
        }
        // Длительность существования одной ноты (в данном случае последней) за вычетом 0.5 стандартного ожидания
        yield return new WaitForSeconds(delay <= 0.86f ? 0.86f - delay : 0);
        dmgSrc = null;
        underAttack = false;
    }

    //public void ProvideAttack(int notes = 4, bool fromQueue = false)
    //{
    //    // 0.86 fixedTime на каждую из нот то есть:
    //    // delay * кол-во нот + 0.86 время гарантированного исчезновения последней ноты
    //    if (!underAttack && (AttackQueue.Count == 0 || fromQueue))
    //        StartCoroutine(Delay(0.5f, notes));//Instantiate(note, new Vector3(25 + i * 60, 200), note.transform.rotation, canvas.transform);
    //    else
    //    {
    //        Debug.Log("Атака записана в очередь");
    //        //if (AttackQueue.Count <= 2)
    //        AttackQueue.Enqueue(notes);
    //    }
    //}

    public void ProvideSimpleAttack(int notes = 2, Color? color = null)
    {
        Debug.Log("Простая атака!");
        if (!underAttack)
            StartCoroutine(Delay(0.5f, notes, color ?? Color.white));
    }

    // Нужно разобраться с одновременной атакой больше чем одного врага // Разобрался)
    public void ProvideEnemyAttack(int notes = 4, GameObject damageSource = null, bool fromQueue = false)
    {
        if (GameObject.FindWithTag("Player") == null)
            return;
        if (!underAttack && (EnemyAttackQueue.Count == 0 || fromQueue))
        {
            if (damageSource != null)
            {
                dmgSrc = damageSource;
            }

            Debug.Log("Вражина: " + damageSource.name + "атакует");
            StartCoroutine(Delay(0.5f, notes));
        }
        else
        {
            Debug.Log("Атака записана в очередь");
            EnemyAttackQueue.Enqueue(new KeyValuePair<int, GameObject>(notes, damageSource));
        }
    }
}
