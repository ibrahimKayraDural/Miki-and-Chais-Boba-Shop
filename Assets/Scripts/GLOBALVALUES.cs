
using UnityEngine;

public static class GLOBALVALUES
{
    public const string UnassignedString = "UNASSIGNED";
    public const int MaxMoneyPerDay = 999;
    public static BobaDatabase BobaDatabaseRef
    {
        get
        {
            if (AUTO_BobaDatabase == null) AUTO_BobaDatabase = Resources.Load<BobaDatabase>("BobaDatabase");
            if (AUTO_BobaDatabase == null) Debug.LogError("Boba database cannot be found. Please create a boba database at Resources named ''BobaDatabase''.");
            return AUTO_BobaDatabase;
        }
    }
    static BobaDatabase AUTO_BobaDatabase = null;
}
