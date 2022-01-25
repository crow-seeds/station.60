using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aspectRatio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    int lastHeight = 0;
    int lastWidth = 0;


    // Update is called once per frame
    void Update()
    {
        int width = Screen.width;
        int height = Screen.height;

        if (lastWidth != width)
        {
            float heightAccordingToWidth = width / 16.0f * 9.0f;
            Screen.SetResolution(width, (int)Mathf.Round(heightAccordingToWidth), false, 0);
        }
        else if (lastHeight != height)
        {
            float widthAccordingToHeight = height / 9.0f * 16.0f;
            Screen.SetResolution((int)Mathf.Round(widthAccordingToHeight), height, false, 0);
        }

        lastWidth = width;
        lastHeight = height;
    }
}
