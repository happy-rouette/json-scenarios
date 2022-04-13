using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour
{
    public static ObjectsManager Instance;

    [SerializeField] private Interactable _interactablePrefab;
    [SerializeField] private SpriteRenderer _backgroundRenderer;
    private Dictionary<string, Interactable> _spawnedObjects = new Dictionary<string, Interactable>();

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Many instances of GameManager, destroying on " + name, gameObject);
            DestroyImmediate(this);
        }

        DataParser.OnPanoplyDeserialized += SpawnObjects;
    } 

    private void Start() 
    {
        Sprite bgSprite = Resources.Load<Sprite>("ObjectSprites/scène");
        _backgroundRenderer.sprite = bgSprite;
    }

    private void SpawnObjects(Panoply data) 
    {
        foreach (KeyValuePair<string, ObjectData> obj in data.objets)
        {
            Interactable interactable = Instantiate(_interactablePrefab, transform);
            interactable.Init(obj);
            _spawnedObjects.AddOrUpdate(obj.Key, interactable);
        }
    }

    public Interactable GetInteractable(string id)
    {
        _spawnedObjects.TryGetValue(id, out Interactable interactable);
        return interactable;
    }
}
