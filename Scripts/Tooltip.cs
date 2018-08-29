using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] public string tooltipText;
    [SerializeField] private bool showToolTip = true, hideAfterArrival = true;
    private string currentToolTipText;
    private bool inventoryTooltip;
    private GUIStyle guiStyleFore;
    private GUIStyle guiStyleBack;
    private EventSystem eventSystem;

    //Tooltip: https://answers.unity.com/questions/44811/tooltip-when-mousing-over-a-game-object.html

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();

        var inspectable = GetComponent<Inspectable>();
        if (inspectable != null)
            inspectable.OnPlayerReachObserveAngle += RemoveSelfTooltip;

        BackButtonController backButton = FindObjectOfType<BackButtonController>();
        if (backButton != null)
            backButton.OnCamBackButtonClick += delegate { RestoreSelfTooltip(null); };

        Ground ground = FindObjectOfType<Ground>();
        if (ground != null)
            ground.OnPlayerClickOnGroundObserveAngle += RestoreSelfTooltip;

        if (string.IsNullOrEmpty(tooltipText))
            tooltipText = gameObject.name;

        // Tooltip style
        guiStyleFore = new GUIStyle();
        guiStyleFore.normal.textColor = Color.white;
        guiStyleFore.alignment = TextAnchor.UpperLeft;
        guiStyleFore.wordWrap = false;
        guiStyleBack = new GUIStyle();
        guiStyleBack.normal.textColor = Color.black;
        guiStyleBack.alignment = TextAnchor.UpperLeft;
        guiStyleBack.wordWrap = false;
    }

    void OnMouseEnter()
    {
        currentToolTipText = tooltipText;
        //Debug.Log("MouseEnter: " + gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.GetComponent<InventoryItem>())
        {
            currentToolTipText = tooltipText;
            inventoryTooltip = true;
        }
    }

    void OnMouseExit()
    {
        currentToolTipText = string.Empty;
        //Debug.Log("MouseExit: " + gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.GetComponent<InventoryItem>())
        {
            currentToolTipText = string.Empty;
            inventoryTooltip = false;
        }
    }

    void OnGUI()
    {
        if (!string.IsNullOrEmpty(currentToolTipText) && showToolTip && (!eventSystem.IsPointerOverGameObject() || inventoryTooltip))
        {
            var tooltip = new GUIContent(currentToolTipText);
            GUI.skin.font = Resources.Load<Font>("Fonts/OpenSans-SemiBold");
            float minWidth, maxWidth;
            guiStyleFore.CalcMinMaxWidth(tooltip, out minWidth, out maxWidth);
            var width = Mathf.Min(maxWidth, 500) + 20;
            var x = Input.mousePosition.x; //Event.current.mousePosition.x;
            var y = Screen.height - Input.mousePosition.y; //Event.current.mousePosition.y;
            //Debug.Log(Event.current.mousePosition.x +", "+Event.current.mousePosition.y);
            GUI.Box(new Rect(x + 16, y, width, 25), tooltip);
            //GUI.Label(new Rect(x + 26, y + 6, width, 25), tooltip, guiStyleBack);
            GUI.Label(new Rect(x + 26, y + 3, width, 25), tooltip, guiStyleFore);
        }
    }

    private void RestoreSelfTooltip(ObserveAngle observable)
    {
        showToolTip = true;
    }

    private void RemoveSelfTooltip(ObserveAngle observable)
    {
        if (hideAfterArrival)
            showToolTip = false;
    }
}
