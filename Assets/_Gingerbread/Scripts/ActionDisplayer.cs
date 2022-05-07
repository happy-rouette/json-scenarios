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
        ChooseScenarioPanel.OnResetScenario += ResetLogs;
        
        _actionScrollView.gameObject.SetActive(_actionsToggle.isOn);
        _actionsToggle.onValueChanged.AddListener(
            (show) => _actionScrollView.gameObject.SetActive(show)
        );

        ResetLogs();
    }

    private void DisplayMessage(Message msg)
    {
        string text = (msg.Type != MessageType.None ? 
            "<sprite index=" + ((int) msg.Type) + "> " : ""
        ) + msg.Text;
        
        _directActionText.text = text;
        if (msg.Text.Length > 0)
            Instantiate(_actionTextPrefab, _scrollContainer).text = text;
    }

    private void ResetLogs() {
        _directActionText.text = "";
        foreach (Transform child in _scrollContainer)
            Destroy(child.gameObject);
        _actionScrollView.gameObject.SetActive(false);
    }
}
