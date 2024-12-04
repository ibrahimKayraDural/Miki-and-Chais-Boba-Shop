using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ItemHolder
{
    public class ItemHolder_BobaMachine : ItemHolder_Base
    {
        [SerializeField] ItemData Milk;
        [SerializeField] ItemData Tea;
        [SerializeField] ItemData Boba;
        [SerializeField] ItemData Cup;
        [SerializeField] ItemData _Product;
        [SerializeField] float _ProcessTime = 1;
        [SerializeField] float _ProgressInterval = .05f;
        [SerializeField] Slider _Slider;
        [SerializeField] Animator _WarningAnimator;
        [SerializeField] TextMeshProUGUI _WarningText;
        [SerializeField, SerializedDictionary("ID", "Aroma")] SerializedDictionary<string, BobaCup.TeaAroma> AromaIdToEnum;

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

        public override bool TryPutItem(ItemData item)
        {
            if (HeldItem != null)
            {
                RunWarningText("Take the finished product first...");
                return false;
            }
            if (item == null) return false;
            if (_inProcess)
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
            else if (item == Tea)
            {
                if (_currentCup.HasTea) return false;
                _currentCup.HasTea = true;
                OnTeaAdded?.Invoke();
                TryStartProcess();
            }
            else if (item == Boba)
            {
                if (_currentCup.HasBoba) return false;
                _currentCup.HasBoba = true;
                OnBobaAdded?.Invoke();
            }
            else if (item == Cup)
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
                if (_currentCup.AromaType != BobaCup.TeaAroma.NONE) return false;

                string id = item.ID;
                var keys = AromaIdToEnum.Keys;
                foreach (var k in keys) if (k == id) goto FoundID;
                return false;
                FoundID:;
                _currentCup.AromaType = AromaIdToEnum[id];
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

        public override bool TryPickItem(out ItemData item)
        {
            item = null;
            if (_inProcess) return false;
            if (HeldItem == null) return false;

            item = _heldItem;

            _heldItem = null;
            SetSpriteByData(null);
            TryStartProcess();

            return true;
        }
        public override bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem == null)
            {
                TryPickItem(out ItemData item);
                other.TryPutItem(item);
            }
            else
            {
                if (TryPutItem(other.HeldItem))
                {
                    other.TryPickItem(out _);
                }
            }

            return false;
        }

        void TryStartProcess()
        {
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
            _currentCup = new BobaCup();
            _cupAdded = false;

            _heldItem = _Product;
            SetSpriteByData(_heldItem);
            OnItemHeld(_heldItem);
            OnReset?.Invoke();
            _Slider.value = 0;

            _inProcess = false;
        }
    }
}
