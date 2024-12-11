using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open = false;

    [SerializeField] UnityEvent _OnOpened;
    [SerializeField] UnityEvent _OnClosed;

    public void Open()
    {
        _OnOpened?.Invoke();
        open = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        _OnClosed?.Invoke();
        open = false;
        gameObject.SetActive(false);
    }
}
