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
        public BobaCup InstantiatedCup => _instantiatedCup;

        [SerializeField] internal Transform _SpriteParent;
        [SerializeField] internal string _SortLayer = "Item";
        [SerializeField] internal int _SortOrder;

        internal BobaCup _instantiatedCup = null;
        internal ItemData _heldItem = null;
        internal PhotonView _photonView = null;

        internal virtual void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        internal void SetSpriteByData(ItemData item, BobaCup cup)
        {
            if (item == null) _photonView.RPC(nameof(RPC_SetSpriteToNull), RpcTarget.All);
            else
            {
                string serializedCup = cup == null ? GV.UnassignedString : cup.Serialize();
                string itemID = item == null ? NULLITEM : item.ID;
                _photonView.RPC(nameof(RPC_SetSpriteByData), RpcTarget.All, itemID, serializedCup);
            }
        }

        [PunRPC]
        internal void RPC_SetSpriteByData(string itemID, string serializedCup)
        {
            ItemData item = itemID == NULLITEM ? null : GV.ItemDatabaseRef.GetItemDataByNameOrID(itemID);
            BobaCup newCup = serializedCup == GV.UnassignedString ? null : BobaCup.Deserialize(serializedCup);

            if (item == null)
            {
                RPC_SetSpriteToNull();
                return;
            }
            else if (item.UseSpritePrefab == false || item.SpritePrefab == null)
            {
                RPC_SetSpriteItemID(item.ID);
                return;
            }

            foreach (var child in _SpriteParent.Cast<Transform>()) Destroy(child.gameObject);
            _instantiatedCup = null;

            if (item != null)
            {
                var go = Instantiate(item.SpritePrefab, _SpriteParent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;

                go.GetComponent<ISpritePrefabScript>()?.Reinitialize(newCup);
                _instantiatedCup = newCup;
            }
        }

        internal void SetSpriteSerialized(int textureWidth, int textureHeight, byte[] textureBytes)
            => _photonView.RPC(nameof(RPC_SetSpriteSerialized), RpcTarget.All, textureWidth, textureHeight, textureBytes);

        [PunRPC]
        internal void RPC_SetSpriteSerialized(int textureWidth, int textureHeight, byte[] textureBytes)
        {
            if (textureBytes.Length <= 0 || textureWidth <= 0 || textureHeight <= 0) return;

            Texture2D tex = new Texture2D(textureWidth, textureHeight);
            ImageConversion.LoadImage(tex, textureBytes);
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, textureWidth, textureHeight), Vector2.one);

            SetSprite(sprite);
        }

        internal void SetSpriteItem(ItemData item) => _photonView.RPC(nameof(RPC_SetSpriteItemID), RpcTarget.All, item.ID);
        [PunRPC]
        internal void RPC_SetSpriteItemID(string itemID)
        {
            Sprite sprite = null;

            if (itemID != NULLITEM)
                sprite = GV.ItemDatabaseRef.GetItemDataByNameOrID(itemID)?.UISprite;

            SetSprite(sprite);
        }
        internal void SetSpriteToNull() => _photonView.RPC(nameof(RPC_SetSpriteToNull), RpcTarget.All);
        [PunRPC]
        internal void RPC_SetSpriteToNull() => SetSprite((Sprite)null);

        internal void SetSprite(Sprite sprite)
        {
            foreach (var child in _SpriteParent.Cast<Transform>()) Destroy(child.gameObject);
            _instantiatedCup = null;

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

        public virtual bool TryPutItem(ItemData item, BobaCup cup)
        {
            if (_heldItem != null) return false;
            if (item == null) return false;

            _photonView.RPC(nameof(RPC_SetHeldItem), RpcTarget.All, item.ID);
            SetSpriteByData(item, cup);

            return true;
        }
        public virtual bool TryPickItem(out ItemData item, out BobaCup cup)
        {
            item = _heldItem;
            cup = _instantiatedCup;

            _photonView.RPC(nameof(RPC_SetHeldItem), RpcTarget.All, NULLITEM);
            SetSpriteToNull();

            return true;
        }

        internal const string NULLITEM = "NULLITEMSTRING";
        [PunRPC]
        internal void RPC_SetHeldItem(string itemID)
        {
            if (itemID == null || itemID == NULLITEM) _heldItem = null;
            else _heldItem = GV.ItemDatabaseRef.AllItems.Find(x => x.ID == itemID);
        }

        public virtual bool ReplaceItems(ItemHolder_Base other)
        {
            if (other.HeldItem == null && HeldItem == null) return false;

            TryPickItem(out ItemData item1, out BobaCup cup1);
            other.TryPickItem(out ItemData item2, out BobaCup cup2);

            other.TryPutItem(item1, cup1);
            TryPutItem(item2, cup2);
            return true;
        }
    }
}
