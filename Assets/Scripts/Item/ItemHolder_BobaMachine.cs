using AYellowpaper.SerializedCollections;
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
        [SerializeField] ItemData Milk;
        [SerializeField] ItemData _Tea;
        [SerializeField] ItemData _Boba;
        [SerializeField] ItemData _Cup;
        [SerializeField] Slider _Slider;
        [SerializeField] Animator _WarningAnimator;
        [SerializeField] TextMeshProUGUI _WarningText;
        [SerializeField] List<ItemData> _AcceptedAromas;

        [SerializeField] UnityEvent OnMilkAdded;
        [SerializeField] UnityEvent OnTeaAdded;
        [SerializeField] UnityEvent OnBobaAdded;
        [SerializeField] UnityEvent OnAromaAdded;
        [SerializeField] UnityEvent OnCupAdded;
        [SerializeField] UnityEvent OnReset;

        bool _inProcess;
        BobaCup _currentCup = new BobaCup();
        bool _cupAdded;

        private void Awake()
        {
            _Slider.maxValue = _ProcessTime;
            _Slider.value = 0;
        }

        public override bool TryPutItem(ItemData item, GameObject instantiatedSpritePrefab)
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

            if (item == Milk)
            {
                if (_currentCup.HasMilk) return false;
                _currentCup.HasMilk = true;
                OnMilkAdded?.Invoke();
                TryStartProcess();
            }
            else if (item == _Tea)
            {
                if (_currentCup.HasTea) return false;
                _currentCup.HasTea = true;
                OnTeaAdded?.Invoke();
                TryStartProcess();
            }
            else if (item == _Boba)
            {
                if (_currentCup.HasBoba) return false;
                _currentCup.HasBoba = true;
                OnBobaAdded?.Invoke();
            }
            else if (item == _Cup)
            {
                if ((_currentCup.HasMilk || _currentCup.HasTea) == false)
                {
                    RunWarningText("Put tea or milk first...");
                    return false;
                }
                _cupAdded = true;
                OnCupAdded?.Invoke();
                TryStartProcess();
            }
            else
            {
                if (_currentCup.Aroma != null) return false;
                if (_AcceptedAromas.Contains(item) == false) return false;

                _currentCup.Aroma = item;
                OnAromaAdded?.Invoke();
            }

            return true;
        }

        private void RunWarningText(string text)
        {
            if (_WarningAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") || text != _WarningText.text)
            {
                _WarningText.text = text;
                _WarningAnimator.Play("idle");
                _WarningAnimator.Play("working");
            }
        }

        public override bool TryPickItem(out ItemData item, out GameObject instantiatedSpritePrefab)
        {
            item = null;
            instantiatedSpritePrefab = null;

            if (_inProcess) return false;
            if (HeldItem == null) return false;

            item = _heldItem;
            instantiatedSpritePrefab = _instantiatedSpritePrefab;

            _heldItem = null;
            SetSpriteByData(null, null);
            TryStartProcess();

            return true;
        }
        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem == null)
            {
                TryPickItem(out ItemData item, out GameObject isp);
                other.TryPutItem(item, isp);
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

        void TryStartProcess()
        {
            if (_inProcess) return;
            if ((_currentCup.HasMilk || _currentCup.HasTea) == false) return;
            if (_cupAdded == false) return;
            if (HeldItem != null) return;

            StopCoroutine(nameof(Process));
            StartCoroutine(nameof(Process));
        }
        IEnumerator Process()
        {
            float currentProgress = 0;
            _Slider.maxValue = _ProcessTime;
            _Slider.value = 0;

            _inProcess = true;
            while (true)
            {
                yield return new WaitForSeconds(_ProgressInterval);
                currentProgress += _ProgressInterval;
                _Slider.value = currentProgress;
                if (currentProgress >= _ProcessTime) break;
            }

            OnProcessDone();
        }
        void OnProcessDone()
        {
            if (ManageCupSpawn())
            {
                _heldItem = _Cup;
                OnItemHeld(_heldItem);
            }

            _currentCup = new BobaCup();
            _cupAdded = false;
            OnReset?.Invoke();
            _Slider.value = 0;
            _inProcess = false;
        }

        bool ManageCupSpawn()
        {
            foreach (var child in _SpriteParent.Cast<Transform>()) Destroy(child.gameObject);
            _instantiatedSpritePrefab = null;

            if (_Cup != null && _Cup.SpritePrefab != null)
            {
                var go = Instantiate(_Cup.SpritePrefab, _SpriteParent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;

                if (go.TryGetComponent(out BobaCupController bcc))
                {
                    bcc.Initialize(_currentCup);
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
