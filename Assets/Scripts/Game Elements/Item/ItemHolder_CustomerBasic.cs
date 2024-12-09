using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_CustomerBasic : ItemHolder_Base
    {
        [SerializeField] List<ItemData> _PossibleItems;
        [SerializeField] FloatRange CooldownRange = new FloatRange(5, 10);

        DayManager _DayManager
        {
            get
            {
                if (AUTO_DayManager == null) AUTO_DayManager = DayManager.Instance;
                return AUTO_DayManager;
            }
        }
        DayManager AUTO_DayManager = null;

        ItemData _wantedItem;
        float _customerOrderTime = -1;

        override internal void Awake()
        {
            RequestNewItem();
        }

        void RequestNewItem()
        {
            if (_PossibleItems.Count <= 0) return;
            _wantedItem = _PossibleItems[Random.Range(0, _PossibleItems.Count)];
            SetSpriteByData(_wantedItem, null);
            _customerOrderTime = Time.time;
        }

        public override bool TryPickItem(out ItemData item, out GameObject instantiatedSpritePrefab)
        {
            item = null;
            instantiatedSpritePrefab = null;
            return false;
        }

        public override bool TryPutItem(ItemData item, GameObject instantiatedSpritePrefab)
        {
            if (_heldItem != null) return false;
            if (item == null) return false;
            if (item != _wantedItem) return false;

            OnSoldItem();

            return true;
        }

        void OnSoldItem()
        {
            int totalMoney = _wantedItem.Price;
            int tip = _wantedItem.MaxTip;
            float timeSpent = Time.time - _customerOrderTime;
            float noTipDur = Mathf.Max(_wantedItem.NoTipAfterSeconds, 0.1f);
            float ratio = Mathf.Clamp(1 - (timeSpent / noTipDur), 0, 1);
            tip = Mathf.CeilToInt(ratio * tip);
            _DayManager.AddMoney(totalMoney + tip);

            _wantedItem = null;
            SetSpriteByData(null, null);
            _customerOrderTime = -1;

            RunCooldown();
        }

        void RunCooldown()
        {
            StopCoroutine(nameof(Cooldown));
            StartCoroutine(nameof(Cooldown));
        }
        IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(CooldownRange.Random());
            RequestNewItem();
        }

        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem != _wantedItem) return false;

            other.TryPickItem(out ItemData item, out _);
            other.TryPutItem(null, null);

            TryPutItem(item, null);

            return true;
        }
    }
}
