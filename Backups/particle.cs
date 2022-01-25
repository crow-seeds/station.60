using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour
{
    int locX = 0;
    int locY = 0;
    public wirePlacer wireSystem;
    public bool original;

    float timer = 0;
    public int direction = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!original)
        {
            direction = decideDirection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!original)
        {
            timer += Time.deltaTime;
            if(timer >= 1)
            {
                timer = 0;
                direction = decideDirection();
                Debug.Log(direction);
            }
            else
            {
                move();
            }
        }
    }

    void move()
    {
        if(direction == 0)
        {
            this.transform.Translate(0, Time.deltaTime, 0, Space.World);
        }else if(direction == 1)
        {
            this.transform.Translate(Time.deltaTime, 0, 0, Space.World);
        }
        else if (direction == 2)
        {
            this.transform.Translate(0, -Time.deltaTime, 0, Space.World);
        }
        else
        {
            this.transform.Translate(-Time.deltaTime, 0, 0, Space.World);
        }
    }

    public void setLocation(int x, int y)
    {
        locX = x;
        locY = y;
    }

    int decideDirection()
    {
        int ret = 0;
        int neighborNumber = wireSystem.getNeighbors(locX, locY, true);

        Debug.Log(neighborNumber);
        switch (neighborNumber)
        {
            case 0:
                Destroy(this.gameObject);
                break;
            case 3:
                if(direction == 3)
                {
                    Destroy(this.gameObject);
                }
                ret = 1;
                break;
            case 5:
                if (direction == 1)
                {
                    Destroy(this.gameObject);
                }
                ret = 3;
                break;
            case 7:
                if (direction == 0)
                {
                    Destroy(this.gameObject);
                }
                ret = 2;
                break;
            case 11:
                if (direction == 2)
                {
                    Destroy(this.gameObject);
                }
                ret = 0;
                break;
            case 8:
                ret = direction;
                break;
            case 18:
                ret = direction;
                break;
            case 10:
                if(direction == 0)
                {
                    ret = 1;
                }
                else
                {
                    ret = 2;
                }
                break;
            case 14:
                if (direction == 3)
                {
                    ret = 0;
                }
                else
                {
                    ret = 1;
                }
                break;
            case 12:
                if (direction == 1)
                {
                    ret = 2;
                }
                else
                {
                    ret = 3;
                }
                break;
            case 16:
                if (direction == 1)
                {
                    ret = 0;
                }
                else
                {
                    ret = 3;
                }
                break;
        }

        if(ret == 2)
        {
            locY--;
        }else if(ret == 1)
        {
            locX++;
        }else if(ret == 0)
        {
            locY++;
        }
        else
        {
            locX--;
        }

        return ret;
    }
}
