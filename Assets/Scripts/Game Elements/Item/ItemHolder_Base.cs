using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemHolder
{
    [RequireComponent(typeof(PhotonView))]
    public abstract class ItemHolder_Base : MonoBehaviour
    {
        public ItemData HeldItem => _heldItem;
        public GameObject InstantiatedSpritePrefab => _instantiatedSpritePrefab;

        [SerializeField] internal Transform _SpriteParent;
        [SerializeField] internal string _SortLayer = "Item";
        [SerializeField] internal int _SortOrder;

        internal ItemData _heldItem = null;
        internal GameObject _instantiatedSpritePrefab = null;
        internal PhotonView _photonView = null;

        internal virtual void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        internal void SetSpriteByData(ItemData item, GameObject instantiatedSpritePrefab)
        {
            if (item == null)
            {
                SetSprite(null);
                return;
            }
            else if (item.UseSpritePrefab == false || item.SpritePrefab == null)
            {
                SetSprite(item.UISprite);
                return;
            }

            var targetPrefab = instantiatedSpritePrefab ?? item.SpritePrefab;
            ISpritePrefabScript OldISPS = null;
            if (instantiatedSpritePrefab)
                OldISPS = instantiatedSpritePrefab.GetComponentInChildren<ISpritePrefabScript>();

            foreach (var child in _SpriteParent.Cast<Transform>()) Destroy(child.gameObject);

            _instantiatedSpritePrefab = null;
            if (item != null)
            {
                var go = Instantiate(targetPrefab, _SpriteParent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.GetComponent<ISpritePrefabScript>()?.Reinitialize(OldISPS);

                _instantiatedSpritePrefab = go;
            }
        }
        internal void SetSprite(Sprite sprite)
        {
            foreach (var child in _SpriteParent.Cast<Transform>()) Destroy(child.gameObject);
            _instantiatedSpritePrefab = null;
            if (sprite != null)
            {
                var go = new GameObject(gameObject.name + "_sprite", new System.Type[] { typeof(SpriteRenderer) });
                go.transform.parent = _SpriteParent;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = sprite;
                sr.sortingLayerName = _SortLayer;
                sr.sortingOrder = _SortOrder;
            }
        }

        public virtual bool TryPutItem(ItemData item, GameObject instantiatedSpritePrefab)
        {
            if (_heldItem != null) return false;
            if (item == null) return false;

            _photonView.RPC(nameof(RPC_SetHeldItem), RpcTarget.All, item.ID);

            SetSpriteByData(item, instantiatedSpritePrefab);


            return true;
        }
        public virtual bool TryPickItem(out ItemData item, out GameObject instantiatedSpritePrefab)
        {
            item = _heldItem;
            instantiatedSpritePrefab = _instantiatedSpritePrefab;

            _photonView.RPC(nameof(RPC_SetHeldItem), RpcTarget.All, NULLITEM);

            SetSpriteByData(null, null);

            return true;
        }

        const string NULLITEM = "NULLITEMSTRING";
        [PunRPC]
        internal void RPC_SetHeldItem(string itemID)
        {
            if (itemID == null || itemID == NULLITEM) _heldItem = null;
            else _heldItem = GV.ItemDatabaseRef.AllItems.Find(x => x.ID == itemID);
        }

        public virtual bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem == null && HeldItem == null) return false;

            TryPickItem(out ItemData item1, out GameObject isp1);
            other.TryPickItem(out ItemData item2, out GameObject isp2);

            other.TryPutItem(item1, isp1);
            TryPutItem(item2, isp2);
            return true;
        }
    }
}
