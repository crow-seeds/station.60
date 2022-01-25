using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class Accessibility : MonoBehaviour
{
    public PostProcessVolume postProcess;
    public GameObject scanlines1;
    public wirePlacer w;
    public AudioSource noiseThing;

    // Start is called before the first frame update
    void Start()
    {
        checkAccessSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void checkAccessSettings()
    {
        for (int i = 0; i < 6; i++)
        {
            string state = "ON";

            if (PlayerPrefs.GetInt("access" + i.ToString(), 0) == 1)
            {
                state = "OFF";
            }

            switch (i)
            {
                case 0:
                    LensDistortion l;
                    postProcess.profile.TryGetSettings(out l);
                    if (state == "ON")
                    {
                        l.intensity.value = 40;
                    }
                    else
                    {
                        l.intensity.value = 0f;
                    }
                    break;
                case 1:
                    ChromaticAberration c;
                    postProcess.profile.TryGetSettings(out c);
                    if (state == "ON")
                    {
                        c.intensity.value = 0.4f;
                    }
                    else
                    {
                        c.intensity.value = 0f;
                    }
                    break;
                case 2:
                    Grain s;
                    postProcess.profile.TryGetSettings(out s);
                    if (state == "ON")
                    {
                        s.intensity.value = 0.6f;
                    }
                    else
                    {
                        s.intensity.value = 0f;
                    }
                    break;
                case 3:
                    if (state == "ON")
                    {
                        scanlines1.SetActive(true);
                    }
                    else
                    {
                        scanlines1.SetActive(false);
                    }
                    break;
                case 4:
                    if (state == "ON")
                    {
                        w.hasStory = true;
                    }
                    else
                    {
                        w.hasStory = false;
                    }    
                    break;
                case 5:
                    if (state == "OFF")
                    {
                        w.hasNoise = false;
                        noiseThing.clip = null;
                    }
                    break;
            }
        }
    }
}
