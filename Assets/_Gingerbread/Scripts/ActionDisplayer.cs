using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionDisplayer : MonoBehaviour
{
    private TextMeshProUGUI _actionText;
    private Interactable _firstInteractable;

    private void Start() 
    {
        _actionText = GetComponent<TextMeshProUGUI>();
        Interactable.OnEnter += HoverNewInteractable;
        Interactable.OnExit += ExitNewInteractable;
        Interactable.OnClicked += ClickNewInteractable;
    }

    private void HoverNewInteractable(Interactable interactable)
    {
        if (interactable == _firstInteractable) return;

        string actionString = "Utiliser ";
        actionString += (_firstInteractable == null ? interactable.name : _firstInteractable.name) + " avec";
        actionString += _firstInteractable == null ? "..." : " " + interactable.name;
        _actionText.text = actionString;
    }

    private void ExitNewInteractable(Interactable interactable)
    {
        _actionText.text = _firstInteractable == null ? "" : "Utiliser " + _firstInteractable.name + " avec...";
    }

    private void ClickNewInteractable(Interactable interactable)
    {
        _firstInteractable = _firstInteractable == null ? interactable : null;
    }
}
