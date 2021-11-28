<br />
<p align="center">
  <img src="HobbitSpeedrunTools/img/icon.png" alt="Logo" width="60" height="60">

  <h2 align="center">HobbitSpeedrunTools</h2>

  <p align="center">
    <i>A tool for effeciently practicing for speedruns of The Hobbit</i>
    <br />
    <a href="../../issues">Report Issue</a>
    -
    <a href="../../issues">Request Feature</a>
  </p>
</p>

# Table of Contents

- [About](#about)
- [Features](#features)
- [Usage](#usage)
- [Download](#download)
- [Warning](#warning)
- [Acknowledgements](#acknowledgements)

# About

This tool is made to effeciently practice for The Hobbit speedruns by providing cheats that are tailored for practice and by providing a save manager with built in saves for many scenarios.

<div align="center">
  <img src="assets/preview.png" alt="Application screenshot" width="auto" height="auto">
  <br />
  <img src="assets/preview_2.png" alt="Application screenshot" width="auto" height="auto" border="1px solid orange">
</div>
<br />

# Features

- **Practice Cheats**:
  - Developer mode gives access to the whole developer menu including flying, invulnerability and much more
  - Infinite jumpattacks removes the waiting when practicing slope boosts
  - Rendering triggers helps you learn and test routes quickly
  - Rendering polycache lets you see the geometry of everything in game
  - Automatically resetting signs lets you practice Riddles without having to worry about restarting your game
- **Save Manager**
  - Includes a large set of saves tailored for practice
  - Keeps your saves at the top of the "Load Game" screen for quick reloading
  - Easily expandable with your own saves
- **In Game Display**
  - When HobbitSpeedrunTools is active you can see which cheats and saves are activated in The Hobbit itself
- **Hotkeys**
  - Customizable hotkeys for every feature meaning you never have to leave the game
  - Shortcut to instantly restart levels

# Usage

## General usage

Start the program, tick the checkbox next to "Manage Saves" to enable the save manager and tick any of the cheat checkboxes to enable them. Note that The Hobbit must be running for the cheats to be activated.

**Only use HobbitSpeedrunTools for practice, make sure to close it and restart your game for real runs**

## Shortcuts

To edit the shortcuts hit the "Open Config" button. Edit the values for every shortcut in the config with the [key number](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=windowsdesktop-6.0) you wish to use. The modifier key is the key you hold while pressing the key of the feature you want to use and can be change to ctrl, alt, shift or win.

The default shortcuts are as follows:

| Shortcut        | Feature                    |
| --------------- | -------------------------- |
| Ctrl & 1        | Toggle Dev Tools           |
| Ctrl & 2        | Toggle Infinite Jumpattack |
| Ctrl & 3        | Toggle Load Triggers       |
| Ctrl & 4        | Toggle Other Triggers      |
| Ctrl & 5        | Toggle Polycache           |
| Ctrl & 6        | Toggle Auto Reset Signs    |
| Ctrl & 7        | Toggle Invincibility       |
| Ctrl & R        | Restart Level              |
| Ctrl & 0        | Toggle Save Manager        |
| Ctrl & +        | Next Save                  |
| Ctrl & -        | Previous Save              |
| Ctrl & PageUp   | Next Save Collection       |
| Ctrl & PageDown | Previous Save Collection   |

## Adding saves

To add your own saves to the save manager follow the following steps:

1. Navigate to your `HobbitSpeedrunTools` installation and into `save-collections`
2. Create a new folder starting with the following format: `[Number]. [Collection Name]`  
   _The number should be the position that you want the collection to appear in, a lower number will appear closer to the top, but make sure every number is only used once._
3. Move your saves into your newly created folder with the following format: `[Number]. [Save Name].hobbit`  
   _The number should be the position that you want the save to appear in, a lower number will appear closer to the top, but make sure every number is only used once._

**Note**: Make sure that only the `.hobbit` files are located within your new folder. Any other files might have unexpected results.

# Download

Download the most recent release [here](../../releases). Simply open the zip file and extract the full "HobbitSpeedrunTools" folder the location where you want to keep it. Then you can run the hobbit-save-manager.exe file inside the folder, and you're up and running.

You will need to have the [**32-bit .NET 5.0 runtime**](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-desktop-5.0.11-windows-x86-installer) if you don't already have it.

# Warning

Although the HobbitSpeedrunTools save manager is written to back up your old saves and restore them after use, I can't guarantee that this will always function as expected. Make sure to back up important saves yourself.

# Acknowledgements

- [Erfg12's Memory.dll](https://github.com/erfg12/memory.dll/) used for reading and writing to the game memory
- [ini-parser by Rickyah](https://github.com/rickyah/ini-parser) used to read and write to the config file
- [MD_Pi](https://www.youtube.com/user/MD0111000001101001) for sharing his cheat table filled with just about every memory address in the game
- [Tasz](https://www.twitch.tv/tasz) for providing save files for the 100% category
- [Chrixiam](https://www.twitch.tv/chrixiam98) for providing save files for the All Quests category
