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
            SetSpriteByData(_OutputItem, InstantiatedCup);

            return;
        }

        public override bool TryPutItem(ItemData item, BobaCup cup) => false;
        public override bool TryPickItem(out ItemData item, out BobaCup cup)
        {
            item = _heldItem;
            cup = _instantiatedCup;

            _heldItem = null;
            SetSpriteToNull();
            Owner.CheckStatus();

            return true;
        }
        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem != null) return false;

            TryPickItem(out ItemData item, out BobaCup cup);
            other.TryPutItem(item, cup);

            return true;
        }
    }
}
