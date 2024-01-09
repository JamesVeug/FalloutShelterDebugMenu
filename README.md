# Debug Menu

Adds an in-game menu to the FalloutShelter-Steam to view and edit some stuff

![Alt text](https://github.com/JamesVeug/FalloutShelterDebugMenu/blob/main/github.png?raw=true "a title")

## Supports:
- Changing resources
- View all Dwellers info


## How to install
- Install https://github.com/BepInEx/BepInEx in your Fallout Shelter directory
- Download latest version https://github.com/JamesVeug/FalloutShelterDebugMenu/releases/tag/release
- Extract `System.Runtime.dll` and `FalloutShelterDebugMenu.dll` to `...Steam\steamapps\common\Fallout Shelter\BepInEx\plugins`
- Run game from Steam as normal


## How to setup Project (May be missing steps)
- Load `FalloutShelterDebugMenu` in Visual Studio / Rider
- Check project TargetFramework set to .NET 3.5 (You may need to install it)
- Add FalloutShelter .dll's as dependencies from `...Steam\steamapps\common\Fallout Shelter\FalloutShelter_Data\Managed`
  - `Assembly-CSharp.dll`
- Add all Unity .dll's as dependencies from `...Steam\steamapps\common\Fallout Shelter\FalloutShelter_Data\Managed`
  - `UnityEngine.CoreModule.dll`
  - `UnityEngine.IMGUIModule.dll`
  - `UnityEngine.UI.dll` from
  - `UnityEngine.TextRenderingModule.dll`
- Add BepInEx .dll's as dependencies from `...Steam\steamapps\common\Fallout Shelter\BepInEx\core`
  - `0Harmony.dll`
  - `BepInEx.dll`


## Contributing
If you want to contribute to the project, you can do so by making a pull request.
There is also a discord on my github if you want to discuss more.