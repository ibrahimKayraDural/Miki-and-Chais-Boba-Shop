using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUIVisual : MonoBehaviour
{
    [SerializeField] List<Sprite> _CustomerSprites;
    [SerializeField] Image _CustomerImage;
    [SerializeField] Transform _Parent;
    [SerializeField] GameObject _IngredientCellPrefab;
    [SerializeField] PhotonView _PhotonView;

    int _lastCustomerIndex = -1;

    public void Clean()
    {
        foreach (Transform child in _Parent)
        {
            Destroy(child.gameObject);
        }
        _CustomerImage.enabled = false;
    }

    public void InitializeRecipie(BobaCup cup)
    {
        ItemDatabase id = GV.ItemDatabaseRef;
        List<ItemData> items = new List<ItemData>();
        if (cup.HasMilk) items.Add(id.Milk);
        if (cup.HasTea) items.Add(id.Tea);
        if (cup.HasBoba) items.Add(id.Boba);
        if (cup.Aroma != null) items.Add(cup.Aroma);

        foreach (var ingr in items)
        {
            GameObject go = Instantiate(_IngredientCellPrefab, _Parent);
            go.GetComponent<IngredientCell>().Initialize(ingr);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            int customerIdx = -1;
            for (int i = 0; i < 10; i++)
            {
                customerIdx = Random.Range(0, _CustomerSprites.Count);

                if (_CustomerSprites.Count == 0 || customerIdx != _lastCustomerIndex) break;
            }
            _lastCustomerIndex = customerIdx;
            _PhotonView.RPC(nameof(SetCustomerVisual), RpcTarget.All, customerIdx);
        }
    }

    [PunRPC]
    void SetCustomerVisual(int index)
    {
        _CustomerImage.sprite = _CustomerSprites[index];
        _CustomerImage.enabled = true;
    }
}
