using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Menu [] menus;
    public static MenuManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void openMenu(string menuToOpen)
    {
        foreach (var menu in menus)
        {
            if(menu.menuName == menuToOpen)
            {
                menu.Open();
            }
            else
            {
                menu.Close();
            }
        }
    }

    public void openMenu(Menu menuToOpen)
    {
        foreach (var menu in menus)
        {
            menu.Close();
        }
        menuToOpen.Open();
    }

    public void closeMenu(Menu menu)
    {
        menu.Close();
    }
}
