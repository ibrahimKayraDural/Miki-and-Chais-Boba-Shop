using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public abstract class ItemHolder_Base : MonoBehaviour
    {
        public ItemData HeldItem => _heldItem;

        [SerializeField] internal SpriteRenderer _HeldItemRenderer;

        internal ItemData _heldItem = null;

        public virtual bool TryPutItem(ItemData item)
        {
            if (_heldItem != null) return false;
            if (item == null) return false;

            _heldItem = item;
            _HeldItemRenderer.sprite = item.UISprite;

            OnItemHeld(_heldItem);
            return true;
        }
        public virtual bool TryPickItem(out ItemData item)
        {
            item = _heldItem;

            _heldItem = null;
            _HeldItemRenderer.sprite = null;

            return true;
        }
        public virtual bool ReplaceItems(ItemHolder_Base other)
        {
            TryPickItem(out ItemData item1);
            other.TryPickItem(out ItemData item2);

            other.TryPutItem(item1);
            TryPutItem(item2);
            return true;
        }

        internal virtual void OnItemHeld(ItemData item) { }
    } 
}
