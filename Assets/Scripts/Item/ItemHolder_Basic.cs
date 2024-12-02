using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Item
{
    public class ItemHolder_Basic : ItemHolder_Base
    {
        [SerializeField] ItemData _StarterItem;

        void Awake()
        {
            if (_StarterItem != null) TryPutItem(_StarterItem);
        }

        internal override void OnItemHeld(ItemData item) { }
    }
}
