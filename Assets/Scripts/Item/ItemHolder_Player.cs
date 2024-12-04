using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_Player : ItemHolder_Base
    {
        public override bool TryPutItem(ItemData item)
        {
            if (item != null) _SpriteParent.transform.localPosition = item.HoldingOffset;

            return base.TryPutItem(item);
        }

        public override bool TryPickItem(out ItemData item)
        {
            _SpriteParent.transform.localPosition = Vector3.zero;
            return base.TryPickItem(out item);
        }
    }
}
