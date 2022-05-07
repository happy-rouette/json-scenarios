using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioButton : MonoBehaviour
{
    public static Action<string> OnScenarioChoosed;

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;

    public void Init(string scenarioName) {
        _text.text = scenarioName;
        _button.onClick.AddListener(() => OnScenarioChoosed?.Invoke(scenarioName));
    }
}
