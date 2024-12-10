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
            base.Awake();

            if (_Item == null)
            {
                Debug.LogError("No item was assigned to " + gameObject.name);
                gameObject.SetActive(false);
            }
        }

        public override bool TryPickItem(out ItemData item, out BobaCup cup)
        {
            item = _Item;
            cup = _instantiatedCup;
            return true;
        }
        public override bool TryPutItem(ItemData item, BobaCup cup) => false;
        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem != null) return false;

            TryPickItem(out ItemData item, out BobaCup cup);
            other.TryPickItem(out _, out _);

            other.TryPutItem(item, cup);
            return true;
        }
    }
}
