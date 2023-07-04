using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
public class NetworkSingelton<T> : NetworkBehaviour where T : NetworkBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //Debug.LogError(typeof(T).ToString() + " is missing.");
            }

            return _instance;
        }
    }


    protected virtual void Awake()
    {
        if (_instance != null)
        {
            //Debug.LogWarning("Second instance of " + typeof(T) + " created. Automatic self-destruct triggered.");
            Destroy(this.gameObject);
            return;
        }

        _instance = this as T;
        Init();
    }


    protected virtual void OnDestroy()
    {
        // if (_instance == this)
        // {
        //     _instance = null;
        // }
    }


    public virtual void Init()
    {
        //Debug.Log($"<color=#00FF00>INSTANCED-->> </color>" + this.GetType());
    }
}
