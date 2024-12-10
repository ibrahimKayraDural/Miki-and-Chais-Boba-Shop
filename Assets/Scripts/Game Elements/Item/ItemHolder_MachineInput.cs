using Photon.Pun;
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
            base.Awake();
            ResetInput();
        }
        public void ResetInput()
        {
            _isReady = false;
            SetSpriteByData(_RequiredItem, InstantiatedCup);
        }

        public override bool TryPutItem(ItemData item, BobaCup cup)
        {
            if (_isReady) return false;
            if (item != _RequiredItem) return false;

            _isReady = true;
            GV.SerializeSprite(_ReadySprite, out int w, out int h, out byte[] b);
            SetSpriteSerialized(w, h, b);
            Owner.CheckStatus();

            return true;
        }
        public override bool TryPickItem(out ItemData item, out BobaCup cup)
        {
            item = null;
            cup = null;
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
