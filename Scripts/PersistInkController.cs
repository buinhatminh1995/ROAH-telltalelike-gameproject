using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistInkController : MonoBehaviour
{
    private static PersistInkController _instance;

    public static PersistInkController Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            var ink = _instance.GetComponent<InkScript>();
            ink.customStartKnot = GetComponent<InkScript>().customStartKnot;
            ink.Start();

            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}