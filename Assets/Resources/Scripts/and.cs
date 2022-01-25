using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class and : twoInputGate
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void inputShape(GameObject s, inOut i, int x, int y, bool on, int z, float time)
    {

        if (inputs == 0 && on)
        {
            input1 = s.GetComponent<SpriteRenderer>().sprite.texture;
            inputs++;
        }
        else if (inputs == 1 && on)
        {
            input2 = s.GetComponent<SpriteRenderer>().sprite.texture;

            Color[] pixels1 = input1.GetPixels();
            Color[] pixels2 = input2.GetPixels();

            for (int j = 0; j < pixels1.Length; j++)
            {
                if (pixels1[j].a >= .9f && pixels2[j].a >= .9f)
                {
                    pixels1[j].a = 1;
                }
                else
                {
                    pixels1[j].a = 0;
                }
            }

            Texture2D orTexture = new Texture2D(128, 128);
            orTexture.SetPixels(pixels1);
            orTexture.Apply();

            shape.GetComponent<SpriteRenderer>().sprite = Sprite.Create(orTexture, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f), 128, 0, SpriteMeshType.FullRect);
            inputs++;
            i.delayedActivate(x, y, shape, "and", true, z, time);
        }
        else if (!on)
        {
            i.delayedActivate(x, y, i.offShape, "and", false, z, time);
            inputs = 0;
        }
    }
}
