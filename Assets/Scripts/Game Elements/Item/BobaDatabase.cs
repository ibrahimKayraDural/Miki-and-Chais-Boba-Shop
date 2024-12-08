using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Boba Database", fileName = "BobaDatabase")]
public class BobaDatabase : ScriptableObject
{
    public ItemData Milk  => _Milk;  
    public ItemData Tea  => _Tea;  
    public ItemData Boba  => _Boba;  
    public ItemData Cup  => _Cup;
    public List<ItemData> Aromas => _Aromas;

    [SerializeField] ItemData _Milk;
    [SerializeField] ItemData _Tea;
    [SerializeField] ItemData _Boba;
    [SerializeField] ItemData _Cup;
    [SerializeField] List<ItemData> _Aromas;
}
