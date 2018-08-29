using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistInteractionUICanvas : MonoBehaviour
{
    private static PersistInteractionUICanvas _instance;

    public static PersistInteractionUICanvas Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}