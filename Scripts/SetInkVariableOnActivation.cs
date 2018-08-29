using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInkVariableOnActivation : MonoBehaviour, ICanClickToActivate {

    [SerializeField] private VariableType variableType;
    [SerializeField]
    private enum VariableType
    {
        Bool,
        String,
        Float,
        Int
    }
    [SerializeField] private string inkVariableName;
    [SerializeField] private string newValue;

    public event Action OnOpen = delegate { };
    public event Action OnClose = delegate { };

    private void Start()
    {
        // restore variable status and apply on objects
        var ink = FindObjectOfType<InkScript>().story;
        if (ink != null)
        {
            var variable = System.Convert.ToBoolean(ink.variablesState[inkVariableName]);
            if (variable)
            {
                GetComponent<OpenInteraction>().HandleClickToActivate();
                GetComponent<IsParentInteraction>().SetColliderState(false);
            }
        }
    }

    public void HandleClickToActivate()
    {
        var ink = FindObjectOfType<InkScript>();

        if (variableType == VariableType.Bool)
            ink.SetVariable<bool>(inkVariableName, newValue);
        if (variableType == VariableType.String)
            ink.SetVariable<string>(inkVariableName, newValue);
        if (variableType == VariableType.Float)
            ink.SetVariable<float>(inkVariableName, newValue);
        if (variableType == VariableType.Int)
            ink.SetVariable<int>(inkVariableName, newValue);
    }
}
