using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] List<Animator> AllMeshes;

    Animator _CurrentMesh;
    bool _isInitialized = false;
    
    public void Initialize(int playerNumber)
    {
        if (_isInitialized) return;
        if (AllMeshes.Count <= 0) return;

        _CurrentMesh = AllMeshes[playerNumber % AllMeshes.Count];

        foreach (var m in AllMeshes) m.gameObject.SetActive(m == _CurrentMesh);

        _isInitialized = true;
    }
}
