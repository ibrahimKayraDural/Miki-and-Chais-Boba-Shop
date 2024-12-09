using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemHolder;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField, Tooltip("If enabled, looks at up, right, down then left for items when faced direction has no items.")]
    bool _CheckSurroundingsForItems = true;
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

    public void Deactivate()
    {
        enabled = false;
    }

    void Interact()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, _RaycastLenght, _ReceiverLayer);
        ItemHolder_Base ih = hit.collider?.GetComponent<ItemHolder_Base>();

        if (ih == null && _CheckSurroundingsForItems)//check surroundings if couldnt find anything
        {
            float rotation = 0;
            for (int i = 0; i < 4; i++)
            {
                Vector3 dir = Quaternion.Euler(0, 0, rotation) * Vector3.up;
                hit = Physics2D.Raycast(transform.position, dir, _RaycastLenght, _ReceiverLayer);
                ih = hit.collider?.GetComponent<ItemHolder_Base>();
                if (TryReplacing()) break;
                rotation -= 90;
            }
        }
        else TryReplacing();

        bool TryReplacing()
        {
            if (ih == null || _PlayerItemHolder == null) return false;
            return ih.ReplaceItems(_PlayerItemHolder);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + _direction * _RaycastLenght);
    }
}
