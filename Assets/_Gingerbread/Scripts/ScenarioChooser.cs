using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScenarioType { Cooking, Shelter }

public class ScenarioChooser : MonoBehaviour
{
    public static Action<ScenarioType> OnScenarioChoosed;

    public void ChooseScenario(int scenario) {
        if (scenario >= Enum.GetNames(typeof(ScenarioType)).Length) {
            Debug.LogError("Chosen scenario out of range");
            return;
        }
        OnScenarioChoosed?.Invoke((ScenarioType) scenario);
        gameObject.SetActive(false);
    }
}
