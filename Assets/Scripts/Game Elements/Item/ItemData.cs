using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemData : ScriptableObject
{
    public string DisplayName => _DisplayName;
    public string ID => _ID;
    public bool UseSpritePrefab => _UseSpritePrefab;
    public Sprite UISprite => _UISprite;
    public GameObject SpritePrefab => _SpritePrefab;
    public Vector2 HoldingOffset => _HoldingOffset;
    public int Price => _Price;
    public int MaxTip => _MaxTip;
    public float NoTipAfterSeconds => _NoTipAfterSeconds;

    [SerializeField] string _DisplayName = GV.UnassignedString;
    [SerializeField] string _ID = GV.UnassignedString;
    [SerializeField] bool _UseSpritePrefab;
    [SerializeField] Sprite _UISprite;
    [SerializeField] GameObject _SpritePrefab;
    [SerializeField] Vector2 _HoldingOffset = Vector2.zero;
    [SerializeField] int _Price = 10;
    [SerializeField] int _MaxTip = 20;
    [SerializeField] float _NoTipAfterSeconds = 5;


    public bool Compare(ItemData other) => other.ID == ID;
}
