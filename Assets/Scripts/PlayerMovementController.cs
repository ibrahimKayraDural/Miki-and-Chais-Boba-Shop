using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float _Speed = 1;

    Rigidbody2D _rb;
    bool _isLookingRight = true;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector2 targetPos = transform.position;
        targetPos += inputVector * _Speed * Time.fixedDeltaTime;

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
