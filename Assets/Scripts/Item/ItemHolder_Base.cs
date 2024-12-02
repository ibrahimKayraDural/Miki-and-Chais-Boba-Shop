using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
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
        public virtual ItemData TryPickItem()
        {
            ItemData itemToReturn = _heldItem;

            _heldItem = null;
            _HeldItemRenderer.sprite = null;

            return itemToReturn;
        }
        public void ReplaceItems(ItemHolder_Base other)
        {
            ItemData item1 = TryPickItem();
            ItemData item2 = other.TryPickItem();

            other.TryPutItem(item1);
            TryPutItem(item2);
        }

        internal abstract void OnItemHeld(ItemData item);
    } 
}
