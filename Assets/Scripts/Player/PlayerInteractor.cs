using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] float _RaycastLenght = 1;
    [SerializeField] LayerMask _ReceiverLayer = 1 << 7;
    [SerializeField] ItemHolder_Player _PlayerItemHolder;

    Vector2 _direction => _pmc != null ? _pmc.PersistentDirection : Vector2.right;
    PlayerMovementController _pmc;
    ItemData _heldItem => _PlayerItemHolder == null ? null : _PlayerItemHolder.HeldItem;

    void Awake()
    {
        _pmc = GetComponent<PlayerMovementController>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Interact")) Interact();
    }

    void Interact()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, _RaycastLenght, _ReceiverLayer);
        ItemHolder_Base ih = hit.collider?.GetComponent<ItemHolder_Base>();
        if (ih == null) return;

        _PlayerItemHolder.ReplaceItems(ih);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + _direction * _RaycastLenght);
    }
}
