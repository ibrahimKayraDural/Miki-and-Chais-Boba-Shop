using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    const string IDLE = "idle";
    const string IDLEHOLD = "idle_hold";
    const string WALK = "walk";
    const string WALKHOLD = "walk_hold";

    [SerializeField] List<Animator> AllAnimators;

    Animator _CurrentAnimator;
    bool _isInitialized = false;

    float _velocity = 0;
    bool _isHolding = false;

    public void Initialize(int playerNumber)
    {
        if (_isInitialized) return;
        if (AllAnimators.Count <= 0) return;

        _CurrentAnimator = AllAnimators[playerNumber % AllAnimators.Count];

        foreach (var m in AllAnimators) m.gameObject.SetActive(m == _CurrentAnimator);

        RefreshAnims();

        _isInitialized = true;
    }

    public void SetVelocity(float setTo)
    {
        _velocity = setTo;
        RefreshAnims();
    }
    public void SetIsHolding(bool setTo)
    {
        _isHolding = setTo;
        RefreshAnims();
    }

    void RefreshAnims()
    {
        string anim = null;

        if (_velocity > .1f) anim = _isHolding ? WALKHOLD : WALK;
        else anim = _isHolding ? IDLEHOLD : IDLE;

        _CurrentAnimator.Play(anim, 0);
    }
}
