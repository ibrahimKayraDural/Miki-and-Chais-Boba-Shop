using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_Player : ItemHolder_Base
    {
        public override bool TryPutItem(ItemData item)
        {
            if (item != null) _HeldItemRenderer.transform.localPosition = item.HoldingOffset;

            return base.TryPutItem(item);
        }

        public override bool TryPickItem(out ItemData item)
        {
            _HeldItemRenderer.transform.localPosition = Vector3.zero;
            return base.TryPickItem(out item);
        }
    }
}
