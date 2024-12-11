using AYellowpaper.SerializedCollections;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ItemHolder
{
    public class ItemHolder_BobaMachine : ItemHolder_Base
    {
        [SerializeField] bool _CanAddWhenInProcess = true;
        [SerializeField] float _ProcessTime = 1;
        [SerializeField] float _ProgressInterval = .05f;
        [SerializeField] Slider _Slider;
        [SerializeField] Animator _WarningAnimator;
        [SerializeField] TextMeshProUGUI _WarningText;
        [SerializeField] SpriteRenderer _AromaSprite;

        [SerializeField] UnityEvent OnMilkAdded;
        [SerializeField] UnityEvent OnTeaAdded;
        [SerializeField] UnityEvent OnBobaAdded;
        [SerializeField] UnityEvent OnAromaAdded;
        [SerializeField] UnityEvent OnCupAdded;
        [SerializeField] UnityEvent OnReset;

        ItemData _milk => _ItemDatabase.Milk;
        ItemData _tea => _ItemDatabase.Tea;
        ItemData _boba => _ItemDatabase.Boba;
        ItemData _cup => _ItemDatabase.Cup;
        List<ItemData> _acceptedAromas => _ItemDatabase.Aromas;
        ItemDatabase _ItemDatabase => GV.ItemDatabaseRef;

        bool _inProcess;
        BobaCup _currentCup = new BobaCup();
        bool _cupAdded;

        override internal void Awake()
        {
            base.Awake();

            _Slider.maxValue = 1;
            _Slider.value = 0;
        }

        public override bool TryPutItem(ItemData item, BobaCup cup)
        {
            if (HeldItem != null)
            {
                RunWarningText("Take the finished product first...");
                return false;
            }
            if (item == null) return false;
            if (_inProcess && _CanAddWhenInProcess == false)
            {
                RunWarningText("Wait untill the process is finished...");
                return false;
            }

            return TryAddItem(item.ID);
        }

        internal bool TryAddItem(string itemID)
        {
            ItemData item = GV.ItemDatabaseRef.GetItemDataByNameOrID(itemID);

            string itemSTR = GV.UnassignedString;
            string aromaID = GV.UnassignedString;

            if (item == _milk)
            {
                if (_currentCup.HasMilk) return false;
                itemSTR = "milk";
            }
            else if (item == _tea)
            {
                if (_currentCup.HasTea) return false;
                itemSTR = "tea";
            }
            else if (item == _boba)
            {
                if (_currentCup.HasBoba) return false;
                itemSTR = "boba";
            }
            else if (item == _cup)
            {
                if ((_currentCup.HasMilk || _currentCup.HasTea) == false)
                {
                    RunWarningText("Put tea or milk first...");
                    return false;
                }
                itemSTR = "cup";
            }
            else
            {
                if (_currentCup.Aroma != null) return false;
                if (_acceptedAromas.Contains(item) == false) return false;
                itemSTR = "aroma";
                aromaID = item.ID;
            }

            _photonView.RPC(nameof(RPC_RunItem), RpcTarget.All, itemSTR, aromaID);
            return true;
        }

        [PunRPC]
        internal void RPC_RunItem(string item, string aromaID)
        {
            switch (item)
            {
                case "milk":
                    _currentCup.HasMilk = true;
                    OnMilkAdded?.Invoke();
                    RPC_TryStartProcess();
                    break;
                case "tea":
                    _currentCup.HasTea = true;
                    OnTeaAdded?.Invoke();
                    RPC_TryStartProcess();
                    break;
                case "boba":
                    _currentCup.HasBoba = true;
                    OnBobaAdded?.Invoke();
                    break;
                case "cup":
                    _cupAdded = true;
                    OnCupAdded?.Invoke();
                    RPC_TryStartProcess();
                    break;
                case "aroma":
                    ItemData itemData = GV.ItemDatabaseRef.GetItemDataByNameOrID(aromaID);
                    _currentCup.Aroma = itemData;
                    _AromaSprite.sprite = itemData.UISprite;
                    OnAromaAdded?.Invoke();
                    break;
            }
        }

        internal void RunWarningText(string text) => _photonView.RPC(nameof(RPC_RunWarningText), RpcTarget.All, text);
        [PunRPC]
        internal void RPC_RunWarningText(string text)
        {
            if (_WarningAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") || text != _WarningText.text)
            {
                _WarningText.text = text;
                _WarningAnimator.Play("idle");
                _WarningAnimator.Play("working");
            }
        }

        public override bool TryPickItem(out ItemData item, out BobaCup cup)
        {
            item = null;
            cup = null;

            if (_inProcess) return false;
            if (HeldItem == null) return false;

            item = _heldItem;
            cup = _instantiatedCup;

            _photonView.RPC(nameof(RPC_SetHeldItem), RpcTarget.All, NULLITEM);
            SetSpriteToNull();
            _photonView.RPC(nameof(RPC_TryStartProcess), RpcTarget.MasterClient);

            return true;
        }
        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem == null)
            {
                TryPickItem(out ItemData item, out BobaCup cup);
                other.TryPutItem(item, cup);
            }
            else
            {
                if (TryPutItem(other.HeldItem, null))
                {
                    other.TryPickItem(out _, out _);
                }
            }

            return false;
        }

        void SyncProgressBar(float amount) => _photonView.RPC(nameof(RPC_SyncProgressBar), RpcTarget.All, amount);

        [PunRPC]
        void RPC_SyncProgressBar(float amount)
        {
            _Slider.value = amount;
        }

        [PunRPC]
        void RPC_TryStartProcess()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (_inProcess) return;
            if ((_currentCup.HasMilk || _currentCup.HasTea) == false) return;
            if (_cupAdded == false) return;
            if (HeldItem != null) return;

            StopCoroutine(nameof(Process));
            StartCoroutine(nameof(Process));
        }
        IEnumerator Process()
        {
            if (PhotonNetwork.IsMasterClient == false) yield break;

            float currentProgress = 0;
            SyncProgressBar(0);

            _inProcess = true;
            while (true && _ProcessTime > 0)
            {
                yield return new WaitForSeconds(_ProgressInterval);
                currentProgress += _ProgressInterval;
                SyncProgressBar(currentProgress / _ProcessTime);
                if (currentProgress >= _ProcessTime) break;
            }

            _photonView.RPC(nameof(RPC_OnProcessDone), RpcTarget.All);
        }
        [PunRPC]
        void RPC_OnProcessDone()
        {
            if (ManageCupSpawn()) _heldItem = _cup;

            _currentCup = new BobaCup();
            _cupAdded = false;
            _AromaSprite.sprite = null;
            OnReset?.Invoke();
            _Slider.value = 0;
            _inProcess = false;
        }

        bool ManageCupSpawn()
        {
            foreach (var child in _SpriteParent.Cast<Transform>()) Destroy(child.gameObject);

            if (_cup != null && _cup.SpritePrefab != null)
            {
                var go = Instantiate(_cup.SpritePrefab, _SpriteParent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;

                if (go.TryGetComponent(out BobaCupController bcc))
                {
                    bcc.Initialize(_currentCup);
                    _instantiatedCup = _currentCup;
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
