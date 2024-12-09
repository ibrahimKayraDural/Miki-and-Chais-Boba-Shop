using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_Source : ItemHolder_Base
    {
        [SerializeField] ItemData _Item = null;

        override internal void Awake()
        {
            if (_Item == null)
            {
                Debug.LogError("No item was assigned to " + gameObject.name);
                gameObject.SetActive(false);
            }
        }

        public override bool TryPickItem(out ItemData item, out GameObject instantiatedSpritePrefab)
        {
            item = _Item;
            instantiatedSpritePrefab = null;
            return true;
        }
        public override bool TryPutItem(ItemData item, GameObject instantiatedSpritePrefab) => false;
        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem != null) return false;

            TryPickItem(out ItemData item1, out _);
            other.TryPickItem(out ItemData item2, out _);

            other.TryPutItem(item1, null);
            TryPutItem(item2, null);
            return true;
        }
    }
}
