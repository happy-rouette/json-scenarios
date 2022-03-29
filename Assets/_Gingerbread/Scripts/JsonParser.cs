using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonParser : MonoBehaviour
{
    public static Action<ObjectListData> OnDataDeserialized;
    [HideInInspector] public ObjectListData Data;

    private string _path, _imgPath, _jsonString;

    private void Start() 
    {
        _path = Application.streamingAssetsPath + "/objets.json";
        _imgPath = Application.streamingAssetsPath + "/Sprites/";
        _jsonString = File.ReadAllText(_path);
        Data = JsonUtility.FromJson<ObjectListData>(_jsonString);
        OnDataDeserialized?.Invoke(Data);
    }
}

[System.Serializable]
public class ObjectListData {
    public List<ObjectData> objets = new List<ObjectData>();
}

[System.Serializable]
public class ObjectData {
    public string nom, type;
    public string[] états;
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
