using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddToInventory : MonoBehaviour {
    [SerializeField] private string itemTooltip = "Item Tooltip";
    [SerializeField] private Sprite overrideItem;
    [SerializeField] private Sprite overrideIcon;
    [SerializeField] private string triggerDialogueKnot;
    private bool pickedUp;

    private void Start() { }

    private void Awake()
    {
        var openClue = GetComponent<ClickOpenClueImageBlurryBackgroundUI>();
        if (openClue != null)
            overrideItem = openClue.imageUI;

        var inspectable = GetComponent<Inspectable>();
        if (inspectable != null)
            inspectable.OnPlayerReachObserveAngle += discardparam => AddItem();

        var inspectableNoCam = GetComponent<InspectableNoCam>();
        if (inspectableNoCam != null)
            inspectableNoCam.OnPlayerReachAndClickOnObject += AddItem;

        var inspectableAnywhere = GetComponent<InspectableAnywhere>();
        if (inspectableAnywhere != null)
            inspectableAnywhere.OnClickFromAnywhere += AddItem;
       
        if (overrideIcon == null)
            overrideIcon = Resources.Load<Sprite>("Sprites/item-" + gameObject.name);
    }

    public void AddItem(BaseEventData e)
    {
        AddItem();
    }

    private void AddItem()
    {
        if (!pickedUp && enabled)
        {
            Debug.Log("added item to inventory");
            var inventoryCanvas = GameObject.Find("Inventory");
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/Item"), inventoryCanvas.transform);

            var pos = go.GetComponent<RectTransform>().localPosition;
            var newPos = new Vector3(pos.x + (inventoryCanvas.transform.childCount - 1) * 83, pos.y, pos.z);
            go.GetComponent<RectTransform>().localPosition = newPos;

            var clueClick = go.GetComponent<ClickOpenClueImageBlurryBackgroundUI>();
            if (GetComponent<ClickOpenClueImageBlurryBackgroundUI>() != null)
            {
                clueClick.imageUI = overrideItem; // set sprite to open when clicked in inventory
                FindObjectOfType<ClueImageController>().AttachOpenImageUIListener(clueClick); // attach delegate listener to controllers
                FindObjectOfType<BackButtonController>().AttachHandleClueBackButtonListener(go.GetComponent<ObjectClickTriggerBackButton>());
            }
            go.GetComponent<Tooltip>().tooltipText = itemTooltip;
            go.transform.Find("Icon").GetComponent<Image>().sprite = overrideIcon; // set icon of item shown in inventory


            if (!string.IsNullOrEmpty(triggerDialogueKnot))
            {
                var td = go.GetComponent<TriggerDialogue>();
                td.startKnot = triggerDialogueKnot;
                td.activated = true;
            }
        }

        pickedUp = true;
    }
}
