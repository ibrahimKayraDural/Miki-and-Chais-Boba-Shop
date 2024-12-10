using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

    public static BobaCup GetRandomCup()
    {
        BobaCup randomCup = new BobaCup();
        int liquidRandom = Random.Range(0, 3);
        randomCup.HasMilk = liquidRandom != 0;
        randomCup.HasTea = liquidRandom != 1;
        randomCup.HasBoba = Random.Range(0, 2) == 0;

        List<ItemData> aromas = GV.ItemDatabaseRef.Aromas;
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

    const string NULLAROMASTRING = "NULLAROMA";
    public string Serialize()
    {
        string serializedStr = "";
        serializedStr += (HasMilk ? "1" : "0") + "/";
        serializedStr += (HasTea  ? "1" : "0") + "/";
        serializedStr += (HasBoba ? "1" : "0") + "/";
        serializedStr += Aroma == null ? NULLAROMASTRING : Aroma.ID;
        return serializedStr;
    }

    public static BobaCup Deserialize(string serializedString)
    {
        string[] deconstructedStr = serializedString.Split("/");
        if (deconstructedStr.Length < 4) return null;

        bool hasMilk = deconstructedStr[0] == "1";
        bool hasTea = deconstructedStr[1] == "1";
        bool hasBoba = deconstructedStr[2] == "1";
        string aromaID = deconstructedStr[3];
        ItemData aroma = aromaID == NULLAROMASTRING ? null : GV.ItemDatabaseRef.Aromas.Find(x => x.ID == aromaID);

        return new BobaCup(hasMilk, hasTea, hasBoba, aroma);
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

        if (sortingLayerName != null)
        {
            foreach (var sr in GetComponentsInChildren<SpriteRenderer>(true))
            {
                sr.sortingLayerName = sortingLayerName;
            }
        }
    }
    public void Reinitialize(BobaCup cup)
    {
        if (cup == null) return;

        Initialize(cup);
    }
}
