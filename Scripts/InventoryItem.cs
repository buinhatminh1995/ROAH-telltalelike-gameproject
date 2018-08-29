using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour {

    public Action OnInventoryClick = delegate { };

	public void OnPointerClick(BaseEventData e)
    {
        Debug.Log("click inventory item");
        OnInventoryClick();
    }
}
