using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaMachineAromaLightController : MonoBehaviour
{
    [SerializeField] List<SerializableKeyValuePair<string, GameObject>> AromaObjects = new List<SerializableKeyValuePair<string, GameObject>>();

    public void SetAromaObject(string aromaID)
    {
        AromaObjects.ForEach(x => x.Value.SetActive(x.Key == aromaID));
    }
}
