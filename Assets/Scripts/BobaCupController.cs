using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaCup
{
    public bool HasMilk;
    public bool HasTea;
    public bool HasBoba;
    public ItemData Aroma;

    public BobaCup()
    {
        HasMilk = false;
        HasTea = false;
        HasBoba = false;
        Aroma = null;
    }
    public BobaCup(bool hasMilk, bool hasTea, bool hasBoba, ItemData aroma)
    {
        HasMilk = hasMilk;
        HasTea = hasTea;
        HasBoba = hasBoba;
        Aroma = aroma;
    }
    public BobaCup(bool hasMilk, bool hasTea, bool hasBoba)
    {
        HasMilk = hasMilk;
        HasTea = hasTea;
        HasBoba = hasBoba;
        Aroma = null;
    }
}
public class BobaCupController : MonoBehaviour
{
    public BobaCup CupData => _cupData;

    [SerializeField] SpriteRenderer Lid1;
    [SerializeField] SpriteRenderer Lid2;
    [SerializeField] SpriteRenderer Water;
    [SerializeField] SpriteRenderer AromaIcon;
    [SerializeField] SpriteRenderer Boba;
    [SerializeField] Color TeaColor = Color.red;
    [SerializeField] Color MilkColor = Color.white;

    BobaCup _cupData = null;

    public void Initialize(BobaCup cup)
    {
        _cupData = cup;

        //Debug.Log($"milk:{cup.HasMilk}, tea:{cup.HasTea}, boba{cup.HasBoba}, aroma{cup.Aroma}");

        Lid1.color = cup.HasTea ? TeaColor : MilkColor;
        Lid2.color = cup.HasMilk ? MilkColor : TeaColor;

        if (cup.HasMilk && cup.HasTea) Water.color = Color.Lerp(TeaColor, MilkColor, .5f);
        else Water.color = cup.HasTea ? TeaColor : MilkColor;

        if (cup.Aroma != null) AromaIcon.sprite = cup.Aroma.UISprite;
        Boba.gameObject.SetActive(cup.HasBoba);
    }
}
