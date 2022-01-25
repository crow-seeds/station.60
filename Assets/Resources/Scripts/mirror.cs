using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class mirror : component
{
    public GameObject[] connectors;
    public GameObject[] directors;
    public Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        type = "mirror";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override int outOrIn(int dir) //false is output, true is input
    {
        if (direction == dir)
        {
            return 1;
        }
        return 0;
    }

    public override bool isInput(int dir)
    {
        if (direction == dir)
        {
            return true;
        }
        return false;
    }

    public override bool isOutput(int dir)
    {
        if (direction == dir)
        {
            return false;
        }
        return true;
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

    public override void rotate(int dir)
    {
        while (direction != dir)
        {
            rotate(true);
        }
    }

    public void inputShape(GameObject s, inOut i, int x, int y, bool on, int z, float time)
    {
        if (on)
        {
            Texture2D input = s.GetComponent<SpriteRenderer>().sprite.texture;
            Color[] pixels = input.GetPixels();

            bool mirrorVert = true;

            if(direction == 1 || direction == 3)
            {
                mirrorVert = false;
            }

            if (!mirrorVert)
            {
                Color[] mirroredPixels = new Color[pixels.Length];

                for (int j = 0; j < pixels.Length / 128; j++)
                {
                    System.Array.Copy(pixels, mirroredPixels.Length - (j + 1) * 128, mirroredPixels, j * 128, 128);
                }

                pixels = mirroredPixels;
            }
            else
            {
                Color[] mirroredPixels = new Color[pixels.Length];

                for (int j = 0; j < 128; j++)
                {
                    for (int k = 0; k < 128; k++)
                    {
                        mirroredPixels[j * 128 + k] = pixels[(j + 1) * 128 - k - 1];
                    }
                }

                pixels = mirroredPixels;
            }

            Texture2D notTexture = new Texture2D(128, 128);
            notTexture.SetPixels(pixels);
            notTexture.Apply();

            shape.GetComponent<SpriteRenderer>().sprite = Sprite.Create(notTexture, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f), 128, 0, SpriteMeshType.FullRect);
            i.delayedActivate(x, y, shape, "mirror", true, z, time);
        }
        else
        {
            i.delayedActivate(x, y, i.offShape, "mirror", false, z, time);
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
}
