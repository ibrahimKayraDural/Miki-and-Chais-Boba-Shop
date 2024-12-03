using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    public Vector2 PersistentDirection => _persistentDirection;

    [SerializeField] float _Speed = 1;

    Rigidbody2D _rb;
    Vector2 _direction = Vector2.zero;
    Vector2 _persistentDirection = Vector2.right;
    bool _isLookingRight = true;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (_direction != Vector2.zero) _persistentDirection = _direction;
    }
    void FixedUpdate()
    {
        Vector2 targetPos = transform.position;
        targetPos += _direction * _Speed * Time.fixedDeltaTime;

        bool? isHeadingRight = null;
        float xDiff = transform.position.x - targetPos.x;
        if (Mathf.Abs(xDiff) >= 0.0001f) isHeadingRight = xDiff < 0;

        _rb.MovePosition(targetPos);

        if (isHeadingRight.HasValue && isHeadingRight.Value != _isLookingRight) Flip();

    }

    void Flip()
    {
        _isLookingRight = !_isLookingRight;
        transform.localScale = new Vector3(_isLookingRight ? 1 : -1, 1, 1);
    }
}