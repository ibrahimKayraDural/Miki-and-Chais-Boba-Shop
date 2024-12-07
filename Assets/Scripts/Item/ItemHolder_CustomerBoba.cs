using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemHolder
{
    public class ItemHolder_CustomerBoba : ItemHolder_Base
    {
        [SerializeField] FloatRange CooldownRange = new FloatRange(5, 10);
        [SerializeField] string _SpriteLayerName = "Foreground";

        ItemData _milk => _BobaDatabase.Milk;
        ItemData _tea => _BobaDatabase.Tea;
        ItemData _boba => _BobaDatabase.Boba;
        ItemData _cup => _BobaDatabase.Cup;
        List<ItemData> _acceptedAromas => _BobaDatabase.Aromas;
        BobaDatabase _BobaDatabase => GLOBALVALUES.BobaDatabaseRef;
        DayManager _DayManager
        {
            get
            {
                if (AUTO_DayManager == null) AUTO_DayManager = DayManager.Instance;
                return AUTO_DayManager;
            }
        }
        DayManager AUTO_DayManager = null;

        BobaCup _currentOrder = null;
        float _customerOrderTime = -1;

        private void Awake()
        {
            RequestNewItem();
        }

        void RequestNewItem()
        {
            _currentOrder = BobaCup.GetRandomCup();
            if (ManageCupSpawn(_currentOrder)) _heldItem = _cup;

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
            if (_heldItem != _cup) return false;
            if (item != _cup) return false;

            if (instantiatedSpritePrefab == null) return false;
            if (instantiatedSpritePrefab.TryGetComponent(out BobaCupController otherBCC) == false) return false;

            if (_instantiatedSpritePrefab == null) return false;
            if (_instantiatedSpritePrefab.TryGetComponent(out BobaCupController selfBCC) == false) return false;

            if (selfBCC.CupData == null) return false;
            if (otherBCC.CupData == null) return false;
            if (selfBCC.CupData.Compare(otherBCC.CupData) == false) return false;

            OnSoldItem();

            return true;
        }

        void OnSoldItem()
        {
            int totalMoney = _cup.Price;

            int tip = _cup.MaxTip;
            float timeSpent = Time.time - _customerOrderTime;
            float noTipDur = Mathf.Max(_cup.NoTipAfterSeconds, 0.1f);
            float ratio = Mathf.Clamp(1 - (timeSpent / noTipDur), 0, 1);
            tip = Mathf.CeilToInt(ratio * tip);
            _DayManager.AddMoney(totalMoney + tip);

            _heldItem = null;
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
            if (TryPutItem(other.HeldItem, other.InstantiatedSpritePrefab) == false) return false;

            other.TryPickItem(out _, out _);

            return true;
        }

        bool ManageCupSpawn(BobaCup cup)
        {
            foreach (var child in _SpriteParent.Cast<Transform>()) Destroy(child.gameObject);
            _instantiatedSpritePrefab = null;

            if (_cup != null && _cup.SpritePrefab != null)
            {
                var go = Instantiate(_cup.SpritePrefab, _SpriteParent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;

                if (go.TryGetComponent(out BobaCupController bcc))
                {
                    bcc.Initialize(cup, _SpriteLayerName);
                    _instantiatedSpritePrefab = go;
                }
                else
                {
                    Destroy(go);
                    goto ThrowError;
                }
            }
            else goto ThrowError;

            return true;

            ThrowError: Debug.LogError("_Product is not properly set. It must have a SpritePrefab with a BobaCupController Attached.");
            return false;
        }
    }
}
