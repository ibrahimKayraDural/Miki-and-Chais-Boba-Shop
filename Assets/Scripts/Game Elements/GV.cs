
using Photon.Pun;
using System.IO;
using UnityEngine;

public static class GV //GLOBALVALUES
{
    public const string UnassignedString = "UNASSIGNED";
    public const int MaxMoneyPerDay = 9999;
    public static string PhotonPrefabPath(string prefabName) => Path.Combine("PhotonPrefabs", prefabName);
    public static BobaDatabase BobaDatabaseRef
    {
        get
        {
            if (AUTO_BobaDatabase == null) AUTO_BobaDatabase = Resources.Load<BobaDatabase>("Database/BobaDatabase");
            if (AUTO_BobaDatabase == null) Debug.LogError("Boba database cannot be found. Please create a boba database at Resources named ''BobaDatabase''.");
            return AUTO_BobaDatabase;
        }
    }
    static BobaDatabase AUTO_BobaDatabase = null;

    public static GameObject PhotonInstantiate(GameObject original, Transform parent)
    {
        var trans = PhotonNetwork.Instantiate(PhotonPrefabPath(original.name), parent.position, Quaternion.identity).transform;
        trans.parent = parent;
        trans.localPosition = Vector3.zero;
        trans.localScale = Vector3.one;

        return trans.gameObject;
    }
}
