using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class ItemHolder_Player : ItemHolder_Base
    {
        internal override void OnItemHeld(ItemData item) { }

        public override bool TryPutItem(ItemData item)
        {
            _HeldItemRenderer.transform.localPosition = item.HoldingOffset;
            return base.TryPutItem(item);
        }

        public override ItemData TryPickItem()
        {
            _HeldItemRenderer.transform.localPosition = Vector3.zero;
            return base.TryPickItem();
        }
    } 
}
