using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class ObjectParser : MonoBehaviour
{
    public static Action<ObjectListData> OnDataDeserialized;
    [HideInInspector] public ObjectListData Data;

    private void Start() 
    {
        TextAsset objectsJson = Resources.Load<TextAsset>("objets");
        Data = JsonConvert.DeserializeObject<ObjectListData>(objectsJson.text);
        ConvertCoordsForUnity();
        OnDataDeserialized?.Invoke(Data);
    }

    private void ConvertCoordsForUnity() 
    {
        foreach (ObjectData obj in Data.objets) {
            obj.y = Screen.height - obj.y; // Invert y axis
            // Vector2Int screenCenter = new Vector2Int(Screen.width/2, Screen.height/2);
            // obj.y = screenCenter.y - screenCenter.y - obj.y;
            // obj.x = screenCenter.x - screenCenter.x - obj.x;
        }
    }
}

[System.Serializable]
public class ObjectListData {
    public List<ObjectData> objets = new List<ObjectData>();
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
