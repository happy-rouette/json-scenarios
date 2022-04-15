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
    [SerializeField] private RectTransform _scrollContainer;
    [SerializeField] private TextMeshProUGUI _actionTextPrefab;

    private void Start() 
    {
        Interactable.OnMessage += DisplayMessage;
        
        _actionScrollView.gameObject.SetActive(_actionsToggle.isOn);
        _actionsToggle.onValueChanged.AddListener(
            (show) => _actionScrollView.gameObject.SetActive(show)
        );

        foreach (Transform child in _scrollContainer)
            Destroy(child.gameObject);
    }

    private void DisplayMessage(string msg, MessageType type)
    {
        string text = "<sprite index=" + ((int) type) + "> " + msg;

        _directActionText.text = text;

        Instantiate(_actionTextPrefab, _scrollContainer).text = text;
    }
}
