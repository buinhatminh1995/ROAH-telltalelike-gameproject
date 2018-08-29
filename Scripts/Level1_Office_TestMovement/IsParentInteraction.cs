using System;
using UnityEngine;

public class IsParentInteraction : MonoBehaviour {

    [SerializeField] private Collider[] interactableChildrenObjects;
    [SerializeField] private bool permanentCollider;
    private bool triggeredPermanently;

    private Ground[] grounds;
    private BackButtonController[] backbuttonControllers;

    private void Awake()
    {
        //when reaching destination of this object, disable the collider on this parent
        var inspectable = GetComponent<Inspectable>();
        if (inspectable != null)
            inspectable.OnPlayerReachObserveAngle += HandlePlayerReachObserveAngle;

        //when reaching destination of this object, disable the collider on this parent
        var inspectableNoCam = GetComponent<InspectableNoCam>();
        if (inspectableNoCam != null)
            inspectableNoCam.OnPlayerReachAndClickOnObject += HandlePlayerReachAndClickOnObject;

        grounds = FindObjectsOfType<Ground>();
        backbuttonControllers = FindObjectsOfType<BackButtonController>();
    }

    void HandlePlayerReachObserveAngle(ObserveAngle observable)
    {
        print("disable a parent object and enable its children objects collider");
        SetColliderState(false);
        SetHandlers(true);
    }

    void HandlePlayerReachAndClickOnObject()
    {
        print("disable a parent object and enable its children objects collider");
        SetColliderState(false);
        SetHandlers(true);
    }

    void HandlePlayerClickOnGround(ObserveAngle observable) {
        print("re-enable parent collider and disable its childrens colliders");
        SetColliderState();
        SetHandlers(false);
    }

    void HandleBackButtonClick() {
        print("re-enable parent colliders - back button clicked");
        SetColliderState();
        SetHandlers(false);
    }

    public void SetColliderState(bool parent = true)
    {
        if (permanentCollider && triggeredPermanently) return;
        
        Collider parentCollider = GetComponent<Collider>();
        if (parentCollider !=null)
            parentCollider.enabled = parent;

        for (int i = 0; i < interactableChildrenObjects.Length; i++)
        {
            interactableChildrenObjects[i].enabled = !parent;
        }
        if (!parent) triggeredPermanently = true;
    }

    private void SetHandlers(bool state)
    {
        //var grounds = FindObjectsOfType<Ground>();
        if (grounds != null)
            for (int i = 0; i < grounds.Length; i++)
            {
                if (state)
                    grounds[i].OnPlayerClickOnGroundObserveAngle += HandlePlayerClickOnGround;
                else
                    grounds[i].OnPlayerClickOnGroundObserveAngle -= HandlePlayerClickOnGround;
            }

        //var backbuttonControllers = FindObjectsOfType<BackButtonController>();
        if (backbuttonControllers != null)
            for (int i = 0; i < backbuttonControllers.Length; i++)
            {
            if (state)
                    backbuttonControllers[i].OnCamBackButtonClick += HandleBackButtonClick;
            else
                    backbuttonControllers[i].OnCamBackButtonClick -= HandleBackButtonClick;
            }
    }

    void OnDestroy()
    {
        var inspectable = GetComponent<Inspectable>();
        if (inspectable != null)
            inspectable.OnPlayerReachObserveAngle -= HandlePlayerReachObserveAngle;
        
        var inspectableNoCam = GetComponent<InspectableNoCam>();
        if (inspectableNoCam != null)
            inspectableNoCam.OnPlayerReachAndClickOnObject -= HandlePlayerReachAndClickOnObject;

        SetHandlers(false);
    }
}
