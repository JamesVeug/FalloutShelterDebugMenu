using System;
using System.Collections.Generic;
using DebugMenu.Scripts.Popups;
using UnityEngine;

public class ResourcesWindow : BaseWindow
{
    public override string PopupName => "Resources";
    public override Vector2 Size => new Vector2(600, (int)(EResource.Count + 2) * RowHeight);

    private Vector2 position;
    
    public override void OnGUI()
    {
        base.OnGUI();

        ColumnWidth = Size.x - 50;
        
        int rows = (int)EResource.Count + 1;
        int columns = 1;
        Rect scrollableAreaSize = new Rect(new Vector2(0, 0), new Vector2(columns * ColumnWidth + (columns - 1) * 10, rows * RowHeight));
        Rect scrollViewSize = new Rect(new Vector2(0, 0), Size - new Vector2(10, 25));
        position = GUI.BeginScrollView(scrollViewSize, position, scrollableAreaSize);

        // Nuka = 0,
        // Food = 1,
        // Energy = 2,
        // Water = 3,
        // StimPack = 4,
        // RadAway = 5,
        // Lunchbox = 6,
        // MrHandy = 7,
        // PetCarrier = 8,
        // CraftedOutfit = 9,
        // CraftedWeapon = 10, // 0x0000000A
        // NukaColaQuantum = 11, // 0x0000000B
        // CraftedTheme = 12, // 0x0000000C
        // Count = 13, // 0x0000000D
        // None = 1000, // 0x000003E8

        GameResources resources = Vault.Instance.Storage.Resources;
        foreach (EResource resource in Enum.GetValues(typeof(EResource)))
        {
            if (resource != EResource.Count && resource != EResource.None)
                DrawResource(resource, resources[(int)resource]);
        }
        
        GUI.EndScrollView();
    }
    private void DrawResource(EResource resources, float value)
    {
        using (HorizontalScope(6))
        {
            Label($"{resources.ToString()}:");
            Label($"{value}");
            if (Button("-1mil"))
            {
                AddResource(resources, -1_000_000);
            }
            if (Button("-1k"))
            {
                AddResource(resources, -1_000);
            }
            
            if (Button("+1k"))
            {
                AddResource(resources, 1_000);
            }
            if (Button("+1mil"))
            {
                AddResource(resources, 1_000_000);
            }

        }
    }

    private void AddResource(EResource resources, int value)
    {
        Storage storage = new Storage();
        GameResources gameResources = new GameResources();
        gameResources.Set(resources, value);
        storage.AddResource(gameResources);
        storage.TransferResourcesTo(Vault.Instance.Storage);
    }
}