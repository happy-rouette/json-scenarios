using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public static Action<Interactable> OnClicked;
    public static Action<Interactable> OnEnter;
    public static Action<Interactable> OnExit;
    public bool IsHomeAppliance;

    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private Sprite[] _sprites;
    private int _stateIndex = 0;
    private Vector2 _mouseOffsetForDrag = Vector2.zero;
    private bool IsMobile;
    private Vector2 _defaultPos;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = transform;
    }

    public void Init(ObjectData data) {
        gameObject.name = data.nom;
        IsHomeAppliance = data.type.Equals("électroménager");
        IsMobile = data.mobile;
        transform.position = new Vector2(data.x, data.y).ToWorldPos();
        _defaultPos = transform.position;
        _sprites = Resources.LoadAll<Sprite>("ObjectSprites/" + data.image.src);
        _spriteRenderer.sprite = _sprites[0];
        GetComponent<BoxCollider2D>().size = _spriteRenderer.bounds.size;
    }

    private void NextState() 
    {
        if (++_stateIndex >= _sprites.Length)
            _stateIndex = 0;
        SetStateIndex(_stateIndex);
    }

    private void SetStateIndex(int stateIndex) 
    {
        _spriteRenderer.sprite = _sprites[stateIndex];
    }

    private void OnMouseEnter() => OnEnter?.Invoke(this);
    private void OnMouseExit() => OnExit?.Invoke(this);
    
    private void OnMouseUpAsButton() 
    {
        OnClicked?.Invoke(this);

        if (IsHomeAppliance)
            SetStateIndex(1);
    }

    public void OnBeginDrag() 
    {
        if (!IsMobile) return;
        _mouseOffsetForDrag = _transform.position - Input.mousePosition.ToWorldPos();
        _spriteRenderer.sortingOrder = 100;
    }

    public void OnDrag() 
    {
        if (!IsMobile) return;
        _transform.position = ((Vector2) Input.mousePosition.ToWorldPos()) + _mouseOffsetForDrag;
    }

    public void OnEndDrag() 
    {
        transform.position = _defaultPos;
        _spriteRenderer.sortingOrder = 0;
    }
}
