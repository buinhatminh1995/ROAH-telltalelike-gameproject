using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class ClickOpenClueImageBlurryBackgroundUI : MonoBehaviour {

    [SerializeField] public Sprite imageUI;

    EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = GameObject.FindObjectOfType<EventSystem>();

        var inspectable = GetComponent<Inspectable>();
        if (inspectable != null)
            inspectable.OnPlayerReachObserveAngle += discardparam => ViewClue();

        var inspectableNoCam = GetComponent<InspectableNoCam>();
        if (inspectableNoCam != null)
            inspectableNoCam.OnPlayerReachAndClickOnObject += ViewClue;

        var inspectableAnywhere = GetComponent<InspectableAnywhere>();
        if (inspectableAnywhere != null)
            inspectableAnywhere.OnClickFromAnywhere += ViewClue;

        var inventoryItem = GetComponent<InventoryItem>();
        if (inventoryItem != null)
            inventoryItem.OnInventoryClick += FireOnClickObjectWithClue;
    }


    //public event Action OnClickObjectWithClue = delegate { };
    public event Action<Sprite> OnClickObjectWithClue = delegate { };

    public void FireOnClickObjectWithClue()
    {
        print("fire view clue");
        OnClickObjectWithClue(imageUI);

        var backButton = GetComponent<ObjectClickTriggerBackButton>();
        if (backButton != null)
            backButton.FireOnObjectClickTriggerBackButton();
    }

    private void ViewClue()
    {
        print("view clue");
        if (eventSystem != null && !eventSystem.IsPointerOverGameObject())
        {
            OnClickObjectWithClue(imageUI);
        }
    }

}
