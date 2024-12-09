using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Item Database", fileName = "ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> AllItems => _AllItems;
    public ItemData Milk => _Milk;
    public ItemData Tea => _Tea;
    public ItemData Boba => _Boba;
    public ItemData Cup => _Cup;
    public List<ItemData> Aromas => _Aromas;


    [SerializeField] List<ItemData> _AllItems;

    [Space(15), Header("Boba Values")]
    [SerializeField] ItemData _Milk;
    [SerializeField] ItemData _Tea;
    [SerializeField] ItemData _Boba;
    [SerializeField] ItemData _Cup;
    [SerializeField] List<ItemData> _Aromas;

    public ItemData GetItemDataByNameOrID(string nameOrID) => _AllItems.Find(x => x.ID == nameOrID || x.DisplayName == nameOrID);
}
