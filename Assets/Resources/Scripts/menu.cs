using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class menu : MonoBehaviour
{

    public GameObject main;
    public GameObject options;
    public GameObject accessability;
    public GameObject exitButton;
    public GameObject optionsButton;
    public GameObject scanlines2;
    public List<TextMeshProUGUI> accessOptions;
    public TextMeshProUGUI fullscreenText;
    public TextMeshProUGUI resolutionText;
    public Button resolutionButton;
    public PostProcessVolume postProcess;
    public AudioSource noise;

    int[] xResolutions = new int[] { 800, 960, 1280, 1600 };
    int[] yResolutions = new int[] { 450, 540, 720, 900 };

    int resolutionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            fullscreenText.text = ">FULLSCREEN";
            resolutionIndex = PlayerPrefs.GetInt("resolutionIndex", 0);
            resolutionText.text = ">" + Screen.width.ToString() + " x " + Screen.height.ToString();
        }
        else
        {
            resolutionButton.interactable = false;
        }

        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            optionsButton.SetActive(false);
            exitButton.SetActive(false);
        }

        checkAccessSettings();
    }

    float timer = 0;
    float timer2 = 0;
    Color black = new Color(0, 0, 0, 1);
    Color white = new Color(0, 0, 0, 0);
    Color seventy = new Color(0,0,0,.3f);
    Color eighty = new Color(0,0,0,.15f);
    public SpriteRenderer overlay;
    public GameObject scanlines;

    bool off = false;
    public AudioSource sound;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;

        if(timer > .87f && timer <= 4.2 && !off)
        {
            overlay.color = Color.Lerp(black, seventy, (timer - .87f) / 3.33f);
        }else if(timer > 4.2 && timer <= 4.4 && !off)
        {
            overlay.color = Color.Lerp(seventy, black, (timer - 4.2f) / .1f);
        }else if(timer > 6.1 && timer < 11 && !off)
        {
            overlay.color = Color.Lerp(black, eighty, (timer - 6.1f) / 4.9f);
        }

        else if(timer2 > 22.066)
        {
            timer2 = 0;
            sound.Stop();
            sound.clip = Resources.Load<AudioClip>("Sound/menuLoop");
            sound.loop = true;
            sound.Play();
            scanlines.transform.position = new Vector2();
        }

        scanlines.transform.Translate(new Vector3(0, Time.deltaTime * -.6797788453f));
    }

    public void turnOn()
    {
        off = true;
        overlay.color = black;
        sound.Stop();
        sound.PlayOneShot(Resources.Load<AudioClip>("Sound/power"));
        StartCoroutine(load());
    }

    IEnumerator load()
    {
        yield return new WaitForSeconds(.7f);
        SceneManager.LoadScene("main");
    }

    

    public void toggleOptions(bool t)
    {
        main.SetActive(t);
        options.SetActive(!t);
        sound.PlayOneShot(Resources.Load<AudioClip>("Sound/volume0"));
    }

    public void toggleAccess(bool t)
    {
        main.SetActive(t);
        accessability.SetActive(!t);
        sound.PlayOneShot(Resources.Load<AudioClip>("Sound/volume0"));
    }

    public void toggleFullscreen()
    {
        if(Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            fullscreenText.text = ">FULLSCREEN";
            resolutionButton.interactable = true;
            Screen.SetResolution(xResolutions[resolutionIndex], yResolutions[resolutionIndex], false);
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            fullscreenText.text = ">WINDOW";
            resolutionButton.interactable = false;
        }

        sound.PlayOneShot(Resources.Load<AudioClip>("Sound/volume1"));

    }

    public void setResolution()
    {
        resolutionIndex++;

        sound.PlayOneShot(Resources.Load<AudioClip>("Sound/volume2"));

        Debug.Log(resolutionIndex);

        if (resolutionIndex >= xResolutions.Length)
        {
            resolutionIndex = 0;
        }

        Screen.SetResolution(xResolutions[resolutionIndex], yResolutions[resolutionIndex], false);
        resolutionText.text = ">" + xResolutions[resolutionIndex].ToString() + " x " + yResolutions[resolutionIndex].ToString();

        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
        
    }

    public void exit()
    {
        off = true;
        overlay.color = black;
        sound.Stop();
        sound.PlayOneShot(Resources.Load<AudioClip>("Sound/power"));
        StartCoroutine(powerOff());
    }

    IEnumerator powerOff()
    {
        yield return new WaitForSeconds(.3f);
        Application.Quit();
    }

    public void accessOption(int i)
    {
        if(PlayerPrefs.GetInt("access"+i.ToString(), 0) == 0)
        {
            PlayerPrefs.SetInt("access"+i.ToString(), 1);
        }
        else
        {
            PlayerPrefs.SetInt("access" + i.ToString(), 0);
        }

        checkAccessSettings();

        sound.PlayOneShot(Resources.Load<AudioClip>("Sound/volume2"));

        
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
                    accessOptions[0].text = ">LENS DISTORTION: " + state;
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
                    accessOptions[1].text = ">CHROMATIC ABERATION: " + state;
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
                    accessOptions[2].text = ">STATIC: " + state;
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
                    accessOptions[3].text = ">SCANLINES: " + state;
                    if (state == "ON")
                    {
                        scanlines.SetActive(true);
                        scanlines2.SetActive(true);
                    }
                    else
                    {
                        scanlines.SetActive(false);
                        scanlines2.SetActive(false);
                    }
                    break;
                case 4:
                    accessOptions[4].text = ">STORY: " + state;
                    break;
                case 5:
                    accessOptions[5].text = ">NOISE: " + state;
                    if (state == "ON")
                    {
                        if (!noise.isPlaying)
                        {
                            timer = 0;
                            timer2 = 0;
                            noise.Play();
                        }
                        
                        
                    }
                    else
                    {
                        Debug.Log("pause!");
                        noise.Stop();                    
                    }
                    break;
            }
        }
    }
}
