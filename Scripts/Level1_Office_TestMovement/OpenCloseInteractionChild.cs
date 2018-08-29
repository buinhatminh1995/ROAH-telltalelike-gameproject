using System;
using UnityEngine;

public class OpenCloseInteractionChild : MonoBehaviour, ICanClickToActivate
{

    [SerializeField] private OpenCloseInteractionParent parent;

    public event Action OnOpen = delegate { };
    public event Action OnClose = delegate { };

    public void HandleClickToActivate()
    {
        if (!parent.animating)
        {
            if (!parent.open)
            {
                parent.Activate();
                OnOpen();
            }
            else
            {
                parent.Deactivate();
                OnClose();
            }
        }

    }

   

    
}
