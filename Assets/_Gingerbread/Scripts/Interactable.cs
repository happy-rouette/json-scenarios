using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Sprite[] _sprites;
    private int _currentSpriteIndex = 0;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(ObjectData data) {
        gameObject.name = data.nom;
        transform.position = new Vector2(data.x, data.y).ToWorldPos();
        _sprites = Resources.LoadAll<Sprite>("ObjectSprites/" + data.image.src);
        _spriteRenderer.sprite = _sprites[0];
    }

    private void OnMouseUpAsButton() {
        if (++_currentSpriteIndex >= _sprites.Length)
            _currentSpriteIndex = 0;
        
        _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
    }
}
