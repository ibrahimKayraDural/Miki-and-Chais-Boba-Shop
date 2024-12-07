using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class BobaCup
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

    public static BobaCup GetRandomCup()
    {
        BobaCup randomCup = new BobaCup();
        int liquidRandom = Random.Range(0, 3);
        randomCup.HasMilk = liquidRandom != 0;
        randomCup.HasTea = liquidRandom != 1;
        randomCup.HasBoba = Random.Range(0, 2) == 0;

        List<ItemData> aromas = GLOBALVALUES.BobaDatabaseRef.Aromas;
        int aromaIDX = Random.Range(-1, aromas.Count);
        randomCup.Aroma = aromaIDX == -1 ? null : aromas[aromaIDX];

        return randomCup;
    }

    public bool Compare(BobaCup other)
    {
        bool returnBool = true;

        if (HasMilk != other.HasMilk) returnBool = false;
        if (HasTea != other.HasTea) returnBool = false;
        if (HasBoba != other.HasBoba) returnBool = false;
        if (Aroma != other.Aroma) returnBool = false;

        return returnBool;
    }
}
public class BobaCupController : MonoBehaviour, ISpritePrefabScript
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

    public void Initialize(BobaCup cup, string sortingLayerName = null)
    {
        _cupData = cup;

        //Debug.Log($"milk:{cup.HasMilk}, tea:{cup.HasTea}, boba{cup.HasBoba}, aroma{cup.Aroma}");

        Lid1.color = cup.HasTea ? TeaColor : MilkColor;
        Lid2.color = cup.HasMilk ? MilkColor : TeaColor;

        if (cup.HasMilk && cup.HasTea) Water.color = Color.Lerp(TeaColor, MilkColor, .5f);
        else Water.color = cup.HasTea ? TeaColor : MilkColor;

        if (cup.Aroma != null) AromaIcon.sprite = cup.Aroma.UISprite;
        Boba.gameObject.SetActive(cup.HasBoba);

        if(sortingLayerName != null)
        {
            foreach (var sr in GetComponentsInChildren<SpriteRenderer>(true))
            {
                sr.sortingLayerName = sortingLayerName;
            }
        }
    }
    public void Reinitialize(ISpritePrefabScript oldInterfaceScript = null)
    {
        if (oldInterfaceScript == null) return;
        BobaCupController bcc = oldInterfaceScript as BobaCupController;
        if (bcc == null) return;
        if (bcc.CupData == null) return;

        Initialize(bcc.CupData);
    }
}
