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

        private void Awake()
        {
            ResetInput();
        }
        public void ResetInput()
        {
            _isReady = false;
            _HeldItemRenderer.sprite = _RequiredItem.UISprite;
        }

        public override bool TryPutItem(ItemData item)
        {
            if (_isReady) return false;
            if (item != _RequiredItem) return false;

            _isReady = true;
            _HeldItemRenderer.sprite = _ReadySprite;
            Owner.CheckStatus();

            return true;
        }
        public override bool TryPickItem(out ItemData item)
        {
            item = null;
            return false;
        }
        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if(TryPutItem(other.HeldItem))
            {
                other.TryPickItem(out _);
                return true;
            }

            return false;
        }
    } 
}
