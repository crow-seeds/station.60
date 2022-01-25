using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wirePlacer : MonoBehaviour
{
    public GameObject[,] grid = new GameObject[9, 17];

    public GameObject wire;
    public GameObject input;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition;
            placeWire(pos.x / Screen.width, pos.y / Screen.height, "wire");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Vector3 pos = Input.mousePosition;
            placeWire(pos.x / Screen.width, pos.y / Screen.height, "input");
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 pos = Input.mousePosition;
            removeWire(pos.x / Screen.width, pos.y / Screen.height);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 pos = Input.mousePosition;
            rotate(pos.x / Screen.width, pos.y / Screen.height, true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 pos = Input.mousePosition;
            rotate(pos.x / Screen.width, pos.y / Screen.height, false);
        }
    }

    void placeWire(float x, float y, string type)
    {
        int xPlacement = (int)(x * 17);
        int yPlacement = (int)(y * 9);

        if(grid[yPlacement, xPlacement] == null)
        {
            GameObject wirePlaced = null;

            switch (type)
            {
                case "wire":
                    wirePlaced = Instantiate(wire, new Vector2(xPlacement - 8, yPlacement - 4), Quaternion.Euler(0, 0, 0));
                    break;
                case "input":
                    wirePlaced = Instantiate(input, new Vector2(xPlacement - 8, yPlacement - 4), Quaternion.Euler(0, 0, 0));
                    break;
            }

            wirePlaced.GetComponent<component>().original = false;
            grid[yPlacement, xPlacement] = wirePlaced;

            updateWire(xPlacement, yPlacement, true);          
        }
    }

    void removeWire(float x, float y)
    {
        int xPlacement = (int)(x * 17);
        int yPlacement = (int)(y * 9);

        unconnector(xPlacement, yPlacement);
        
        Destroy(grid[yPlacement, xPlacement]);
        grid[yPlacement, xPlacement] = null;

        updateWire(xPlacement - 1, yPlacement, false);
        updateWire(xPlacement + 1, yPlacement, false);
        updateWire(xPlacement, yPlacement + 1, false);
        updateWire(xPlacement, yPlacement - 1, false);    
    }

    void unconnector(int x, int y)
    {
        if (grid[y, x] != null && grid[y,x].GetComponent<component>().type == "wire")
        {
            if (x + 1 < grid.GetLength(1) && grid[y, x + 1] != null && grid[y, x + 1].GetComponent<component>().direction == 3)
            {
                grid[y, x + 1].GetComponent<component>().unconnect();
            }

            if (x != 0 && grid[y, x - 1] != null && grid[y, x - 1].GetComponent<component>().direction == 1)
            {
                grid[y, x - 1].GetComponent<component>().unconnect();
            }

            if (y + 1 < grid.GetLength(0) && grid[y + 1, x] != null && grid[y + 1, x].GetComponent<component>().direction == 2)
            {
                grid[y + 1, x].GetComponent<component>().unconnect();
            }

            if (y != 0 && grid[y - 1, x] != null && grid[y - 1, x].GetComponent<component>().direction == 0)
            {
                grid[y - 1, x].GetComponent<component>().unconnect();
            }
        }
        
    }

    void updateWire(int x, int y, bool updateNeighbors)
    {
        if(x >= 0 && x < grid.GetLength(1) && y >= 0 && y < grid.GetLength(0) && grid[y,x] != null && grid[y,x].GetComponent<component>().type == "wire")
        {
            int spriteNum = getNeighbors(x, y, false);
            GameObject wirePlaced = grid[y, x];

            switch (spriteNum)
            {
                case 0:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireZero");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 3:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireOne");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 5:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireOne");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 7:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireOne");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 11:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireOne");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                case 8:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwo");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 18:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwo");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 10:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwoCorner");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 14:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwoCorner");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                case 12:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwoCorner");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 16:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireTwoCorner");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 15:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireThree");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 19:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireThree");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 23:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireThree");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 21:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireThree");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                case 26:
                    wirePlaced.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Wires/wireFour");
                    wirePlaced.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                default:
                    Debug.Log("pee");
                    break;
            }

            wirePlaced.GetComponent<SpriteMask>().sprite = wirePlaced.GetComponent<SpriteRenderer>().sprite;
        }

        if (updateNeighbors)
        {
            updateWire(x - 1, y, false);
            updateWire(x + 1, y, false);
            updateWire(x, y + 1, false);
            updateWire(x, y - 1, false);
        }

    }

    public int getNeighbors(int x, int y, bool onlyWires)
    {
        int ret = 0;
        component comp;

        if(x + 1 < grid.GetLength(1) && grid[y, x+1] != null) //right
        {
            comp = grid[y, x + 1].GetComponent<component>();
            if(comp.type == "wire" || (comp.type == "io" && comp.direction == 3 && !onlyWires))
            {
                comp.connect();
                ret += 3;
            }
        }

        if (x != 0 && grid[y, x-1] != null) //left
        {
            comp = grid[y, x - 1].GetComponent<component>();
            if (comp.type == "wire" || (comp.type == "io" && comp.direction == 1 && !onlyWires))
            {
                comp.connect();
                ret += 5;
            }
        }

        if (y + 1 < grid.GetLength(0) && grid[y + 1, x] != null) //up
        {
            comp = grid[y + 1, x].GetComponent<component>();
            if (comp.type == "wire" || (comp.type == "io" && comp.direction == 2 && !onlyWires))
            {
                comp.connect();
                ret += 11;
            }
        }
        if (y != 0 && grid[y - 1, x] != null) //down
        {
            comp = grid[y - 1, x].GetComponent<component>();
            if (comp.type == "wire" || (comp.type == "io" && comp.direction == 0 && !onlyWires))
            {
                comp.connect();
                ret += 7;
            }
        }

        return ret;
    }

    void rotate(float x, float y, bool direction) //false for clockwise, true for counterclockwise
    {
        int xPlacement = (int)(x * 17);
        int yPlacement = (int)(y * 9);

        grid[yPlacement, xPlacement].GetComponent<component>().rotate(direction);
        grid[yPlacement, xPlacement].GetComponent<component>().unconnect();

        getNeighbors(xPlacement, yPlacement, false);
        updateWire(xPlacement - 1, yPlacement, false);
        updateWire(xPlacement + 1, yPlacement, false);
        updateWire(xPlacement, yPlacement + 1, false);
        updateWire(xPlacement, yPlacement - 1, false);

        
    }
}
