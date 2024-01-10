using System;
using System.Collections.Generic;
using System.Linq;
using DebugMenu.Scripts.Popups;
using DebugMenu.Scripts.Utils;
using UnityEngine;

public class InventoryWindow : BaseWindow
{
    public override string PopupName => "Inventory";
    public override Vector2 Size => new Vector2(900, 800);

    private EItemType itemTypeFilter;
    private string nameFilter;
    private Vector2 position;
    
    
    public override void OnGUI()
    {
        base.OnGUI();
        ColumnWidth = 200;
        
        List<DwellerItem> buttonNames = GetItems();
        int rows = buttonNames.Count + 2;
        int columns = 1;
        Rect scrollableAreaSize = new Rect(new Vector2(0, 0), new Vector2(columns * ColumnWidth + (columns - 1) * 10, rows * RowHeight));
        Rect scrollViewSize = new Rect(new Vector2(0, 0), Size - new Vector2(10, 25));
        position = GUI.BeginScrollView(scrollViewSize, position, scrollableAreaSize);

        
        LabelHeader("Inventory");

        using (HorizontalScope(2))
        {
            Label("Filter");
            Dropdown<EItemType>(itemTypeFilter.ToString(), (a) => itemTypeFilter = (EItemType)a);
        }

        using (HorizontalScope(2))
        {
            Label("Name");
            nameFilter = TextField(nameFilter);
        }
        
        ColumnWidth = Size.x - 50;
        foreach (DwellerItem item in buttonNames)
        {
            if (itemTypeFilter != EItemType.None && item.ItemType != itemTypeFilter)
                continue;

            if (!string.IsNullOrEmpty(nameFilter) && !item.GetName().ContainsText(nameFilter, false))
                continue;
            
            DrawItem(item);
        }
        
        GUI.EndScrollView();
    }

    private List<DwellerItem> GetItems()
    {
        List<DwellerItem> items = new List<DwellerItem>(Vault.Instance.Inventory.Items);
        items.Sort((a, b) =>
        {
            if (a.ItemType != b.ItemType)
                return (int)a.ItemType - (int)b.ItemType;
            
            return (int)a.GetItemData().ItemRarity - (int)b.GetItemData().ItemRarity;
        });
        return items;
    }

    private void DrawItem(DwellerItem item)
    {
        DwellerBaseItem data = item.GetItemData();
        
        using (HorizontalScope(14))
        {
            if (Button("Sell", new Vector2(50, Height)))
            {
                SellItem(item);
            }
            
            Label($"{item.GetName()}", new Vector2(150, Height));
            
            LabelBold($"Type:", new Vector2(50, Height));
            Label($"{item.ItemType}", new Vector2(100, Height));
            
            LabelBold($"Rarity:", new Vector2(50, Height));
            Label($"{data.ItemRarity}", new Vector2(100, Height));

            // + 8 extra fields
            switch (item.ItemType)
            {
                case EItemType.None:
                    break;
                case EItemType.Outfit:
                    DrawOutfit(item, (DwellerOutfitItem)data);
                    break;
                case EItemType.Weapon:
                    DrawWeapon(item, (DwellerWeaponItem)data);
                    break;
                case EItemType.Decoration:
                    break;
                case EItemType.Junk:
                    break;
                case EItemType.Pet:
                    break;
                case EItemType.Theme:
                    break;
            }
            
        }
    }

    private void SellItem(DwellerItem item)
    {
        if (MonoSingleton<Vault>.IsInstanceValid)
        {
            DwellerItem item2 = MonoSingleton<Vault>.Instance.Inventory.HandleItem(item.Id);
            MonoSingleton<Vault>.Instance.Inventory.RemoveItem(item2);
            MonoSingleton<Vault>.Instance.OnItemSold(item2, null);
            
            int num2 = item.GetNukaExchangeValue();
            Storage storage = new Storage();
            storage.AddResource(new GameResources(EResource.Nuka, num2));
            Storage.TriggerResourcesCallback(storage.Resources, storage.Owner, true);
            MonoSingleton<ResourceParticleMgr>.Instance.CollectResourcesGUI(Vector3.one, storage, true, false);
        }
    }

    private void DrawWeapon(DwellerItem item, DwellerWeaponItem data)
    {
        LabelBold($"Damage:", new Vector2(75, Height));
        Label($"{data.DamageMin}-{data.DamageMax}", new Vector2(50, Height));
        
    }

    private void DrawOutfit(DwellerItem item, DwellerOutfitItem data)
    {
        LabelBold($"Stats:", new Vector2(50, Height));
        for(int i = 1; i < (int)ESpecialStat.Max; i++)
        {
            ESpecialStat stat = (ESpecialStat)i;
            char s = stat.ToString()[0];
            var d = data.ModificationStats.FirstOrDefault(a => a.Type == stat);
            int v = d != null ? d.Value : 0;
            Label($"{s}:{v}", new Vector2(40, Height));
        }
    }
}