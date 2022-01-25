using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class output : component
{
    public GameObject director;
    public GameObject connector;
    public Color originalColor;

    public AudioSource soundEffects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override int outOrIn(int dir) //0 is output, 1 is input, 2 is neither
    {
        if (dir == direction)
        {
            return 1;
        }
        return 2;
    }

    public override bool isInput(int dir)
    {
        if (dir == direction)
        {
            return true;
        }
        return false;
    }

    public override bool isOutput(int dir)
    {
        return false;
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

    public bool imageCheck(GameObject g, bool on)
    {
        if (on)
        {
            Color[] newPixels = g.GetComponent<SpriteRenderer>().sprite.texture.GetPixels();
            Color[] originalPixels = shape.GetComponent<SpriteRenderer>().sprite.texture.GetPixels();

            bool matching = true;

            for (int i = 0; i < newPixels.Length; i++)
            {
                if (newPixels[i].a >= .8f != originalPixels[i].a >= .8f)
                {
                    Debug.Log(i);
                    matching = false;
                    break;
                }
            }

            if (matching)
            {
                soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/correct"), .5f);
                return true;
            }
            else
            {
                soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/wrong"), .7f);        
            }
        }

        return false;
    }

    public void setTexture(string s)
    {
        textureName = s;
        shape.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Shapes/" + s);
    }

    public override void protect()
    {
        permament = true;
        shape.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        originalColor = new Color(1, 0, 0);

        connector.GetComponent<SpriteRenderer>().color = originalColor;
        director.GetComponent<SpriteRenderer>().color = originalColor;
    }
}
