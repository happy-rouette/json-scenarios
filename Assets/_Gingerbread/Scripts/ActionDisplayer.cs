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
        Interactable.OnMessage += (msg) => _actionText.text = msg;
    }
}
