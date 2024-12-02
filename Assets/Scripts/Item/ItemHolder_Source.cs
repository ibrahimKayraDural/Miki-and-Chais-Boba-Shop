using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_Source : ItemHolder_Base
    {
        [SerializeField] ItemData _Item = null;

        void Awake()
        {
            if(_Item == null)
            {
                Debug.LogError("No item was assigned to " + gameObject.name);
                gameObject.SetActive(false);
            }
        }

        public override bool TryPickItem(out ItemData item)
        {
            item = _Item;
            return true;
        }
        public override bool TryPutItem(ItemData item) => false;
        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem != null) return false;

            TryPickItem(out ItemData item1);
            other.TryPickItem(out ItemData item2);

            other.TryPutItem(item1);
            TryPutItem(item2);
            return true;
        }
    }
}
