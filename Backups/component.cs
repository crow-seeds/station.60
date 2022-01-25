using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class component : MonoBehaviour
{
    public string type;
    public int direction = 0; //0 is up, 1 is right, 2 is down, 3 is left
    public bool original;

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

    public virtual void connect()
    {

    }

    public virtual void unconnect()
    {

    }

    public virtual void rotate(bool dir) //false for clockwise, true for counterclockwise
    {

    }
}
