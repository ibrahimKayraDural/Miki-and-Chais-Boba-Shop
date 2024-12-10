using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_Trash : ItemHolder_Base
    {
        public override bool TryPickItem(out ItemData item, out BobaCup cup)
        {
            item = null;
            cup = null;
            return false;
        }
        public override bool TryPutItem(ItemData item, BobaCup cup) => true;
    }
}
