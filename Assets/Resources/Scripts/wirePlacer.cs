using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class wirePlacer : MonoBehaviour
{
    public GameObject[,,] grid = new GameObject[5, 8, 17];

    public GameObject wire;
    public GameObject input;
    public GameObject negate;
    public GameObject or;
    public GameObject and;
    public GameObject xor;
    public GameObject amplify;
    public GameObject deamplify;
    public GameObject output;
    public GameObject mirror;

    public int currentLayer = 0;

    public List<GameObject> inputs;
    public List<component> activeItems;
    public TextMeshProUGUI volume;
    public TextMeshProUGUI leftVolume;
    public TextMeshProUGUI rightVolume;
    public TextMeshProUGUI leftVolumeText;
    public TextMeshProUGUI rightVolumeText;
    public TextMeshProUGUI channel;

    public List<TextMeshProUGUI> toolBarText;
    public List<RawImage> toolBarImages;
    public RawImage playButton;

    string objectBeingPlaced = "wire";

    public AudioSource backgroundNoise;
    public AudioSource soundEffects;

    public bool state = false; //false for off, true for on

    public gameData saveData;

    int activatedWires = 0;
    int highestComponentUnlocked = 0;

    public SpriteRenderer top;
    public GameObject statBackground;
    public TextMeshProUGUI stats;

    int compAmount = 0;
    int spaceAmount = 0;
    public bool hasStory = true;
    public bool hasNoise = true;

    public NGHelper ng;

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        compareData();
        syncNewgroundsAchievements();
    }

    bool hasScrolled = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !state && !onStatic)
        {
            Vector3 pos = Input.mousePosition;
            placeWire(pos.x / Screen.width, pos.y / Screen.height, objectBeingPlaced, false);
        }else if(Input.GetMouseButtonDown(0) && state && !onStatic)
        {
            Vector3 pos = Input.mousePosition;
            if(pos.y / Screen.height < .9f)
            {
                activation();
            }
            
        }
        else if (Input.GetMouseButton(0) && !state && !onStatic)
        {
            Vector3 pos = Input.mousePosition;
            placeWire(pos.x / Screen.width, pos.y / Screen.height, objectBeingPlaced, true);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 pos = Input.mousePosition;
            copySelect(pos.x / Screen.width, pos.y / Screen.height);
        }

        if (Input.GetKeyDown(KeyCode.W) && highestComponentUnlocked >= 1)
        {
            toolBar("negate");
        }

        if (Input.GetKeyDown(KeyCode.D) && highestComponentUnlocked >= 2)
        {
            toolBar("or");
        }

        if (Input.GetKeyDown(KeyCode.A) && highestComponentUnlocked >= 3)
        {
            toolBar("and");
        }

        if (Input.GetKeyDown(KeyCode.S) && highestComponentUnlocked >= 4)
        {
            toolBar("xor");
        }

        if (Input.GetKeyDown(KeyCode.C) && highestComponentUnlocked >= 5)
        {
            toolBar("amplify");
        }

        if (Input.GetKeyDown(KeyCode.V) && highestComponentUnlocked >= 5)
        {
            toolBar("deamplify");
        }

        if (Input.GetKeyDown(KeyCode.X) && highestComponentUnlocked >= 7)
        {
            toolBar("mirror");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            toolBar("wire");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeChannel(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeChannel(true);
        }

        if (Input.GetMouseButton(1) && !state)
        {
            Vector3 pos = Input.mousePosition;
            removeWire(pos.x / Screen.width, pos.y / Screen.height);
        }

        if (Input.GetKeyDown(KeyCode.Q) && !state)
        {
            Vector3 pos = Input.mousePosition;
            rotate(pos.x / Screen.width, pos.y / Screen.height, true);
        }

        if (Input.GetKeyDown(KeyCode.E) && !state)
        {
            Vector3 pos = Input.mousePosition;
            rotate(pos.x / Screen.width, pos.y / Screen.height, false);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !hasScrolled)
        {
            toolBar("up");
            hasScrolled = true;
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0 && !hasScrolled)
        {
            toolBar("down");
            hasScrolled = true;
        } else if (Input.GetAxis("Mouse ScrollWheel") == 0 && hasScrolled)
        {
            hasScrolled = false;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            activation();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backToMenu();
        }

        if (Input.GetKeyDown(KeyCode.R) && highestComponentUnlocked >= 5)
        {
            changeLayer(true);
        }

        if (Input.GetKeyDown(KeyCode.F) && highestComponentUnlocked >= 5)
        {
            changeLayer(false);
        }
    }


    public void activation()
    {
        Debug.Log("pee");
        state = !state;
        turnOnSystem(state);

        amountOfCorrectOutputs = 0;

        if (!state)
        {
            playButton.texture = Resources.Load<Texture>("Sprites/Components/play");
            soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/pause"));
        }
        else
        {
            playButton.texture = Resources.Load<Texture>("Sprites/Components/pause");
            soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/play"));
        }
    }

    void turnOnSystem(bool on) //true for turn on, false for turn off
    {
        foreach (GameObject g in inputs)
        {
            if (g != null)
            {
                g.GetComponent<inOut>().activate(on);
            }
        }
    }

    void placeWire(float x, float y, string type, bool held)
    {
        int xPlacement = (int)(x * 17);
        int yPlacement = (int)(y * 9);

        if (inBounds(xPlacement, yPlacement) && grid[currentLayer, yPlacement, xPlacement] == null)
        {
            GameObject wirePlaced = null;

            spaceAmount++;
            compAmount++;
            switch (type)
            {
                case "wire":
                    wirePlaced = Instantiate(wire, new Vector3(xPlacement - 8, yPlacement - 4, currentLayer), Quaternion.Euler(0, 0, 0));
                    compAmount--;
                    break;
                case "input":
                    wirePlaced = Instantiate(input, new Vector3(xPlacement - 8, yPlacement - 4, currentLayer), Quaternion.Euler(0, 0, -90));
                    inputs.Add(wirePlaced);
                    compAmount--;
                    spaceAmount--;
                    break;
                case "negate":
                    wirePlaced = Instantiate(negate, new Vector3(xPlacement - 8, yPlacement - 4, currentLayer), Quaternion.Euler(0, 0, -90));
                    break;
                case "or":
                    wirePlaced = Instantiate(or, new Vector3(xPlacement - 8, yPlacement - 4, currentLayer), Quaternion.Euler(0, 0, -90));
                    break;
                case "and":
                    wirePlaced = Instantiate(and, new Vector3(xPlacement - 8, yPlacement - 4, currentLayer), Quaternion.Euler(0, 0, -90));
                    break;
                case "xor":
                    wirePlaced = Instantiate(xor, new Vector3(xPlacement - 8, yPlacement - 4, currentLayer), Quaternion.Euler(0, 0, -90));
                    break;
                case "amplify":
                    wirePlaced = Instantiate(amplify, new Vector3(xPlacement - 8, yPlacement - 4, currentLayer), Quaternion.Euler(0, 0, 0));
                    break;
                case "deamplify":
                    wirePlaced = Instantiate(deamplify, new Vector3(xPlacement - 8, yPlacement - 4, currentLayer), Quaternion.Euler(0, 0, 0));
                    break;
                case "mirror":
                    wirePlaced = Instantiate(mirror, new Vector3(xPlacement - 8, yPlacement - 4, currentLayer), Quaternion.Euler(0, 0, -90));
                    break;
                case "output":
                    wirePlaced = Instantiate(output, new Vector3(xPlacement - 8, yPlacement - 4, currentLayer), Quaternion.Euler(0, 0, -90));
                    compAmount--;
                    spaceAmount--;
                    break;
            }

            wirePlaced.GetComponent<component>().original = false;
            grid[currentLayer, yPlacement, xPlacement] = wirePlaced;
            soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/placeTick"));

            wirePlaced.GetComponent<component>().setPosition(xPlacement, yPlacement, currentLayer);
            activeItems.Add(wirePlaced.GetComponent<component>());
            updateWire(xPlacement, yPlacement, true, currentLayer);
        }
        else if (inBounds(xPlacement, yPlacement) && !held)
        {
            rotate(x, y, false);
        }
    }

    void removeWire(float x, float y)
    {
        int xPlacement = (int)(x * 17);
        int yPlacement = (int)(y * 9);

        if (exists(xPlacement, yPlacement, currentLayer) && grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().type != "input" && grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().type != "output" && activatedWires == 0 && !grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().permament)
        {
            spaceAmount--;
            compAmount--;

            if (grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().type == "wire")
            {
                compAmount++;
            }

            unconnector(xPlacement, yPlacement);
            soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/placeTickReverse"));

            Destroy(grid[currentLayer, yPlacement, xPlacement]);
            grid[currentLayer, yPlacement, xPlacement] = null;

            updateWire(xPlacement - 1, yPlacement, false, currentLayer);
            updateWire(xPlacement + 1, yPlacement, false, currentLayer);
            updateWire(xPlacement, yPlacement + 1, false, currentLayer);
            updateWire(xPlacement, yPlacement - 1, false, currentLayer);
        }
    }

    void copySelect(float x, float y)
    {
        int xPlacement = (int)(x * 17);
        int yPlacement = (int)(y * 9);

        if (exists(xPlacement, yPlacement, currentLayer))
        {
            if (grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().type != "input" && grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().type != "output")
            {
                toolBar(grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().type);
            }
        }
    }

    void unconnector(int x, int y)
    {
        if (grid[currentLayer, y, x] != null && grid[currentLayer, y, x].GetComponent<component>().type == "wire")
        {

            if (x + 1 < grid.GetLength(2) && grid[currentLayer, y, x + 1] != null && grid[currentLayer, y, x + 1].GetComponent<component>().shouldConnect(3))
            {
                grid[currentLayer, y, x + 1].GetComponent<component>().unconnect(3);
            }

            if (x != 0 && grid[currentLayer, y, x - 1] != null && grid[currentLayer, y, x - 1].GetComponent<component>().shouldConnect(1))
            {
                grid[currentLayer, y, x - 1].GetComponent<component>().unconnect(1);
            }

            if (y + 1 < grid.GetLength(1) && grid[currentLayer, y + 1, x] != null && grid[currentLayer, y + 1, x].GetComponent<component>().shouldConnect(2))
            {
                grid[currentLayer, y + 1, x].GetComponent<component>().unconnect(2);
            }

            if (y != 0 && grid[currentLayer, y - 1, x] != null && grid[currentLayer, y - 1, x].GetComponent<component>().shouldConnect(0))
            {
                grid[currentLayer, y - 1, x].GetComponent<component>().unconnect(0);
            }
        }

    }

    void updateWire(int x, int y, bool updateNeighbors, int z)
    {
        if (x >= 0 && x < grid.GetLength(2) && y >= 0 && y < grid.GetLength(1) && grid[z, y, x] != null)
        {
            int spriteNum = getNeighbors(x, y, z);
            GameObject wirePlaced = grid[z, y, x];

            if (grid[z, y, x].GetComponent<component>().type == "wire")
            {
                grid[z, y, x].GetComponent<wire>().wireSplit(spriteNum);
            }
            else
            {
                if (!exists(x + 1, y, z) || (exists(x + 1, y, z) && !(areCompatible(grid[z, y, x + 1].GetComponent<component>(), grid[z, y, x].GetComponent<component>(), 3))))
                {
                    grid[z, y, x].GetComponent<component>().unconnect(1);

                    if (exists(x + 1, y, z))
                    {
                        grid[z, y, x + 1].GetComponent<component>().unconnect(3);
                    }
                }

                if (!exists(x - 1, y, z) || (exists(x - 1, y, z) && !(areCompatible(grid[z, y, x - 1].GetComponent<component>(), grid[z, y, x].GetComponent<component>(), 1))))
                {
                    grid[z, y, x].GetComponent<component>().unconnect(3);

                    if (exists(x - 1, y, z))
                    {
                        grid[z, y, x - 1].GetComponent<component>().unconnect(1);
                    }
                }

                if (!exists(x, y + 1, z) || (exists(x, y + 1, z) && !(areCompatible(grid[z, y + 1, x].GetComponent<component>(), grid[z, y, x].GetComponent<component>(), 2))))
                {
                    grid[z, y, x].GetComponent<component>().unconnect(0);

                    if (exists(x, y + 1, z))
                    {
                        grid[z, y + 1, x].GetComponent<component>().unconnect(2);
                    }
                }

                if (!exists(x, y - 1, z) || (exists(x, y - 1, z) && !(areCompatible(grid[z, y - 1, x].GetComponent<component>(), grid[z, y, x].GetComponent<component>(), 0))))
                {
                    grid[z, y, x].GetComponent<component>().unconnect(2);

                    if (exists(x, y - 1, z))
                    {
                        grid[z, y - 1, x].GetComponent<component>().unconnect(0);
                    }
                }
            }
        }

        if (updateNeighbors)
        {
            getNeighbors(x, y, z);
            updateWire(x - 1, y, false, z);
            updateWire(x + 1, y, false, z);
            updateWire(x, y + 1, false, z);
            updateWire(x, y - 1, false, z);
        }

    }

    bool areCompatible(component a, component b, int dir) //dir is direction of a
    {
        int altDir = (dir + 2) % 4;
        return (a.isInput(dir) && b.isOutput(altDir)) || (a.isOutput(dir) && b.isInput(altDir));
    }

    public int getNeighbors(int x, int y, int z)
    {
        int ret = 0;
        component comp;

        if (x + 1 < grid.GetLength(2) && grid[z, y, x + 1] != null) //right
        {
            comp = grid[z, y, x + 1].GetComponent<component>();
            if (comp.connect(3))
            {
                ret += 3;
            }
        }

        if (x != 0 && grid[z, y, x - 1] != null) //left
        {
            comp = grid[z, y, x - 1].GetComponent<component>();
            if (comp.connect(1))
            {
                ret += 5;
            }

        }

        if (y + 1 < grid.GetLength(1) && grid[z, y + 1, x] != null) //up
        {
            comp = grid[z, y + 1, x].GetComponent<component>();
            if (comp.connect(2))
            {
                ret += 11;
            }

        }

        if (y != 0 && grid[z, y - 1, x] != null) //down
        {
            comp = grid[z, y - 1, x].GetComponent<component>();
            if (comp.connect(0))
            {
                ret += 7;
            }
        }

        return ret;
    }

    public bool inBounds(int x, int y)
    {
        return x < grid.GetLength(2) && x >= 0 && y >= 0 && y < grid.GetLength(1);
    }

    public bool exists(int x, int y, int z)
    {
        return inBounds(x, y) && grid[z, y, x] != null;
    }

    void rotate(float x, float y, bool direction) //false for clockwise, true for counterclockwise
    {
        int xPlacement = (int)(x * 17);
        int yPlacement = (int)(y * 9);

        if (grid[currentLayer, yPlacement, xPlacement] != null && grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().type != "input" && grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().type != "output" && activatedWires == 0 && !grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().permament)
        {
            grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().rotate(direction);
            grid[currentLayer, yPlacement, xPlacement].GetComponent<component>().unconnect(4);

            getNeighbors(xPlacement, yPlacement, currentLayer);
            updateWire(xPlacement, yPlacement, false, currentLayer);
            updateWire(xPlacement - 1, yPlacement, false, currentLayer);
            updateWire(xPlacement + 1, yPlacement, false, currentLayer);
            updateWire(xPlacement, yPlacement + 1, false, currentLayer);
            updateWire(xPlacement, yPlacement - 1, false, currentLayer);
        }
    }

    public void changeLayer(bool up) //true for increase layer, false for decrease
    {
        if ((up && currentLayer + 1 < grid.GetLength(0)) || (!up && currentLayer != 0))
        {
            for (int i = 0; i < grid.GetLength(1); i++)
            {
                for (int j = 0; j < grid.GetLength(2); j++)
                {
                    if (grid[currentLayer, i, j] != null)
                    {
                        grid[currentLayer, i, j].GetComponent<component>().makeTransparent(true);
                    }
                }
            }

            if (up)
            {
                currentLayer++;
                backgroundNoise.volume += .1f;
            }
            else
            {
                currentLayer--;
                backgroundNoise.volume -= .1f;
            }

            int volumeSoundEffect = UnityEngine.Random.Range(0, 5);
            soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/volume" + volumeSoundEffect.ToString()), .6f);

            for (int i = 0; i < grid.GetLength(1); i++)
            {
                for (int j = 0; j < grid.GetLength(2); j++)
                {
                    if (grid[currentLayer, i, j] != null)
                    {
                        grid[currentLayer, i, j].GetComponent<component>().makeTransparent(false);
                    }
                }
            }

            if (currentLayer == 0)
            {
                leftVolume.color = new Color(1, 1, 1, .4f);
                leftVolumeText.color = new Color(1, 1, 1, .4f);
            }
            else
            {
                leftVolume.color = new Color(1, 1, 1);
                leftVolumeText.color = new Color(1, 1, 1);
            }

            if (currentLayer + 1 == grid.GetLength(0))
            {
                rightVolume.color = new Color(1, 1, 1, .4f);
                rightVolumeText.color = new Color(1, 1, 1, .4f);
            }
            else
            {
                rightVolume.color = new Color(1, 1, 1);
                rightVolumeText.color = new Color(1, 1, 1);
            }

            volume.text = "VOL " + currentLayer.ToString();
        }
    }
    int position = 0;
    List<string> componentNames = new List<string>(new string[] { "wire", "negate", "or", "and", "xor", "amplify", "deamplify", "mirror" });

    public void toolBar(string s)
    {
        Color trans = new Color(1, 1, 1, .4f);
        Color white = new Color(1, 1, 1);

        toolBarText[position].color = trans;
        toolBarImages[position].color = trans;

        string toBeUsed = s;

        if (s != "up" && s != "down")
        {
            switch (s)
            {
                case "wire":
                    position = 0;
                    break;
                case "negate":
                    position = 1;
                    break;
                case "or":
                    position = 2;
                    break;
                case "and":
                    position = 3;
                    break;
                case "xor":
                    position = 4;
                    break;
                case "amplify":
                    position = 5;
                    break;
                case "deamplify":
                    position = 6;
                    break;
                case "mirror":
                    position = 7;
                    break;
            }
        }
        else
        {
            if (s == "up")
            {
                position++;
            }
            else
            {
                position--;
            }

            if (position < 0)
            {
                position = highestComponentUnlocked;
            }

            if (position > highestComponentUnlocked)
            {
                position = 0;
            }

            toBeUsed = componentNames[position];
        }




        toolBarText[position].color = white;
        toolBarImages[position].color = white;
        objectBeingPlaced = toBeUsed;
    }

    public void SaveData()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath + "/Saves/save.binary");

        saveData.setLevelData(convertGridToString());

        formatter.Serialize(saveFile, saveData);

        saveFile.Close();
    }

    public void LoadData()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Debug.Log("water");
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        BinaryFormatter formatter = new BinaryFormatter();

        if (!File.Exists(Application.persistentDataPath + "/Saves/save.binary"))
        {
            Debug.Log("fire");
            saveData = new gameData();
            FileStream newFile = File.Create(Application.persistentDataPath + "/Saves/save.binary");
            formatter.Serialize(newFile, saveData);
            newFile.Close();
        }


        FileStream saveFile = File.Open(Application.persistentDataPath + "/Saves/save.binary", FileMode.Open);

        saveData = (gameData)formatter.Deserialize(saveFile);
        convertStringToGrid(saveData.getLevelData(saveData.currentChannel));

        saveFile.Close();

        channel.text = "CH " + saveData.currentChannel.ToString();
        clearSubtitles();
        unlockComponents();
        StartCoroutine(startSubtitles(saveData.getSubtitleData()));

        if (saveData.hasCompletedLevel())
        {
            top.color = new Color(.4f, .4f, .4f);
            statBackground.SetActive(true);
            stats.text = getStats();
        }
    }

    string getStats()
    {
        List<float> dat = saveData.getStats();
        string ret = "SPACE " + dat[0].ToString() + " / COMP " + dat[1].ToString() + " / TIME " + Math.Round(dat[2], 2).ToString();
        return ret;
    }

    List<string> convertGridToString()
    {
        List<string> ret = new List<string>();

        foreach (component c in activeItems)
        {
            if (c != null && c.gameObject != null)
            {
                string s = "";
                if (c.permament)
                {
                    s += '0';
                }
                s += c.type + "," + c.x.ToString() + "," + c.y.ToString() + "," + c.z.ToString() + "," + c.direction.ToString() + "," + c.textureName;
                ret.Add(s);
            }
        }

        return ret;
    }

    public GameObject TVstatic;
    public GameObject gridTexture;
    bool onStatic = false;

    void convertStringToGrid(List<string> data)
    {
        if (data.Count == 0)
        {
            TVstatic.SetActive(true);
            gridTexture.SetActive(false);
            onStatic = true;
        }
        else
        {
            onStatic = false;
            TVstatic.SetActive(false);
            gridTexture.SetActive(true);
        }

        foreach (string s in data)
        {
            string[] attributes = s.Split(',');
            placeData(Int32.Parse(attributes[1]), Int32.Parse(attributes[2]), Int32.Parse(attributes[3]), attributes[0], Int32.Parse(attributes[4]), attributes[5]);
        }
    }

    void placeData(int xPlacement, int yPlacement, int zPlacement, string type, int direction, string texture)
    {
        if (inBounds(xPlacement, yPlacement) && grid[zPlacement, yPlacement, xPlacement] == null)
        {
            GameObject wirePlaced = null;

            bool shouldProtect = false;

            if (type[0] == '0')
            {
                shouldProtect = true;
                type = type.Remove(0, 1);
            }

            spaceAmount++;
            compAmount++;
            switch (type)
            {
                case "wire":
                    wirePlaced = Instantiate(wire, new Vector3(xPlacement - 8, yPlacement - 4, zPlacement), Quaternion.Euler(0, 0, 0));
                    compAmount--;
                    break;
                case "input":
                    wirePlaced = Instantiate(input, new Vector3(xPlacement - 8, yPlacement - 4, zPlacement), Quaternion.Euler(0, 0, -90));
                    wirePlaced.GetComponent<inOut>().setTexture(texture);
                    inputs.Add(wirePlaced);
                    compAmount--;
                    spaceAmount--;
                    break;
                case "negate":
                    wirePlaced = Instantiate(negate, new Vector3(xPlacement - 8, yPlacement - 4, zPlacement), Quaternion.Euler(0, 0, -90));
                    break;
                case "or":
                    wirePlaced = Instantiate(or, new Vector3(xPlacement - 8, yPlacement - 4, zPlacement), Quaternion.Euler(0, 0, -90));
                    break;
                case "and":
                    wirePlaced = Instantiate(and, new Vector3(xPlacement - 8, yPlacement - 4, zPlacement), Quaternion.Euler(0, 0, -90));
                    break;
                case "xor":
                    wirePlaced = Instantiate(xor, new Vector3(xPlacement - 8, yPlacement - 4, zPlacement), Quaternion.Euler(0, 0, -90));
                    break;
                case "amplify":
                    wirePlaced = Instantiate(amplify, new Vector3(xPlacement - 8, yPlacement - 4, zPlacement), Quaternion.Euler(0, 0, 0));
                    break;
                case "deamplify":
                    wirePlaced = Instantiate(deamplify, new Vector3(xPlacement - 8, yPlacement - 4, zPlacement), Quaternion.Euler(0, 0, 0));
                    break;
                case "mirror":
                    wirePlaced = Instantiate(mirror, new Vector3(xPlacement - 8, yPlacement - 4, zPlacement), Quaternion.Euler(0, 0, -90));
                    break;
                case "output":
                    wirePlaced = Instantiate(output, new Vector3(xPlacement - 8, yPlacement - 4, zPlacement), Quaternion.Euler(0, 0, -90));
                    wirePlaced.GetComponent<output>().setTexture(texture);
                    amountOfOutputs++;
                    compAmount--;
                    spaceAmount--;
                    break;
            }

            wirePlaced.GetComponent<component>().original = false;
            grid[zPlacement, yPlacement, xPlacement] = wirePlaced;

            wirePlaced.GetComponent<component>().rotate(direction);
            if (zPlacement != currentLayer)
            {
                wirePlaced.GetComponent<component>().makeTransparent(true);
            }

            if (shouldProtect)
            {
                wirePlaced.GetComponent<component>().protect();
            }

            wirePlaced.GetComponent<component>().setPosition(xPlacement, yPlacement, zPlacement);
            activeItems.Add(wirePlaced.GetComponent<component>());
            updateWire(xPlacement, yPlacement, true, zPlacement);
        }
    }

    void clearAll()
    {
        for (int i = 0; i < activeItems.Count; i++)
        {
            if (activeItems[i] != null)
            {
                grid[activeItems[i].z, activeItems[i].y, activeItems[i].x] = null;
                Destroy(activeItems[i].gameObject);
            }
        }
        activeItems.Clear();
        inputs.Clear();
    }

    public void changeChannel(bool direction)
    {
        if (saveData.inRange(direction))
        {
            spaceAmount = 0;
            compAmount = 0;
            totalTime = 0;
            if (!onStatic)
            {
                SaveData();
            }
            clearAll();
            clearSubtitles();
            amountOfOutputs = 0;
            amountOfCorrectOutputs = 0;
            saveData.changeChannel(direction);
            convertStringToGrid(saveData.getLevelData(saveData.currentChannel));

            channel.text = "CH " + saveData.currentChannel.ToString();

            state = false;
            playButton.texture = Resources.Load<Texture>("Sprites/Components/play");

            int channelSoundEffect = UnityEngine.Random.Range(0, 2);

            soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/channel" + channelSoundEffect.ToString()));
            StartCoroutine(temporarySilence());

            activatedWires = 0;

            top.color = new Color(0f, 0f, 0f);
            statBackground.SetActive(false);
            stats.text = "";

            unlockComponents();
            StartCoroutine(startSubtitles(saveData.getSubtitleData()));
        }
    }

    public GameObject blackCovering;

    IEnumerator temporarySilence()
    {
        backgroundNoise.Pause();
        blackCovering.SetActive(true);
        yield return new WaitForSeconds(.7f);

        if (saveData.hasCompletedLevel())
        {
            top.color = new Color(.3f, .3f, .3f);
        }

        blackCovering.SetActive(false);

        if (hasNoise)
        {
            if (onStatic)
            {
                backgroundNoise.clip = Resources.Load<AudioClip>("Sound/whiterNoise");
            }
            else
            {
                backgroundNoise.clip = Resources.Load<AudioClip>("Sound/whiteNoise");
            }
        }
        

        if (saveData.hasCompletedLevel())
        {
            top.color = new Color(.4f, .4f, .4f);
            statBackground.SetActive(true);
            stats.text = getStats();
        }

        backgroundNoise.Play();
    }

    public void activationWireAmount(bool b) //changes activation wire amount so people dont delete wires when there are any active wires
    {
        if (b)
        {
            activatedWires++;
        }
        else
        {
            activatedWires--;
        }
    }

    int amountOfOutputs = 0;
    int amountOfCorrectOutputs = 0;
    float totalTime = 0;

    public void outputCorrect(float time)
    {
        amountOfCorrectOutputs++;

        if (amountOfOutputs == amountOfCorrectOutputs)
        {
            totalTime = time;
            saveData.setStats(spaceAmount, compAmount, totalTime);
            stats.text = getStats();

            if (saveData.highestChannelUnlocked >= 1)
            {
                ng.unlockMedal(65656);
            }
            if (saveData.highestChannelUnlocked >= 3)
            {
                ng.unlockMedal(65657);
            }
            if (saveData.highestChannelUnlocked >= 10)
            {
                ng.unlockMedal(65658);
            }
            if (saveData.highestChannelUnlocked >= 17)
            {
                ng.unlockMedal(65659);
            }
            if (saveData.highestChannelUnlocked >= 20)
            {
                ng.unlockMedal(65660);
            }
            if (saveData.highestChannelUnlocked >= 26)
            {
                ng.unlockMedal(65661);
            }

            if (saveData.completedLevel())
            {
                changeChannel(true);

                if (saveData.currentChannel == 30)
                {
                    clearSubtitles();
                    ng.unlockMedal(65662);
                    StartCoroutine(ending());
                }
            }
        }
    }

    void unlockComponents()
    {
        highestComponentUnlocked = saveData.componentUnlocks();

        for (int i = 0; i <= highestComponentUnlocked; i++)
        {
            toolBarImages[i].gameObject.SetActive(true);
            toolBarText[i].gameObject.SetActive(true);
        }

        if (highestComponentUnlocked >= 5)
        {
            rightVolume.gameObject.SetActive(true);
            leftVolume.gameObject.SetActive(true);
        }
    }

    public TextMeshProUGUI subtitlesObject;

    IEnumerator startSubtitles(List<string> subtitles)
    {
        yield return new WaitForSeconds(1f);
        if (hasStory)
        {
            foreach (string s in subtitles)
            {
                AudioClip voice = Resources.Load<AudioClip>("Sound/chirp");

                bool marimba = false;
                string quote = s;

                switch (quote[0])
                {
                    case '0':
                        voice = Resources.Load<AudioClip>("Sound/chirp");
                        break;
                    case '1':
                        voice = Resources.Load<AudioClip>("Sound/chirp2");
                        break;
                    case '2':
                        voice = Resources.Load<AudioClip>("Sound/chirp3");
                        break;
                    case '3':
                        marimba = true;
                        break;
                    case '*':
                        voice = Resources.Load<AudioClip>("Sound/chirp3");
                        if (Application.platform != RuntimePlatform.WebGLPlayer)
                        {
                            quote += (Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\to_monkey\\letter.txt").Remove(32);
                        }
                        else
                        {
                            quote += "crowseeds.com/STATION/letter.txt";
                        }
                        break;
                    case '$':
                        voice = Resources.Load<AudioClip>("Sound/chirp3");
                        if (Application.platform != RuntimePlatform.WebGLPlayer)
                        {
                            quote += (Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\to_monkey\\letter.txt").Remove(0, 32);
                        }
                        else
                        {
                            quote += "cool website btw";
                        }
                        writeThreateningLetter();
                        break;
                }

                quote = quote.Remove(0, 1);

                for (int i = 0; i < quote.Length; i++)
                {
                    subtitlesObject.text += quote[i];
                    if (!marimba)
                    {
                        soundEffects.PlayOneShot(voice);
                        yield return new WaitForSeconds(.05f);
                    }
                    else
                    {
                        int mod = i % 12;
                        soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/chirp4-" + mod.ToString()));
                        yield return new WaitForSeconds(.15f);
                    }
                }

                yield return new WaitForSeconds(3f);

                int size = subtitlesObject.text.Length;

                for (int i = 0; i < size; i++)
                {
                    subtitlesObject.text = subtitlesObject.text.Remove(subtitlesObject.text.Length - 1);
                    yield return new WaitForSeconds(.05f);
                }
            }
        }
        
    }



    void clearSubtitles()
    {
        StopAllCoroutines();

        subtitlesObject.text = "";
    }

    public GameObject screenCover;

    public void backToMenu()
    {
        SaveData();
        screenCover.SetActive(true);
        subtitlesObject.gameObject.SetActive(false);
        backgroundNoise.Stop();
        soundEffects.gameObject.SetActive(false);
        backgroundNoise.PlayOneShot(Resources.Load<AudioClip>("Sound/power"));
        StartCoroutine(powerOff());
    }

    IEnumerator powerOff()
    {
        yield return new WaitForSeconds(.7f);
        SceneManager.LoadScene("menu");
    }

    void compareData()
    {
        gameData temp = new gameData();

        if (temp.getAmountOfLevels() != saveData.getAmountOfLevels())
        {
            int l = saveData.getAmountOfLevels();

            for (int i = l; i < temp.getAmountOfLevels(); i++)
            {
                saveData.addLevelData(temp.getLevelDataFullAccess(i));
                saveData.addSubtitleData(temp.getSubtitleDataFullAccess(i));
            }
        }
    }

    void writeThreateningLetter()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {

        }
        else
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/to_monkey/letter.txt";

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/to_monkey"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/to_monkey");
            }

            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/to_monkey/letter.txt"))
            {
                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine("im going to break your legs monkey and then cook them up for dinner if you dont leave. -fermi");
                writer.Close();
            }
        }
    }

    List<string> endingText = new List<string> { "0Thank you." };
    IEnumerator ending()
    {
        soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/power"));
        screenCover.SetActive(true);
        backgroundNoise.Stop();
        yield return new WaitForSeconds(2f);
        StartCoroutine(startSubtitles(endingText));
        yield return new WaitForSeconds(5f);
        soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sound/power"));
        StartCoroutine(powerOff());
    }
    void syncNewgroundsAchievements()
    {
        if (saveData.highestChannelUnlocked >= 1)
        {
            ng.unlockMedal(65656);
        }
        if (saveData.highestChannelUnlocked >= 3)
        {
            ng.unlockMedal(65657);
        }
        if (saveData.highestChannelUnlocked >= 10)
        {
            ng.unlockMedal(65658);
        }
        if (saveData.highestChannelUnlocked >= 17)
        {
            ng.unlockMedal(65659);
        }
        if (saveData.highestChannelUnlocked >= 20)
        {
            ng.unlockMedal(65660);
        }
        if (saveData.highestChannelUnlocked >= 26)
        {
            ng.unlockMedal(65661);
        }
        if (saveData.highestChannelUnlocked >= 30)
        {
            ng.unlockMedal(65662);
        }
    }
}
