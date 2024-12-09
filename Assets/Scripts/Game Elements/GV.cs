
using Photon.Pun;
using System.IO;
using UnityEngine;

public static class GV //GLOBALVALUES
{
    public const string UnassignedString = "UNASSIGNED";
    public const int MaxMoneyPerDay = 9999;
    public static string PhotonPrefabPath(string prefabName) => Path.Combine("PhotonPrefabs", prefabName);
    public static ItemDatabase ItemDatabaseRef
    {
        get
        {
            if (AUTO_ItemDatabase == null) AUTO_ItemDatabase = Resources.Load<ItemDatabase>("Database/ItemDatabase");
            if (AUTO_ItemDatabase == null) Debug.LogError("Item database cannot be found. Please create an item database at Resources/Database/ named ''ItemDatabase''.");
            return AUTO_ItemDatabase;
        }
    }
    static ItemDatabase AUTO_ItemDatabase = null;

    public static GameObject PhotonInstantiate(GameObject original, Transform parent)
    {
        var trans = PhotonNetwork.Instantiate(PhotonPrefabPath(original.name), parent.position, Quaternion.identity).transform;
        trans.parent = parent;
        trans.localPosition = Vector3.zero;
        trans.localScale = Vector3.one;

        return trans.gameObject;
    }
}
