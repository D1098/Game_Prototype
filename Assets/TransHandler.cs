using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransHandler : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject transition;

    private void Awake()
    {
        transition = GameObject.FindWithTag("LevelTrans");
        // Оно могло быть где угодно, просто не хотелось отдельный скрипт делать
        if (transition != null)
        {
            transition.GetComponentInChildren<Animator>().SetTrigger("Continue");
            Destroy(transition, 1.5f);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
