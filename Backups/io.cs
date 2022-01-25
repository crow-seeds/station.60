using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class io : component
{
    public GameObject director;
    public GameObject connector;
    public string mode;
    public Color originalColor;
    public particle part;

    float timer = 0;

    void Start()
    {
        type = "io";
    }

    // Update is called once per frame
    void Update()
    {
       if (!original)
        {
            timer += Time.deltaTime;

            if (timer >= 2)
            {
                particle p = Instantiate(part, new Vector2(transform.localPosition.x, transform.localPosition.y), Quaternion.Euler(0, 0, 0));
                p.direction = direction;
                p.original = false;
                p.setLocation((int)transform.localPosition.x + 8, (int)transform.localPosition.y + 4);
                timer = 0;
            }
        }
        
    }

    public override void connect()
    {
        director.gameObject.GetComponent<SpriteRenderer>().color = new Color(1 - originalColor.r, 1 - originalColor.g, 1 - originalColor.b);
        connector.SetActive(true);
    }

    public override void unconnect()
    {
        director.gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        connector.SetActive(false);
    }

    public override void rotate(bool dir)
    {
        if (dir)
        {
            this.gameObject.transform.Rotate(Vector3.back, -90);
            direction--;
        }
        else
        {
            this.gameObject.transform.Rotate(Vector3.back, 90);
            direction++;
        }

        if (direction == 4)
        {
            direction = 0;
        }

        if (direction == -1)
        {
            direction = 3;
        }
    }
}
