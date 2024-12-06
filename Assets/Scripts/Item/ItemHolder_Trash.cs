using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_Trash : ItemHolder_Base
    {
        public override bool TryPickItem(out ItemData item, out GameObject instantiatedSpritePrefab)
        {
            item = null;
            instantiatedSpritePrefab = null;
            return false;
        }
        public override bool TryPutItem(ItemData item, GameObject instantiatedSpritePrefab) => true;
    }
}
