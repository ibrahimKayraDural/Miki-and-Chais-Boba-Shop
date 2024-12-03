using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_MachineOutput : ItemHolder_Base
    {
        public MachineController Owner;
        public bool IsReady => HeldItem == null;

        [SerializeField] ItemData _OutputItem;

        public void OutputFromMachine()
        {
            if (_heldItem != null) return;
            if (_OutputItem == null) return;

            _heldItem = _OutputItem;
            _HeldItemRenderer.sprite = _OutputItem.UISprite;

            OnItemHeld(_heldItem);
            return;
        }

        public override bool TryPutItem(ItemData item) => false;
        public override bool TryPickItem(out ItemData item)
        {
            item = _heldItem;

            _heldItem = null;
            _HeldItemRenderer.sprite = null;
            Owner.CheckStatus();

            return true;
        }
        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem != null) return false;

            TryPickItem(out ItemData item);
            other.TryPutItem(item);

            return true;
        }
    }
}
