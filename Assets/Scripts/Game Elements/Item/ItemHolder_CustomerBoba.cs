using Photon.Pun;
using Photon.Realtime;
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

        ItemData _milk => _ItemDatabase.Milk;
        ItemData _tea => _ItemDatabase.Tea;
        ItemData _boba => _ItemDatabase.Boba;
        ItemData _cup => _ItemDatabase.Cup;
        List<ItemData> _acceptedAromas => _ItemDatabase.Aromas;
        ItemDatabase _ItemDatabase => GV.ItemDatabaseRef;
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

        internal override void Awake()
        {
            base.Awake();
            SpawnRandomCup();
        }

        public override bool TryPickItem(out ItemData item, out BobaCup cup)
        {
            item = null;
            cup = null;
            return false;
        }

        public override bool TryPutItem(ItemData item, BobaCup cup)
        {
            if (_heldItem != _cup) return false;
            if (item != _cup) return false;

            if (_instantiatedCup == null) return false;
            if (cup == null) return false;

            if (cup.Compare(_instantiatedCup) == false) return false;

            OnSoldItem();

            return true;
        }

        void OnSoldItem()
        {
            int totalMoney = _cup.Price;

            int tip = 0;
            if (_customerOrderTime >= 0)
            {
                tip = _cup.MaxTip;
                float timeSpent = Time.time - _customerOrderTime;
                float noTipDur = Mathf.Max(_cup.NoTipAfterSeconds, 0.1f);
                float ratio = Mathf.Clamp(1 - (timeSpent / noTipDur), 0, 1);
                tip = Mathf.CeilToInt(ratio * tip);
            }

            _photonView.RPC(nameof(RPC_OnSoldItem), RpcTarget.All, totalMoney + tip);
        }

        [PunRPC]
        internal void RPC_OnSoldItem(int gainedMoney)
        {
            _DayManager.AddMoney(gainedMoney);

            _heldItem = null;
            SetSpriteToNull();
            _customerOrderTime = -1;

            if (PhotonNetwork.IsMasterClient)
            {
                RunCooldown();
            }
        }

        void RunCooldown()
        {
            StopCoroutine(nameof(Cooldown));
            StartCoroutine(nameof(Cooldown));
        }
        IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(CooldownRange.Random());
            SpawnRandomCup();
        }

        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (TryPutItem(other.HeldItem, other.InstantiatedCup) == false) return false;

            other.TryPickItem(out _, out _);

            return true;
        }

        void SpawnRandomCup()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                BobaCup cup = BobaCup.GetRandomCup();

                _photonView.RPC(nameof(RPC_ManageCupSpawn), RpcTarget.All, cup.Serialize());
            }
        }

        [PunRPC]
        internal void RPC_ManageCupSpawn(string serializedCup)
        {
            foreach (var child in _SpriteParent.Cast<Transform>()) Destroy(child.gameObject);

            if (_cup != null && _cup.SpritePrefab != null)
            {
                var go = GV.PhotonInstantiate(_cup.SpritePrefab, _SpriteParent);

                if (go.TryGetComponent(out BobaCupController bcc))
                {
                    BobaCup cupData = BobaCup.Deserialize(serializedCup);
                    bcc.Initialize(cupData, _SpriteLayerName);

                    _heldItem = _cup;
                    _instantiatedCup = cupData;
                    _currentOrder = cupData;
                    _customerOrderTime = Time.time;
                }
                else
                {
                    Destroy(go);
                    goto ThrowError;
                }
            }
            else goto ThrowError;

            return;

            ThrowError: Debug.LogError("_Product is not properly set. It must have a SpritePrefab with a BobaCupController Attached.");
            _customerOrderTime = -1;

            return;
        }
    }
}
