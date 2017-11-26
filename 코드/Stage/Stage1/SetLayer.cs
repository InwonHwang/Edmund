using UnityEngine;
using System.Collections;

public class SetLayer : MonoBehaviour {

    SpriteRenderer _edmundRenderer;
    SpriteRenderer _renderer;

    BoxCollider2D _edmundCollider;
    BoxCollider2D _collider;

    void Start()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _collider = gameObject.GetComponent<BoxCollider2D>();
        _edmundRenderer = Utility.Instance.findChild("Edmund", "Base").GetComponent<SpriteRenderer>();
        _edmundCollider = GameObject.Find("Edmund").GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(_edmundCollider.bounds.min.y > _collider.bounds.max.y)
        {
            _renderer.sortingOrder = _edmundRenderer.sortingOrder + 1;
        }
        else
        {
            _renderer.sortingOrder = _edmundRenderer.sortingOrder - 1;
        }
    }

    
}
