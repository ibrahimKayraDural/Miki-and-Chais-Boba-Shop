using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemHolder
{
    public abstract class ItemHolder_Base : MonoBehaviour
    {
        public ItemData HeldItem => _heldItem;

        [SerializeField] internal Transform _SpriteParent;
        [SerializeField] internal string _SortLayer = "Item";
        [SerializeField] internal int _SortOrder;

        internal ItemData _heldItem = null;

        internal void SetSpriteByData(ItemData item)
        {
            if (item == null)
            {
                SetSprite(null);
                return;
            }
            else if (item.UseSpritePrefab == false || item.SpritePrefab == null)
            {
                SetSprite(item.UISprite);
                return;
            }

            foreach (var child in _SpriteParent.Cast<Transform>()) Destroy(child.gameObject);
            if (item != null)
            {
                var go = Instantiate(item.SpritePrefab, _SpriteParent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;

                if (go.TryGetComponent(out SpriteRenderer sr))
                {
                    sr.sortingLayerName = _SortLayer;
                    sr.sortingOrder = _SortOrder;
                }
            }
        }
        internal void SetSprite(Sprite sprite)
        {
            foreach (var child in _SpriteParent.Cast<Transform>()) Destroy(child.gameObject);
            if (sprite != null)
            {
                var go = new GameObject(gameObject.name + "_sprite", new System.Type[] { typeof(SpriteRenderer) });
                go.transform.parent = _SpriteParent;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = sprite;
                sr.sortingLayerName = _SortLayer;
                sr.sortingOrder = _SortOrder;
            }
        }

        public virtual bool TryPutItem(ItemData item)
        {
            if (_heldItem != null) return false;
            if (item == null) return false;

            _heldItem = item;
            SetSpriteByData(item);

            OnItemHeld(_heldItem);
            return true;
        }
        public virtual bool TryPickItem(out ItemData item)
        {
            item = _heldItem;

            _heldItem = null;
            SetSpriteByData(null);

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
