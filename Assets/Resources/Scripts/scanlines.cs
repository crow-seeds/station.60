using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scanlines : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float timer = 0;
    public GameObject scanLine;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 22.066)
        {
            timer = 0;
            scanLine.transform.position = new Vector2();
        }

        scanLine.transform.Translate(new Vector3(0, Time.deltaTime * -.6797788453f));
    }
}
