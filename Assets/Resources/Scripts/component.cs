using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class component : MonoBehaviour
{
    public string type;
    public int direction = 0; //0 is up, 1 is right, 2 is down, 3 is left
    public bool original;
    public GameObject shape;

    public Material regular;
    public Material transparent;

    public bool ghost = false;

    public int x;
    public int y;
    public int z;

    public string textureName = "";

    public bool permament = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getType()
    {
        return type;
    }

    public virtual bool connect(int dir)
    {
        return true;
    }

    public virtual void unconnect(int dir)
    {

    }

    public virtual void rotate(bool dir) //false for clockwise, true for counterclockwise
    {

    }

    public virtual void rotate(int dir) //0 for up, 1 for right, 2 for down, 3 for left
    {

    }

    public virtual bool shouldConnect(int dir)
    {
        return true;
    }

    public virtual bool isInput(int dir)
    {
        return true;
    }

    public virtual bool isOutput(int dir)
    {
        return true;
    }

    public virtual int outOrIn(int dir) //0 for output, 1 for input, 2 for neither, 3 for both
    {
        return 3;
    }

    public virtual void makeTransparent(bool t) //true for translucent, false for opaque
    {

    }

    public void setPosition(int newX, int newY, int newZ)
    {
        x = newX;
        y = newY;
        z = newZ;
    }

    public virtual void protect()
    {
        permament = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
    }
}
