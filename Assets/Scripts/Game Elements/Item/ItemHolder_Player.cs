using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_Player : ItemHolder_Base
    {
        public override bool TryPutItem(ItemData item, BobaCup cup)
        {
            if (item != null) _SpriteParent.transform.localPosition = item.HoldingOffset;

            return base.TryPutItem(item, cup);
        }

        public override bool TryPickItem(out ItemData item, out BobaCup cup)
        {
            _SpriteParent.transform.localPosition = Vector3.zero;
            return base.TryPickItem(out item, out cup);
        }
    }
}
