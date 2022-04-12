using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;


public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    private Recipe _recipe;

    [HideInInspector] public string errorFeedback
    {
        get => _recipe.erreurFeedbacks[UnityEngine.Random.Range(0, _recipe.erreurFeedbacks.Count)];
    }
    

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else 
        {
            Debug.LogError("Many instances of RecipeManager, detroying on " + name, gameObject);
            DestroyImmediate(this);
        }
    }

    private void Start()
    {
        TextAsset matriceJson = Resources.Load<TextAsset>("matrice");
        _recipe = JsonConvert.DeserializeObject<Recipe>(matriceJson.text);
    }

    public string Interact(Interactable from, Interactable to)
    {
        if (_recipe.étapes.TryGetValue(from.Key, out Dictionary<string, RecipeResult> tos))
        {
            if (tos.TryGetValue(to.Key, out RecipeResult result))
            {
                // TODO Post conditions
                return result.message;
            }
        }
        return "";
    }
}

[System.Serializable]
public class Recipe
{
    public Dictionary<string, Dictionary<string, RecipeResult>> étapes;
    public List<string> erreurFeedbacks;
}

[System.Serializable]
public class RecipeResult 
{
    public string[] postconditions;
    public string message;
}