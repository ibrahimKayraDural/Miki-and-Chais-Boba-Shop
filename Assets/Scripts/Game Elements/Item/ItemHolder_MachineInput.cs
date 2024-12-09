using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_MachineInput : ItemHolder_Base
    {
        public bool IsReady => _isReady;
        public MachineController Owner = null;

        [SerializeField] ItemData _RequiredItem;
        [SerializeField] Sprite _ReadySprite;

        bool _isReady;

        override internal void Awake()
        {
            ResetInput();
        }
        public void ResetInput()
        {
            _isReady = false;
            SetSpriteByData(_RequiredItem, null);
        }

        public override bool TryPutItem(ItemData item, GameObject instantiatedSpritePrefab)
        {
            if (_isReady) return false;
            if (item != _RequiredItem) return false;

            _isReady = true;
            SetSprite(_ReadySprite);
            Owner.CheckStatus();

            return true;
        }
        public override bool TryPickItem(out ItemData item, out GameObject instantiatedSpritePrefab)
        {
            item = null;
            instantiatedSpritePrefab = null;
            return false;
        }
        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (TryPutItem(other.HeldItem, null))
            {
                other.TryPickItem(out _, out _);
                return true;
            }

            return false;
        }
    }
}
