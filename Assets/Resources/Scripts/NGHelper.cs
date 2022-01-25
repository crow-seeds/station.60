using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGHelper : MonoBehaviour
{
    public io.newgrounds.core ngio_core;

    // Start is called before the first frame update
    void Start()
    {
        ngio_core.onReady(() =>
        {
            ngio_core.checkLogin((bool logged_in) => {
                if (logged_in)
                {
                    onLoggedIn();
                }
                else
                {
                    requestLogIn();
                }

             });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onLoggedIn()
    {
        io.newgrounds.objects.user player = ngio_core.current_user;
    }

    void requestLogIn()
    {
        ngio_core.requestLogin(onLoggedIn);
    }

    public void unlockMedal(int medal_id)
    {
        io.newgrounds.components.Medal.unlock medal_unlock = new io.newgrounds.components.Medal.unlock();

        medal_unlock.id = medal_id;

        medal_unlock.callWith(ngio_core);
        Debug.Log("sent medal id " + medal_id);
    }
}
