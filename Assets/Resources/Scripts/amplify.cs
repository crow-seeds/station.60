using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class amplify : component
{
    public GameObject director;
    public GameObject connector;
    public Color originalColor;
    public bool active = false;
    public SpriteRenderer mainSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool connect(int dir)
    {
        if (dir == direction)
        {
            director.GetComponent<SpriteRenderer>().color = new Color(1 - originalColor.r, 1 - originalColor.g, 1 - originalColor.b);
            connector.SetActive(true);
            return true;
        }
        return false;
    }

    public override bool shouldConnect(int dir)
    {
        return dir == direction;
    }

    public override void unconnect(int dir)
    {
        if (dir == direction || dir == 4)
        {
            director.GetComponent<SpriteRenderer>().color = originalColor;
            connector.SetActive(false);
        }
    }

    public override int outOrIn(int dir)
    {
        if(dir == direction)
        {
            return 1;
        }
        return 6;
    }

    public override bool isInput(int dir)
    {
        if(dir == direction)
        {
            return true;
        }
        return false;
    }

    public override bool isOutput(int dir)
    {
        return false;
    }

    public override void rotate(bool dir)
    {
        if (dir)
        {
            gameObject.transform.Rotate(Vector3.back, -90);
            shape.transform.Rotate(Vector3.back, 90);
            direction--;
        }
        else
        {
            gameObject.transform.Rotate(Vector3.back, 90);
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

    public override void rotate(int dir)
    {
        while (direction != dir)
        {
            rotate(true);
        }
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


    public void inputShape(GameObject s, inOut i, int x, int y, bool on, int z, string previousOp, float time)
    {
        int newZ = z;

        if(type == "amplify")
        {
            newZ++;
        }
        else
        {
            newZ--;
        }

        if (on && !active)
        {
            if(previousOp == "deamplify" && type == "amplify" || previousOp == "amplify" && type == "deamplify")
            {
                if(direction == 0)
                {
                    i.layerActivate(x, y + 4, s, true, newZ, "", time);
                }

                if (direction == 1)
                {
                    i.layerActivate(x + 4, y, s, true, newZ, "", time);
                }

                if (direction == 2)
                {
                    i.layerActivate(x, y - 4, s, true, newZ, "", time);
                }

                if (direction == 3)
                {
                    i.layerActivate(x - 4, y, s, true, newZ, "", time);
                }
            }
            else
            {
                i.layerActivate(x, y, s, true, newZ, type, time);
            }
            active = true;
        }
        else if(!on && active)
        {
            if (previousOp == "deamplify" && type == "amplify" || previousOp == "amplify" && type == "deamplify")
            {
                if (direction == 0)
                {
                    i.layerActivate(x, y + 4, s, false, newZ, "", time);
                }

                if (direction == 1)
                {
                    i.layerActivate(x + 4, y, s, false, newZ, "", time);
                }

                if (direction == 2)
                {
                    i.layerActivate(x, y - 4, s, false, newZ, "", time);
                }

                if (direction == 3)
                {
                    i.layerActivate(x - 4, y, s, false, newZ, "", time);
                }
            }
            else
            {
                i.layerActivate(x, y, s, false, newZ, type, time);
            }
            active = false;
        }
    }

    public override void protect()
    {
        permament = true;
        mainSprite.color = new Color(1, 0, 0);
        originalColor = new Color(1, 0, 0);

        connector.GetComponent<SpriteRenderer>().color = originalColor;
        director.GetComponent<SpriteRenderer>().color = originalColor;
    }
}
