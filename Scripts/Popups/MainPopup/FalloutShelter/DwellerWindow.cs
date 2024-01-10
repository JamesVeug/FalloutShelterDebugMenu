using System.Collections.Generic;
using DebugMenu.Scripts.Popups;
using UnityEngine;

public class DwellerWindow : BaseWindow
{
    public override string PopupName => "Dwellers";
    public override Vector2 Size => new Vector2(750, 800);

    private Vector2 position;
    
    public override void OnGUI()
    {
        base.OnGUI();
        
        if(Button("Create Refugee"))
        {
            EDwellerRarity randomRarity = (EDwellerRarity)Random.Range(0, 4);
            MonoSingleton<DwellerSpawner>.Instance.CreateWaitingDweller(EGender.Any, false, 0, randomRarity);
        }


        ColumnWidth = 50;
        
        List<Dweller> buttonNames = Vault.Instance.Dwellers;
        int rows = buttonNames.Count + 2;
        int columns = 1;
        Rect scrollableAreaSize = new Rect(new Vector2(0, 0), new Vector2(columns * ColumnWidth + (columns - 1) * 10, rows * RowHeight));
        Rect scrollViewSize = new Rect(new Vector2(0, 0), Size - new Vector2(10, 25));
        position = GUI.BeginScrollView(scrollViewSize, position, scrollableAreaSize);

        foreach (Dweller dweller in buttonNames)
        {
            DrawDweller(dweller);
        }
        
        GUI.EndScrollView();
    }
    
    private void DrawDweller(Dweller dweller)
    {
        using (HorizontalScope(10))
        {
            LabelBold($"Name:", new Vector2(50, Height));
            Label($"{dweller.Name}", new Vector2(100, Height));
            
            LabelBold($"Level:", new Vector2(40, Height));
            Label($"{dweller.Experience.CurrentLevel}", new Vector2(50, Height));
            
            float xpForLevel = dweller.Experience.InvokePrivateMethod<float>("ExpForNextLevel");
            LabelBold($"XP:", new Vector2(30, Height));
            Label($"{dweller.Experience.ExperienceValue:N0}/{xpForLevel:N0}", new Vector2(150, Height));
            
            LabelBold($"Health:", new Vector2(50, Height));
            Label($"{dweller.Health.HealthValue:N0}/{dweller.Health.HealthMax:N0}", new Vector2(75, Height));
            
            LabelBold($"Happy:", new Vector2(50, Height));
            Label($"{dweller.Happiness.HappinessValue:N0}%", new Vector2(75, Height));
        }
    }
}