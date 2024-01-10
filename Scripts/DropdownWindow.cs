using System;
using System.Collections.Generic;
using DebugMenu;
using DebugMenu.Scripts.Popups;
using UnityEngine;

public class DropdownWindow : BaseWindow
{
    public override string PopupName => "";
    public override Vector2 Size => new Vector2(150, 300);

    private List<string> names = new();
    private List<object> values = new();
    private Vector2 position;
    private Action<object> callback;
    
    public override void OnGUI()
    {
        base.OnGUI();
        ColumnWidth = Size.x-50;
        RowHeight = 20;
        
        int rows = values.Count + 1;
        int columns = 1;
        Rect scrollableAreaSize = new Rect(new Vector2(0, 0), new Vector2(columns * ColumnWidth + (columns - 1) * 10, rows * RowHeight));
        Rect scrollViewSize = new Rect(new Vector2(0, 0), Size - new Vector2(10, 25));
        position = GUI.BeginScrollView(scrollViewSize, position, scrollableAreaSize);

        for (int i = 0; i < names.Count; i++)
        {
            if (Button(names[i]))
            {
                callback?.Invoke(values[i]);
            }
        }

        GUI.EndScrollView();
    }

    public static void Show(Type type, Action<object> callback, Vector2 position)
    {
        DropdownWindow dropdown = Plugin.Instance.ToggleWindow<DropdownWindow>();
        dropdown.windowRect.position = position;
        dropdown.names.Clear();
        dropdown.values.Clear();
        dropdown.callback = callback;
        
        foreach (object value in Enum.GetValues(type))
        {
            dropdown.values.Add(value);
        }
        
        foreach (string value in Enum.GetNames(type))
        {
            dropdown.names.Add(value);
        }
    }
}