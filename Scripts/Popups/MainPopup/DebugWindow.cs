using DebugMenu.Scripts.Popups;
using UnityEngine;

namespace DebugMenu.Scripts.Acts;

public class DebugWindow : BaseWindow
{
	public enum ToggleStates
	{
		Off, Minimal, All
	}
	
	public override string PopupName => "Debug Menu";
	public override Vector2 Size => new Vector2(700, 420);
	public override bool ClosableWindow => false;
	public GameWindow CurrentAct => currentAct;

	private GameWindow currentAct = null;
	private ToggleStates currentState = ToggleStates.Off;

	private Vector2 position;

	public DebugWindow() : base()
	{
		currentAct = new GameWindow(this);
		IsActive = true;
		isOpen = false;
	}

	public override void Update()
	{
		if (currentAct != null)
			currentAct.Update();
	}

	public override void OnGUI()
	{
		float scrollAreaWidth = Mathf.Max(TotalWidth, windowRect.width);
		float scrollAreaHeight = Mathf.Max(Height, windowRect.y);
		Rect contentSize = new Rect(new Vector2(0, 0), new Vector2(scrollAreaWidth, scrollAreaHeight));
		Rect viewportSize = new Rect(new Vector2(0, 0), Size - new Vector2(10, 0));
		position = GUI.BeginScrollView(viewportSize, position, contentSize);
		
		DrawToggleButtons();
		
		if (currentState > ToggleStates.Off)
		{
			base.OnGUI();

			currentAct.OnGUI(currentState);
		}

		GUI.EndScrollView();
	}

	private void DrawToggleButtons()
	{
		if (GUI.Button(new Rect(5f, 0f, 20f, 20f), "-"))
		{
			currentState = ToggleStates.Off;
			position = Vector2.zero;
		}

		if (GUI.Button(new Rect(25f, 0f, 20f, 20f), "+"))
		{
			currentState = ToggleStates.Minimal;
		}

		if (GUI.Button(new Rect(45F, 0f, 25f, 20f), "X"))
		{
			currentState = ToggleStates.All;
		}
	}

	protected override bool OnToggleWindowDraw()
	{
		switch (currentState)
		{
			case ToggleStates.Off:
				windowRect.Set(windowRect.x, windowRect.y, 100, 60);
				break;
			case ToggleStates.Minimal:
				windowRect.Set(windowRect.x, windowRect.y, ColumnWidth+40, Size.y);
				break;
			case ToggleStates.All:
				windowRect.Set(windowRect.x, windowRect.y, Size.x, Size.y);
				break;
		}
		BeginDrawingGUI();

		return false;
	}
}