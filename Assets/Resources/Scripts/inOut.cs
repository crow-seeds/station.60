using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inOut : component
{
    public GameObject director;
    public GameObject connector;
    public Color originalColor;
    public GameObject offShape;

    public wirePlacer system;
    public GameObject[,,] grid;

    public AudioSource soundEffects;

    void Start()
    {
        type = "input";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void protect()
    {
        permament = true;
        shape.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        originalColor = new Color(1, 0, 0);

        connector.GetComponent<SpriteRenderer>().color = originalColor;
        director.GetComponent<SpriteRenderer>().color = originalColor;
    }

    public void activate(bool on)
    {
        if (!original)
        {
            int x = (int)transform.localPosition.x + 8;
            int y = (int)transform.localPosition.y + 4;
            int z = (int)transform.localPosition.z;

            GameObject sourceShape = shape;

            grid = system.grid;

            if (!on)
            {
                sourceShape = offShape;
            }

            if (direction == 0)
            {
                StartCoroutine(power(x, y + 1, on, sourceShape, "", z, 0));
            }
            else if (direction == 1)
            {
                StartCoroutine(power(x + 1, y, on, sourceShape, "", z, 0));
            }
            else if (direction == 2)
            {
                StartCoroutine(power(x, y - 1, on, sourceShape, "", z, 0));
            }
            else
            {
                StartCoroutine(power(x - 1, y, on, sourceShape, "", z, 0));
            }
        }
    }

    public override int outOrIn(int dir) //0 is output, 1 is input, 2 is neither
    {
        if(dir == direction)
        {
            return 0;
        }
        return 2;
    }

    public override bool isInput(int dir)
    {
        return false;
    }

    public override bool isOutput(int dir)
    {
        if (dir == direction)
        {
            return true;
        }
        return false;
    }

    public IEnumerator power(int x, int y, bool on, GameObject newShape, string operation, int currentLayer, float time) //true is turn on, false is turn off
    {   
        yield return new WaitForSeconds(.05f);

        if (system.inBounds(x, y) && grid[currentLayer, y, x] != null)
        {
            time += .05f;
            if (grid[currentLayer, y, x].GetComponent<component>().type == "wire" && ((!grid[currentLayer, y, x].GetComponent<wire>().active && on) || (grid[currentLayer, y, x].GetComponent<wire>().active && !on)))
            {
                system.grid[currentLayer, y, x].GetComponent<wire>().activate(on);
                system.grid[currentLayer, y, x].GetComponent<wire>().changeShape(newShape);

                system.activationWireAmount(on);

                if (on)
                {
                    soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/wireTransfer"), .3f + .02f * currentLayer);
                }
                else
                {
                    soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/wireStop"), .3f + .02f * currentLayer);
                }
                

                if (x + 1 < grid.GetLength(2) && grid[currentLayer, y, x + 1] != null && (grid[currentLayer, y, x + 1].GetComponent<component>().outOrIn(3) == 1 || grid[currentLayer, y, x + 1].GetComponent<component>().outOrIn(3) == 3))
                {
                    StartCoroutine(power(x + 1, y, on, system.grid[currentLayer, y, x].GetComponent<component>().shape, "", currentLayer, time));
                }

                if (x != 0 && grid[currentLayer, y, x - 1] != null && (grid[currentLayer, y, x - 1].GetComponent<component>().outOrIn(1) == 1 || grid[currentLayer, y, x - 1].GetComponent<component>().outOrIn(1) == 3))
                {
                    StartCoroutine(power(x - 1, y, on, system.grid[currentLayer, y, x].GetComponent<component>().shape, "", currentLayer, time));
                }

                if (y + 1 < grid.GetLength(1) && grid[currentLayer, y + 1, x] != null && (grid[currentLayer, y + 1, x].GetComponent<component>().outOrIn(2) == 1 || grid[currentLayer, y + 1, x].GetComponent<component>().outOrIn(2) == 3))
                {
                    StartCoroutine(power(x, y + 1, on, system.grid[currentLayer, y, x].GetComponent<component>().shape, "", currentLayer, time));
                }

                if (y != 0 && grid[currentLayer, y - 1, x] != null && (grid[currentLayer, y - 1, x].GetComponent<component>().outOrIn(0) == 1 || grid[currentLayer, y - 1, x].GetComponent<component>().outOrIn(0) == 3))
                {
                    StartCoroutine(power(x, y - 1, on, system.grid[currentLayer, y, x].GetComponent<component>().shape, "", currentLayer, time));
                }

                
            }
            else if(grid[currentLayer, y, x].GetComponent<component>().type == "negate")
            {
                if (on)
                {
                    soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/negateOff"), .5f + .02f * currentLayer);
                }
                else
                {
                    soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/wireStop"), .3f + .02f * currentLayer);
                }
                
                grid[currentLayer, y, x].GetComponent<negate>().inputShape(newShape, this, x, y, on, currentLayer, time);
            }
            else if(grid[currentLayer, y, x].GetComponent<component>().type == "or" || grid[currentLayer, y, x].GetComponent<component>().type == "and" || grid[currentLayer, y, x].GetComponent<component>().type == "xor")
            {
                if (on)
                {
                    soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/twoInput"), .3f + .02f * currentLayer);
                }
                else
                {
                    soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/wireStop"), .3f + .02f * currentLayer);
                }

                grid[currentLayer, y, x].GetComponent<twoInputGate>().inputShape(newShape, this, x, y, on, currentLayer, time);
            }
            else if(grid[currentLayer, y, x].GetComponent<component>().type == "amplify" || grid[currentLayer, y, x].GetComponent<component>().type == "deamplify")
            {
                if (on)
                {
                    soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/amplify"), .4f + .02f * currentLayer);
                }
                else
                {
                    soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/wireStop"), .3f + .02f * currentLayer);
                }

                grid[currentLayer, y, x].GetComponent<amplify>().inputShape(newShape, this, x, y, on, currentLayer, operation, time);
            }else if(grid[currentLayer, y, x].GetComponent<component>().type == "output")
            {
                if(grid[currentLayer, y, x].GetComponent<output>().imageCheck(newShape, on))
                {
                    system.outputCorrect(time);
                }
            }else if(grid[currentLayer, y, x].GetComponent<component>().type == "mirror")
            {
                if (on)
                {
                    soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/mirror"), .5f + .02f * currentLayer);
                }
                else
                {
                    soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/wireStop"), .3f + .02f * currentLayer);
                }

                grid[currentLayer, y, x].GetComponent<mirror>().inputShape(newShape, this, x, y, on, currentLayer, time);
            }
        }
        
    }

    public void delayedActivate(int x, int y, GameObject newShape, string mode, bool on, int currentLayer, float time)
    {
            if (grid[currentLayer, y, x].GetComponent<component>().outOrIn(1) == 0 && x + 1 < grid.GetLength(2) && grid[currentLayer, y, x + 1] != null && grid[currentLayer, y, x + 1].GetComponent<component>().isInput(3))
            {
                StartCoroutine(power(x + 1, y, on, newShape, mode, currentLayer, time));
            }

            if (grid[currentLayer, y, x].GetComponent<component>().outOrIn(3) == 0 && x != 0 && grid[currentLayer, y, x - 1] != null && grid[currentLayer, y, x - 1].GetComponent<component>().isInput(1))
            {
                StartCoroutine(power(x - 1, y, on, newShape, mode, currentLayer, time));
            }

            if (grid[currentLayer, y, x].GetComponent<component>().outOrIn(0) == 0 && y + 1 < grid.GetLength(1) && grid[currentLayer, y + 1, x] != null && grid[currentLayer, y + 1, x].GetComponent<component>().isInput(2))
            {
                StartCoroutine(power(x, y + 1, on, newShape, mode, currentLayer, time));
            }

            if (grid[currentLayer, y, x].GetComponent<component>().outOrIn(2) == 0 && y != 0 && grid[currentLayer, y - 1, x] != null && grid[currentLayer, y - 1, x].GetComponent<component>().isInput(0))
            {
                StartCoroutine(power(x, y - 1, on, newShape, mode, currentLayer, time));
            }       
    }

    public void layerActivate(int x, int y, GameObject newShape, bool on, int currentLayer, string mode, float time)
    {
        if(currentLayer >= 0 && currentLayer < grid.GetLength(0))
        { 
            StartCoroutine(power(x, y, on, newShape, mode, currentLayer, time));
        }
    }


    public override bool connect(int dir)
    {
        if(dir == direction)
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
        while(direction != dir)
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

        foreach(Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().material = m;
            child.GetComponent<SpriteRenderer>().sortingOrder += renderLayerAddition;
        }

        ghost = t;
    }

    public void setTexture(string s)
    {
        textureName = s;
        shape.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Shapes/" + s);
    }
}
