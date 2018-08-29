using System;
using System.Collections;
using UnityEngine;

public class OpenInteraction : MonoBehaviour, ICanClickToActivate
{

    [SerializeField] private Animator animationController;
    [SerializeField] private string openStateName = "trigger";

    private bool animated = false;

    public event Action OnOpen = delegate {};
    public event Action OnClose = delegate {};


    public void HandleClickToActivate()
    {
        if (!animated)
        {     

            if (animationController != null)
            {
                //play the animation for open
                animationController.Play(openStateName);
                OnOpen();
                animated = true;
            }
            
        }

    }

    

}
