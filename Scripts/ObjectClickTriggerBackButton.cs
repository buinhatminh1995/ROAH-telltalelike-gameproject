using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ObjectClickTriggerBackButton : MonoBehaviour {

    public event Action OnObjectClickTriggerBackButton = delegate { };
    EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
    }

    public void FireOnObjectClickTriggerBackButton()
    {
        OnObjectClickTriggerBackButton();
    }

    private void OnMouseDown()
    {
        if (eventSystem != null && !eventSystem.IsPointerOverGameObject())
        {
            OnObjectClickTriggerBackButton();
        }
    }
}
