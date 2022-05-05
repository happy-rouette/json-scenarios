using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class DataParser : MonoBehaviour
{
    public static Action<Panoply> OnPanoplyDeserialized;
    public static Action<Recipe> OnRecipeDeserialized;

    private Panoply _panoply;
    private Recipe _recipe;

    private void Awake() {
        ScenarioChooser.OnScenarioChoosed += LoadScenarioData;
    }

    private void LoadScenarioData(ScenarioType scenario) 
    {
        string objectsPath = "", recipePath = "";
        switch(scenario) {
            case ScenarioType.Cooking:
                objectsPath = "cooking_objets";
                recipePath = "cooking_matrice";
                break;
            case ScenarioType.Shelter:
                objectsPath = "shelter_objets";
                recipePath = "shelter_matrice";
                break;
        }

        TextAsset objectsJson = Resources.Load<TextAsset>(objectsPath);
        _panoply = JsonConvert.DeserializeObject<Panoply>(objectsJson.text);
        ConvertCoordsForUnity();
        OnPanoplyDeserialized?.Invoke(_panoply);

        TextAsset matriceJson = Resources.Load<TextAsset>(recipePath);
        _recipe = JsonConvert.DeserializeObject<Recipe>(matriceJson.text);
        OnRecipeDeserialized?.Invoke(_recipe);
    }

    private void ConvertCoordsForUnity() 
    {
        foreach (KeyValuePair<string, ObjectData> obj in _panoply.objets) {
            obj.Value.y = Screen.height - obj.Value.y; // Invert y axis
            // Vector2Int screenCenter = new Vector2Int(Screen.width/2, Screen.height/2);
            // obj.y = screenCenter.y - screenCenter.y - obj.y;
            // obj.x = screenCenter.x - screenCenter.x - obj.x;
        }
    }
}


//* Objects *//

[System.Serializable]
public class Panoply {
    public Dictionary<string, ObjectData> objets = new Dictionary<string, ObjectData>();
}

[System.Serializable]
public class ObjectData {
    public string nom, type;
    public string[] etats;
    public int x, y;
    public bool mobile;
    public ImageData image;
}

[System.Serializable]
public class ImageData {
    public string src;
    public int dimX, dimY;
    public ClefsData clefs;
}

[System.Serializable]
public class ClefsData {
    public int x, y;
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