using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum ScenarioType { Cuisine, Cabane }

public class ChooseScenarioPanel : MonoBehaviour
{
    public static Action OnResetScenario;

    [SerializeField] ScenarioButton _scenarioButtonPrefab;
    [SerializeField] Transform _scenarioButtonsContainer;

    private void Awake() => ScenarioButton.OnScenarioChoosed += _ => gameObject.SetActive(false);

    private void Start() {
        TextAsset allScenarios = Resources.Load<TextAsset>("Scenarios/ScenariosEnumeration");
        StringReader reader = new StringReader(allScenarios.text);
        List<string> scenarios = new List<string>();
        string line;
        while ((line = reader.ReadLine()) != null)
            scenarios.Add(line);
        reader.Close();

        for (int i = 0; i < scenarios.Count; i++)
            Instantiate(_scenarioButtonPrefab, _scenarioButtonsContainer).Init(scenarios[i]);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_scenarioButtonsContainer.GetComponent<RectTransform>());
    }

    public void ResetScenario() {
        gameObject.SetActive(true);
        OnResetScenario?.Invoke();
    }
}
