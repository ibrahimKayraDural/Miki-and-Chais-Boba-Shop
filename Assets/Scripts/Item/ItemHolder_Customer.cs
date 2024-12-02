using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_Customer : ItemHolder_Base
    {
        [SerializeField] ItemData _WantedItem;
        [SerializeField] int _ItemPrice = 10;

        DayManager _DayManager
        {
            get
            {
                if (AUTO_DayManager == null) AUTO_DayManager = DayManager.Instance;
                return AUTO_DayManager;
            }
        }
        DayManager AUTO_DayManager = null;

        private void Awake()
        {
            _HeldItemRenderer.sprite = _WantedItem.UISprite;
        }

        public override bool TryPickItem(out ItemData item)
        {
            item = null;
            return false;
        }

        public override bool TryPutItem(ItemData item)
        {
            if (_heldItem != null) return false;
            if (item != _WantedItem) return false;

            _DayManager.AddMoney(_ItemPrice);

            return true;
        }

        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem != _WantedItem) return false;

            other.TryPickItem(out ItemData item);
            other.TryPutItem(null);

            TryPutItem(item);

            return true;
        }
    } 
}
