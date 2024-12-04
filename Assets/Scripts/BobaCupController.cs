using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaCup
{
    public enum TeaAroma { NONE, Strawberry, Apple, Orange }

    public bool HasMilk;
    public bool HasTea;
    public bool HasBoba;
    public TeaAroma AromaType;

    public BobaCup()
    {
        HasMilk = false;
        HasTea = false;
        HasBoba = false;
        AromaType = TeaAroma.NONE;
    }
    public BobaCup(bool hasMilk, bool hasTea, bool hasBoba, TeaAroma aromaType)
    {
        HasMilk = hasMilk;
        HasTea = hasTea;
        HasBoba = hasBoba;
        AromaType = aromaType;
    }
    public BobaCup(bool hasMilk, bool hasTea, bool hasBoba)
    {
        HasMilk = hasMilk;
        HasTea = hasTea;
        HasBoba = hasBoba;
        AromaType = TeaAroma.NONE;
    }
}

public class BobaCupController : MonoBehaviour
{
    [SerializeField] SpriteRenderer Lid1;
    [SerializeField] SpriteRenderer Lid2;
    [SerializeField] SpriteRenderer Water;
    [SerializeField] SpriteRenderer AromaIcon;
    [SerializeField] SpriteRenderer Boba;

    [SerializeField] Color TeaColor = Color.red;
    [SerializeField] Color MilkColor = Color.white;
}
