using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _directActionText;
    [SerializeField] private Toggle _actionsToggle;
    [SerializeField] private ScrollRect _actionScrollView;

    private Interactable _firstInteractable;

    private void Start() 
    {
        Interactable.OnMessage += (msg, type) => _directActionText.text = "<sprite index=" + ((int) type) + "> " + msg;
        _actionScrollView.gameObject.SetActive(_actionsToggle.isOn);
        _actionsToggle.onValueChanged.AddListener(
            (show) => _actionScrollView.gameObject.SetActive(show)
        );
    }
}
