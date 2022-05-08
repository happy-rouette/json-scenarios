using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class DataParser : MonoBehaviour
{
    public static Action<Panoply, Sprite> OnPanoplyDeserialized;
    public static Action<Recipe> OnRecipeDeserialized;

    private void Awake() => ScenarioButton.OnScenarioChoosed += LoadScenarioData;

    private void LoadScenarioData(string scenario) 
    {
        string basePath = "Scenarios/" + scenario + "/";
        string bgPath = basePath + "Sprites/Environement";
        string objectsPath = basePath + "objets";
        string recipePath = basePath + "matrice";

        Sprite bgSprite = Resources.Load<Sprite>(bgPath);

        try {
            TextAsset objectsJson = Resources.Load<TextAsset>(objectsPath);
            Panoply panoply = JsonConvert.DeserializeObject<Panoply>(objectsJson.text);
            ConvertCoordsForUnity(panoply);
            OnPanoplyDeserialized?.Invoke(panoply, bgSprite);
        } catch { Debug.LogError("Objects path \"" + objectsPath + "\" doesn't exist"); }

        try {
            TextAsset matriceJson = Resources.Load<TextAsset>(recipePath);
            Recipe recipe = JsonConvert.DeserializeObject<Recipe>(matriceJson.text);
            OnRecipeDeserialized?.Invoke(recipe);
        } catch { Debug.LogError("Recipe path \"" + recipePath + "\" doesn't exist"); }

    }

    private void ConvertCoordsForUnity(Panoply panoply) 
    {
        foreach (KeyValuePair<string, ObjectData> obj in panoply.objets)
            obj.Value.y = Screen.height - obj.Value.y; // Invert y axis
    }
}


//* Objects *//

[System.Serializable]
public class Panoply {
    public Dictionary<string, ObjectData> objets = new Dictionary<string, ObjectData>();
}

[System.Serializable]
public class ObjectData {
    public string nom;
    public string[] etats;
    public int x, y;
    public bool mobile;
    public string image_src;
}

//* Recipe *//

[System.Serializable]
public class Recipe
{
    public Dictionary<string, RecipeResult> étapes;
}

[System.Serializable]
public class RecipeResult 
{
    public string[] postconditions;
    public string messageType;
    public string message;
}