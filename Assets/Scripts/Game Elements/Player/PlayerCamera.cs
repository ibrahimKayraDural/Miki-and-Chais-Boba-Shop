using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] PlayerController Owner;
    [SerializeField] Camera _Camera;
    [SerializeField] float _CameraSpeed = 1;

    FloatRange CameraMinMaxX
    {
        get
        {
            float middle = _cameraBoundsOffset.x;
            float offset = 0;
            if (_cameraBounds.x != 0)
                offset += _cameraBounds.x / 2;
            offset -= _cameraSize.x;

            return new FloatRange(middle - offset, middle + offset);
        }
    }
    FloatRange CameraMinMaxY
    {
        get
        {
            float middle = _cameraBoundsOffset.y;
            float offset = 0;
            if (_cameraBounds.y != 0)
                offset += _cameraBounds.y / 2;
            offset -= _cameraSize.y;

            return new FloatRange(middle - offset, middle + offset);
        }
    }

    Transform _target;
    Vector2 _cameraBounds;
    Vector2 _cameraBoundsOffset;
    Vector2 _cameraSize;
    bool _isInitialized;

    void Start()
    {
        if (DayManager.Instance != null && _Camera != null)
        {
            transform.parent = null;
            _cameraBounds = DayManager.Instance.CameraBounds;
            _cameraBoundsOffset = DayManager.Instance.CameraBoundsOffset;
            _cameraSize = Vector2.one * _Camera.orthographicSize;
            _cameraSize.x *= _Camera.aspect;
            _target = Owner.transform;

            if (_cameraBounds.x <= 0 || _cameraBounds.x / 2 <= _cameraSize.x) _cameraBounds.x = _cameraSize.x * 2;
            if (_cameraBounds.y <= 0 || _cameraBounds.y / 2 <= _cameraSize.y) _cameraBounds.y = _cameraSize.y * 2;

            _isInitialized = true;
        }
        else Debug.LogError("Player camera was not initialized. Check inspector values of " + gameObject.name);
    }

    void FixedUpdate()
    {
        if (_isInitialized == false) return;

        Vector3 targetPos = _target.position;

        //smooth movement
        targetPos = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * _CameraSpeed);

        //lock to bounds
        targetPos.x = Mathf.Clamp(targetPos.x, CameraMinMaxX.Min, CameraMinMaxX.Max);
        targetPos.y = Mathf.Clamp(targetPos.y, CameraMinMaxY.Min, CameraMinMaxY.Max);

        targetPos.z = -10;
        transform.position = targetPos;
    }

    public void Deactivate()
    {
        _Camera.gameObject.SetActive(false);
        enabled = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_cameraBoundsOffset, _cameraBounds);
    }
}
