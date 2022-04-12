using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Interactable _interactablePrefab;
    [SerializeField] private SpriteRenderer _backgroundRenderer;

    private void Awake() => ObjectParser.OnDataDeserialized += SpawnObjects;

    private void Start() 
    {
        Sprite bgSprite = Resources.Load<Sprite>("ObjectSprites/scène");
        _backgroundRenderer.sprite = bgSprite;
    }

    private void SpawnObjects(ObjectListData data) 
    {
        foreach (ObjectData obj in data.objets)
            Instantiate(_interactablePrefab, transform).Init(obj);
    }
}
