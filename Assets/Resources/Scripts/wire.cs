using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wire : component
{
    public bool active = false;
    public GameObject wireframe;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void negate()
    {
        negateImage();
    }

    public void negateImage()
    {
        Texture2D negatedImage = new Texture2D(128, 128);
        Sprite newSprite = Sprite.Create(negatedImage, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f), 128);
        Color[] pixels = shape.GetComponent<SpriteRenderer>().sprite.texture.GetPixels();

        for(int i = 0; i < pixels.Length; i++)
        {
            pixels[i].a = 1 - pixels[i].a;
        }

        negatedImage.SetPixels(pixels);
        negatedImage.Apply();

        shape.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    public void changeShape(GameObject g)
    {
        GameObject gCopy = Instantiate(g);
        Destroy(shape);
        gCopy.transform.parent = gameObject.transform;
        gCopy.transform.localPosition = new Vector2();
        gCopy.transform.rotation = g.transform.rotation;
        shape = gCopy;
        shape.SetActive(true);

        if (ghost)
        {
            shape.GetComponent<SpriteRenderer>().material = transparent;
            shape.GetComponent<SpriteRenderer>().sortingOrder = -2;
        }
        else
        {
            shape.GetComponent<SpriteRenderer>().material = regular;
            shape.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
    }

    public void activate(bool on)
    {
        active = on;
        wireframe.SetActive(on);
    }

    public void wireSplit(int spriteNum)
    {
        switch (spriteNum)
        {
            case 0:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireZero");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireOne");
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case 5:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireOne");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 7:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireOne");
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 11:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireOne");
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case 8:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwo");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 18:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwo");
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 10:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwoCorner");
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case 14:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwoCorner");
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case 12:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwoCorner");
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 16:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwoCorner");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 15:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireThree");
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case 19:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireThree");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 23:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireThree");
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 21:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireThree");
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case 26:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireFour");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            default:
                Debug.Log("pee");
                break;
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

        shape.GetComponent<SpriteRenderer>().material = m;
        GetComponent<SpriteRenderer>().material = m;
        wireframe.GetComponent<SpriteRenderer>().material = m;

        shape.GetComponent<SpriteRenderer>().sortingOrder += renderLayerAddition;
        GetComponent<SpriteRenderer>().sortingOrder += renderLayerAddition;
        wireframe.GetComponent<SpriteRenderer>().sortingOrder += renderLayerAddition;


        ghost = t;
    }
}
