using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ItemHolder
{
    public class ItemHolder_Processor : ItemHolder_Base
    {
        [SerializeField] ItemData _Ingredient;
        [SerializeField] ItemData _Product;
        [SerializeField] float _ProcessTime = 1;
        [SerializeField] float _ProgressInterval = .05f;
        [SerializeField] Slider _Slider;

        bool _inProcess;

        private void Awake()
        {
            _Slider.maxValue = _ProcessTime;
            _Slider.value = 0;
            _Slider.gameObject.SetActive(false);
        }

        public override bool TryPutItem(ItemData item)
        {
            if (_inProcess) return false;
            if (_heldItem != null) return false;
            if (_Product == null || _Ingredient == null) return false;
            if (item != _Ingredient) return false;

            StartProcess();

            return true;
        }
        public override bool TryPickItem(out ItemData item)
        {
            item = null;
            if (HeldItem != _Product) return false;
            item = _heldItem;

            _heldItem = null;
            _HeldItemRenderer.sprite = null;

            return true;
        }
        public override bool ReplaceItems(ItemHolder_Base other)
        {
            ItemData item = other.HeldItem;
            if (item == null)
            {
                if (TryPickItem(out _))
                {
                    other.TryPutItem(_Product);
                    return true;
                }
            }
            else if (item == _Ingredient)
            {
                if (TryPutItem(_Ingredient))
                {
                    other.TryPickItem(out _);
                    return true;
                }
            }
            return false;
        }
        
        void StartProcess()
        {
            StopCoroutine(nameof(Process));
            StartCoroutine(nameof(Process));
        }
        IEnumerator Process()
        {
            float currentProgress = 0;
            _Slider.maxValue = _ProcessTime;
            _Slider.value = 0; 
            _Slider.gameObject.SetActive(true);

            _inProcess = true;
            while (true)
            {
                yield return new WaitForSeconds(_ProgressInterval);
                currentProgress += _ProgressInterval;
                _Slider.value = currentProgress;
                if (currentProgress >= _ProcessTime) break;
            }

            _Slider.gameObject.SetActive(false);
            OnProcessDone();
        }
        void OnProcessDone()
        {
            _heldItem = _Product;
            _HeldItemRenderer.sprite = _heldItem.UISprite;
            OnItemHeld(_heldItem);

            _inProcess = false;
        }
    }
}
