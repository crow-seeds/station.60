using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twoInputGate : component
{
    public GameObject[] connectors;
    public GameObject[] directors;
    public Color originalColor;

    public int inputs = 0;

    // Start is called before the first frame update
    void Start()
    {
        type = "or";
        shape.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override int outOrIn(int dir) //false is output, true is input
    {
        if(direction - dir == 2 || direction - dir == -2 || direction - dir == 1 || direction - dir == -3)
        {
            return 1;
        }
        return 0;
    }

    public override bool isInput(int dir)
    {
        if(outOrIn(dir) == 1)
        {
            return true;
        }
        return false;
    }

    public override bool isOutput(int dir)
    {
        if (outOrIn(dir) == 0)
        {
            return true;
        }
        return false;
    }

    public override bool connect(int dir)
    {
        int adjustedDir = rotateDir(dir);
        directors[adjustedDir].GetComponent<SpriteRenderer>().color = new Color(1 - originalColor.r, 1 - originalColor.g, 1 - originalColor.b);
        connectors[adjustedDir].SetActive(true);
        return true;
    }

    public int rotateDir(int dir)
    {
        int ret = dir - direction;
        if (ret < 0)
        {
            ret += 4;
        }
        return ret;
    }

    public override void rotate(int dir)
    {
        while (direction != dir)
        {
            rotate(true);
        }
    }

    public override void unconnect(int dir)
    {
        if (dir == 4)
        {
            foreach (GameObject g in connectors)
            {
                g.SetActive(false);
            }

            foreach (GameObject g in directors)
            {
                g.GetComponent<SpriteRenderer>().color = originalColor;
            }
        }
        else
        {
            int adjustedDir = rotateDir(dir);
            connectors[adjustedDir].SetActive(false);
            directors[adjustedDir].GetComponent<SpriteRenderer>().color = originalColor;
        }
    }

    public override void rotate(bool dir)
    {
        if (dir)
        {
            this.gameObject.transform.Rotate(Vector3.back, -90);
            shape.transform.Rotate(Vector3.back, 90);
            direction--;
        }
        else
        {
            this.gameObject.transform.Rotate(Vector3.back, 90);
            shape.transform.Rotate(Vector3.back, -90);
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

    public Texture2D input1;
    public Texture2D input2;

    public virtual void inputShape(GameObject s, inOut i, int x, int y, bool on, int z, float time)
    {

    }

    public override void makeTransparent(bool t)
    {
        Material m = regular;
        int renderLayerAddition = 5;
        if (t)
        {
            m = transparent;
            renderLayerAddition = -5;
        }

        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().material = m;
            child.GetComponent<SpriteRenderer>().sortingOrder += renderLayerAddition;
        }

        ghost = t;
    }

    public SpriteRenderer mainSprite;

    public override void protect()
    {
        permament = true;
        mainSprite.color = new Color(1, 0, 0);
        originalColor = new Color(1, 0, 0);

        foreach (GameObject c in connectors)
        {
            c.GetComponent<SpriteRenderer>().color = originalColor;
        }

        foreach (GameObject c in directors)
        {
            c.GetComponent<SpriteRenderer>().color = originalColor;
        }
    }
}
