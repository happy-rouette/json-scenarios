using System.Security.Principal;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System;

public enum MessageType { None = -1, Good, Warning, Error, Reset }

public class Message
{
    public MessageType Type;
    public string Text;

    public Message(string type, string msg)
    {
        Text = msg;
        Type = (MessageType) Enum.Parse(typeof(MessageType), type, true);
    }
}

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    private Recipe _recipe;
    

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

    public Message Interact(string fromKey, string toKey)
    {
        string key = fromKey + "+" + toKey;
        if (_recipe.étapes.TryGetValue(key, out RecipeResult result))
        {
            foreach(string postCondition in result.postconditions)
                SetPostCondition(postCondition);
            return new Message(result.messageType, result.message);
        }
        return null;
    }

    private void SetPostCondition(string postCondition) 
    {
        string[] idAndState = postCondition.Split('/');
        Interactable interactable = ObjectsManager.Instance.GetInteractable(idAndState[0]);
        interactable?.SetState(idAndState[1]);
    }
}