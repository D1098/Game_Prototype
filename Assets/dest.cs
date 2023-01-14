using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        //Destroy(gameObject, 2.1f);
    }
}
