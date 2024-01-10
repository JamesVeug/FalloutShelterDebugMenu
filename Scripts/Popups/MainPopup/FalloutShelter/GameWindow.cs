using System;
using DebugMenu;
using DebugMenu.Scripts.Acts;
using DebugMenu.Scripts.Popups;
using UnityEngine;
using UnityEngine.UI;

public partial class GameWindow
{
    private readonly DebugWindow Window;

    public GameWindow(DebugWindow window)
    {
        Window = window;
    }
    
    public void Update()
    {
        
    }

    public void OnGUI(DebugWindow.ToggleStates currentState)
    {
        if (Window.Button("Resources"))
        {
            Plugin.Instance.ToggleWindow<ResourcesWindow>();
        }

        if (Window.Button("Dwellers"))
        {
            Plugin.Instance.ToggleWindow<DwellerWindow>();
        }

        if (Window.Button("Inventory"))
        {
            Plugin.Instance.ToggleWindow<InventoryWindow>();
        }
    }
}