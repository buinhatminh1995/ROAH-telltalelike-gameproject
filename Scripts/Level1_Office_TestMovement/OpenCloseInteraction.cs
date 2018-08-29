using System;
using System.Collections;
using UnityEngine;

public class OpenCloseInteraction : MonoBehaviour, ICanClickToActivate
{
    [SerializeField] private Animator animationController;
    [SerializeField] private string openAnimStateName = "open";
    [SerializeField] private float openAnimTime = 2.1f;
    [SerializeField] private string closeAnimStateName = "close";
    [SerializeField] private float closeAnimTime = 2.1f;
    
    private bool open = false;
    private bool animating = false;

    public event Action OnOpen = delegate { };
    public event Action OnClose = delegate { };

    //private void Awake()
    //{
    //    var clickToMoveController = FindObjectOfType<ClickToMove>();
    //    if (clickToMoveController != null)
    //        clickToMoveController.OnClickToActivate += HandleClickToActivate;
    //}

    public void HandleClickToActivate()
    {
        if (!animating) {
            if (!open)
            {
                Activate();
                OnOpen();
            }
            else
            {
                Deactivate();
                OnClose();
            }
        }
        
    }

    public void Activate()
    {
        if(animationController != null)
        {
            open = true;
            //play the animation for open
            animationController.Play(openAnimStateName);
            StartCoroutine(StartAnimationBool(openAnimTime));
        }
        
    }

    public void Deactivate()
    {
        open = false;
        animationController.Play(closeAnimStateName);
        StartCoroutine(StartAnimationBool(closeAnimTime));
    }

    IEnumerator StartAnimationBool(float time) {
        animating = true;
        yield return new WaitForSecondsRealtime(time);
        animating = false;
        yield break;
    }

}
