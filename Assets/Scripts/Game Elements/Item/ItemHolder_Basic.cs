using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_Basic : ItemHolder_Base
    {
        [SerializeField] ItemData _StarterItem;

        override internal void Awake()
        {
            base.Awake();
            if (_StarterItem != null) TryPutItem(_StarterItem, null);
        }
    }
}
