using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemData : ScriptableObject
{
    public string Name => _Name;
    public string ID => _ID;
    public Sprite UISprite => _UISprite;
    public Vector2 HoldingOffset => _HoldingOffset;
    public int Price => _Price;
    public int MaxTip => _MaxTip;
    public float NoTipAfterSeconds => _NoTipAfterSeconds;

    [SerializeField] string _Name = GLOBALVALUES.UnassignedString;
    [SerializeField] string _ID = GLOBALVALUES.UnassignedString;
    [SerializeField] Sprite _UISprite;
    [SerializeField] Vector2 _HoldingOffset = Vector2.zero;
    [SerializeField] int _Price = 10;
    [SerializeField] int _MaxTip = 20;
    [SerializeField] float _NoTipAfterSeconds = 5;


    public bool Compare(ItemData other) => other.ID == ID;
}
