using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance { get; private set; } = null;

    [SerializeField] UnityEvent _OnOpened;
    [SerializeField] UnityEvent _OnClosed;

    bool _isPaused;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    public void ToggleMenu()
    {
        if (_isPaused) CloseMenu();
        else OpenMenu();
    }

    public void OpenMenu()
    {
        _OnOpened?.Invoke();
        _isPaused = true;
    }

    public void CloseMenu()
    {
        _OnClosed?.Invoke();
        _isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
