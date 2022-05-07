using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScenarioType { Cuisine, Cabane }

public class ScenarioChooser : MonoBehaviour
{
    public static Action<ScenarioType> OnScenarioChoosed;
    public static Action OnResetScenario;

    public void ChooseScenario(int scenario) {
        if (scenario >= Enum.GetNames(typeof(ScenarioType)).Length) {
            Debug.LogError("Chosen scenario out of range");
            return;
        }
        OnScenarioChoosed?.Invoke((ScenarioType) scenario);
        gameObject.SetActive(false);
    }

    public void ResetScenario() {
        gameObject.SetActive(true);
        OnResetScenario?.Invoke();
    }
}
