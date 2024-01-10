using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using DebugMenu.Scripts.Hotkeys;
using DebugMenu.Scripts.Popups;
using HarmonyLib;
using UnityEngine;

namespace DebugMenu
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
	    public const string PluginGuid = "jamesgames.falloutshelter.debugmenu";
	    public const string PluginName = "Debug Menu";
	    public const string PluginVersion = "0.1.0";

	    public static Plugin Instance;
	    public static ManualLogSource Log;
	    public static HotkeyController Hotkeys;
	    
	    public static string PluginDirectory;
	    public static float StartingFixedDeltaTime;

	    public static List<BaseWindow> AllWindows = new();
	    
	    private bool showDebugMenu = true;

        private void Awake()
        {
	        Instance = this;
	        Log = Logger;
	        StartingFixedDeltaTime = Time.fixedDeltaTime;
	        Hotkeys = new HotkeyController();
	        
            PluginDirectory = this.Info.Location.Replace("FalloutShelterDebugMenu.dll", "");

            new Harmony(PluginGuid).PatchAll();

            // Get all types of BaseWindow, instntiate them and add them to allwindows
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            for (int i = 0; i < types.Length; i++)
			{
	            Type type = types[i];
	            if (type.IsSubclassOf(typeof(BaseWindow)))
	            {
		            Logger.LogDebug($"Made {type}!");	   
		            AllWindows.Add((BaseWindow)Activator.CreateInstance(type));
	            }
			}

            Logger.LogInfo($"Loaded {PluginName}!");	        
        }

        private void Update()
        {
	        if (Input.GetKeyUp(KeyCode.BackQuote))
	        {
		        showDebugMenu = !showDebugMenu;
	        }
	        
	        if (!showDebugMenu)
		        return;
	        
	        for (int i = 0; i < AllWindows.Count; i++)
	        {
		        if(AllWindows[i].IsActive)
					AllWindows[i].Update();
	        }

	        // TODO: Hotkeys.Update();
        }

        private void OnGUI()
        {
	        if (!showDebugMenu)
		        return;
	        
	        for (int i = 0; i < AllWindows.Count; i++)
	        {
		        if(AllWindows[i].IsActive)
					AllWindows[i].OnWindowGUI();
	        }
        }

        public T ToggleWindow<T>() where T : BaseWindow, new()
        {
	        return (T)ToggleWindow(typeof(T));
        }

        public BaseWindow ToggleWindow(Type t)
        {
	        for (int i = 0; i < AllWindows.Count; i++)
	        {
		        BaseWindow window = AllWindows[i];
		        if (window.GetType() == t)
		        {
			        window.IsActive = !window.IsActive;
			        return window;
		        }
	        }

	        return null;
        }
        
        public T GetWindow<T>() where T : BaseWindow
        {
	        for (int i = 0; i < AllWindows.Count; i++)
	        {
		        T window = (T)AllWindows[i];
		        if (window.GetType() == typeof(T))
			        return window;
	        }

	        return null;
        }
    }
}
