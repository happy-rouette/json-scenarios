using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

public enum MessageType { Good, Warning, Error }

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

        DataParser.OnRecipeDeserialized += (recipe) => _recipe = recipe;
    }

    public string Interact(string fromKey, string toKey)
    {
        if (_recipe.étapes.TryGetValue(fromKey, out Dictionary<string, RecipeResult> tos))
        {
            if (tos.TryGetValue(toKey, out RecipeResult result))
            {
                foreach(string postCondition in result.postconditions)
                    SetPostCondition(postCondition);
                return result.message;
            }
        }
        return "";
    }

    private void SetPostCondition(string postCondition) 
    {
        string[] idAndState = postCondition.Split('/');
        Interactable interactable = ObjectsManager.Instance.GetInteractable(idAndState[0]);
        interactable?.SetState(idAndState[1]);
    }
}