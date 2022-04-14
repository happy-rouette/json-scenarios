using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _directActionText;

    private Interactable _firstInteractable;

    private void Start() 
    {
        Interactable.OnMessage += (msg, type) => _directActionText.text = "<sprite index=" + ((int) type) + "> " + msg;
    }
}
