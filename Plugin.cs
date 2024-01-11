using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using DebugMenu.Scripts.Hotkeys;
using DebugMenu.Scripts.Popups;
using Game.BuildSystem;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace DebugMenu
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
	    public const string PluginGuid = "jamesgames.falloutshelter.debugmenu";
	    public const string PluginName = "Debug Menu";
	    public const string PluginVersion = "0.2.0";

	    public static Plugin Instance;
	    public static ManualLogSource Log;
	    public static HotkeyController Hotkeys;
	    
	    public static string PluginDirectory;
	    public static float StartingFixedDeltaTime;

	    public static List<BaseWindow> AllWindows = new();
	    
	    private bool showDebugMenu = true;
	    private GameObject blockerParent = null;
	    private Canvas blockerParentCanvas = null;

        private void Awake()
        {
	        Instance = this;
	        Log = Logger;
	        StartingFixedDeltaTime = Time.fixedDeltaTime;
	        Hotkeys = new HotkeyController();
	        
	        Log.LogInfo($"{Screen.width}x{Screen.height} {Application.targetFrameRate}");
            PluginDirectory = Info.Location.Replace("FalloutShelterDebugMenu.dll", "");

            blockerParent = new GameObject("DebugMenuBlocker");
            blockerParent.transform.SetParent(transform);
            blockerParent.layer = LayerMask.NameToLayer("2D UI");
            blockerParentCanvas = blockerParent.AddComponent<Canvas>();
            blockerParentCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            blockerParentCanvas.sortingOrder = 32767;
            blockerParent.AddComponent<CanvasScaler>();
            blockerParent.AddComponent<GraphicRaycaster>();
            
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
		        blockerParentCanvas.enabled = showDebugMenu;
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

	        rectTransformPool.AddRange(activeRectTransforms);
	        activeRectTransforms.Clear();
	        
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

        private List<RectTransform> activeRectTransforms = new List<RectTransform>();
        private List<RectTransform> rectTransformPool = new List<RectTransform>();
        
        public RectTransform GetWindowBlocker()
        {
	        if (rectTransformPool.Count == 0)
	        {
		        GameObject myGO = new GameObject("WindowBlocker", typeof(RectTransform));
		        myGO.transform.SetParent(blockerParent.transform);
		        myGO.layer = LayerMask.NameToLayer("2D UI");
		        
		        Image image = myGO.AddComponent<Image>();
		        Color color = Color.magenta;
		        color.a = 0;
		        image.color = color;
		        
		        RectTransform blocker = myGO.GetComponent<RectTransform>();
		        blocker.sizeDelta = new Vector2(Screen.width / 4, Screen.height / 4);
		        blocker.anchoredPosition = Vector2.zero;
		        blocker.pivot = new Vector2(0.0f, 1.0f);
		        blocker.anchorMin = Vector2.zero;
		        blocker.anchorMax = Vector2.zero;
		        
		        activeRectTransforms.Add(blocker);
		        return blocker;
	        }

	        RectTransform rectTransform = rectTransformPool[rectTransformPool.Count - 1];
	        rectTransformPool.RemoveAt(rectTransformPool.Count - 1);
	        activeRectTransforms.Add(rectTransform);
	        return rectTransform;
        }
    }
}
