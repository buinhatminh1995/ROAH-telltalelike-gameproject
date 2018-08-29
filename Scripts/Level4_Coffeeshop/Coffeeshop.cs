using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffeeshop : MonoBehaviour {

    [SerializeField] private GameObject coffeecup;
    [SerializeField] private GameObject ben;

    private void Awake()
    {
        var ink = FindObjectOfType<InkScript>();
        if (ink != null)
            ink.GameEventFired += OnGameEventFired;
    }

    void OnGameEventFired(string gameEvent)
    {
        if (gameEvent == "bringCoffee")
        {
            coffeecup.GetComponent<AddToInventory>().enabled = true;
            coffeecup.GetComponent<Tooltip>().tooltipText = "[Pick up Coffee]";
            coffeecup.GetComponent<Inspectable>().OnPlayerReachObserveAngle += (ObserveAngle observeAngle) =>
            {
                coffeecup.SetActive(false);
            };
            ben.GetComponent<Inspectable>().OnPlayerReachObserveAngle += RemoveCoffee;
        }
    }

    private void RemoveCoffee(ObserveAngle observeAngle)
    {
        ben.GetComponent<Inspectable>().OnPlayerReachObserveAngle -= RemoveCoffee;
        var inventory = GameObject.Find("Inventory");
        Destroy(inventory.transform.GetChild(inventory.transform.childCount - 1).gameObject);
    }
}
