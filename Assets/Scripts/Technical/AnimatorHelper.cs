using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHelper : MonoBehaviour
{
    [SerializeField] Animator _Animator;

    string _targetName = null;

    public void SetBoolToTrue(string name) => _Animator.SetBool(name, true);
    public void SetBoolToFalse(string name) => _Animator.SetBool(name, false);

    public void SetTargetName(string name) => _targetName = name;

    public void SetBoolOfTargetName(bool setTo)
    {
        if (_targetName == null) return;
        _Animator.SetBool(_targetName, setTo);
    }
    public void SetFloatOfTargetName(float setTo)
    {
        if (_targetName == null) return;
        _Animator.SetFloat(_targetName, setTo);
    }
}
