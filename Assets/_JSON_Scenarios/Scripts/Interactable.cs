using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IComparable<Interactable>
{
    public static Action<Message> OnMessage;
    public string Key { get => objectID + "/" + _stateStrings[_stateIndex]; }
    public string KeyMagnified 
    { 
        get 
        {
            string stateToString = _stateStrings[_stateIndex].SplitCamelCaseToLower();
            return objectID + " (" + stateToString + ")";
        }
    }

    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private Sprite[] _stateSprites;
    private string[] _stateStrings;
    private int _stateIndex = 0;
    private string objectID;

    // Drag & Drop
    private Vector2 _mouseOffsetForDrag = Vector2.zero;
    private bool _isMobile;
    private Vector2 _defaultPos;
    private bool _isBeingDragged = false;

    // Drag interaction with other Interactables
    private static Interactable _grabbedInteractable;
    private float _distanceWithGrabbed 
    { get => Vector3.Distance(_transform.position, _grabbedInteractable.transform.position); }
    private List<Interactable> _interactablesInRange = new List<Interactable>();

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = transform;
    }

    public void Init(KeyValuePair<string, ObjectData> data) {
        gameObject.name = data.Value.nom;
        objectID = data.Key;
        _isMobile = data.Value.mobile;
        transform.position = new Vector2(data.Value.x, data.Value.y).ToWorldPos();
        _defaultPos = transform.position;
        _stateSprites = Resources.LoadAll<Sprite>(data.Value.image_src);
        _spriteRenderer.sprite = _stateSprites[0];
        _stateStrings = data.Value.etats;
        GetComponent<BoxCollider2D>().size = _spriteRenderer.bounds.size;
    }

    private void NextState() 
    {
        if (++_stateIndex >= _stateSprites.Length)
            _stateIndex = 0;
        SetStateIndex(_stateIndex);
    }

    public void SetState(string state)
    {
        _stateIndex = Array.IndexOf(_stateStrings, state);
        SetStateIndex(_stateIndex);
    }

    private void SetStateIndex(int stateIndex) 
    {
        _spriteRenderer.sprite = _stateSprites[stateIndex];
    }
    
    private void OnMouseUpAsButton() 
    {
        if (_isBeingDragged) return;
        Message feedback = RecipeManager.Instance.Interact(Key, "self");
        if (feedback != null)
            OnMessage?.Invoke(feedback);
    }

    public void OnBeginDrag() 
    {
        if (!_isMobile) return;
        _isBeingDragged = true;
        _mouseOffsetForDrag = _transform.position - Input.mousePosition.ToWorldPos();
        _spriteRenderer.sortingOrder = 100;
        _grabbedInteractable = this;
    }

    public void OnDrag() 
    {
        if (!_isMobile) return;
        _transform.position = ((Vector2) Input.mousePosition.ToWorldPos()) + _mouseOffsetForDrag;
    }

    public void OnEndDrag() 
    {
        if (!_isMobile) return;
        _isBeingDragged = false;
        Message feedback = InteractWithDestination();
        OnMessage?.Invoke(feedback);

        transform.position = _defaultPos;
        _spriteRenderer.sortingOrder = 0;
        _grabbedInteractable = null;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Interactable>(out Interactable interactable))
            _interactablesInRange.Add(interactable);
    }

    private void OnTriggerExit2D(Collider2D other) {
        Interactable interactable = other.GetComponent<Interactable>();
        if (_interactablesInRange.Contains(interactable))
            _interactablesInRange.Remove(interactable);
    }

    private Message InteractWithDestination() {
        Message feedback = new Message("none", "");
        // Find the nearest interactable to interact with
        if (_interactablesInRange.Count > 0) {
            _interactablesInRange.Sort();
            feedback = new Message("error", 
                "Impossible d'associer " + KeyMagnified + " avec " + _interactablesInRange[0].KeyMagnified
            );
            foreach (Interactable interactable in _interactablesInRange)
            {
                Message newFeedback = RecipeManager.Instance.Interact(Key, interactable.Key);
                if (newFeedback != null) {
                    feedback = newFeedback;
                    break;
                }
            }
        }
        return feedback;
    }

    public int CompareTo(Interactable other)
    {
        return _distanceWithGrabbed.CompareTo(other._distanceWithGrabbed);
    }
}
