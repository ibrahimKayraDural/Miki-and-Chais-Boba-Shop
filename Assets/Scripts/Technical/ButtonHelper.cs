using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonHelper : MonoBehaviour
{
    [SerializeField] List<SerializableKeyValuePair<string, UnityEvent>> _Events;

    public void InvokeAll()
    {
        foreach (var item in _Events) item.Value?.Invoke();
    }

    public void Invoke(string key)
    {
        foreach (var item in _Events)
        {
            if (item.Key == key) item.Value?.Invoke();
        }
    }
}
