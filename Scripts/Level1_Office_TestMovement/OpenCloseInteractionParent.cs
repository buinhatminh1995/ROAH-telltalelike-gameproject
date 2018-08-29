using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OpenCloseInteractionParent : MonoBehaviour {

    [SerializeField] private Animator animationController;
    [SerializeField] private string openAnimStateName = "open";
    [SerializeField] private float openAnimTime = 2.1f;
    [SerializeField] private string closeAnimStateName = "close";
    [SerializeField] private float closeAnimTime = 2.1f;

    [HideInInspector] public bool open = false;
    [HideInInspector] public bool animating = false;

    IEnumerator StartAnimationBool(float time)
    {
        animating = true;
        yield return new WaitForSecondsRealtime(time);
        animating = false;
        yield break;
    }

    public void Activate()
    {
        if (animationController != null)
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
}
