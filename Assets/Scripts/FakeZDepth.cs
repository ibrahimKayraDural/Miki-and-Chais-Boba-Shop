using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeZDepth : MonoBehaviour
{
    [SerializeField] SpriteRenderer _SpriteRenderer;

    float _origin = 0;
    bool _isPaused;

    void Awake()
    {
        ResetOrigin();
    }

    public void ResetOrigin()
    {
        _origin = transform.position.y;
    }
    public void Pause() => _isPaused = true;
    public void Unpause() => _isPaused = false;

    void Update()
    {
        if (_isPaused) return;
        float targetY = transform.position.y - _origin;
        targetY *= 10;
        _SpriteRenderer.sortingOrder = -Mathf.RoundToInt(targetY);
    }
}
