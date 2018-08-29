using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAnimatorOnInspect : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        var inspectable = GetComponent<Inspectable>();
        if (inspectable != null)
            inspectable.OnPlayerReachObserveAngle += discardparam => DisableAnimator();

        var inspectableNoCam = GetComponent<InspectableNoCam>();
        if (inspectableNoCam != null)
            inspectableNoCam.OnPlayerReachAndClickOnObject += DisableAnimator;
    }

    private void DisableAnimator()
    {
        animator.enabled = false;

        var sound = GetComponent<AudioSource>();
        if (sound != null)
            sound.Stop();
    }

}