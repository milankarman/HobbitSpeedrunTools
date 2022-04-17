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

- [Table of Contents](#table-of-contents)
- [About](#about)
- [Features](#features)
- [Usage](#usage)
  - [General usage](#general-usage)
  - [Shortcuts](#shortcuts)
  - [Adding saves](#adding-saves)
- [Download](#download)
- [Warning](#warning)
- [Acknowledgements](#acknowledgements)

# About

This tool is made to effeciently practice for The Hobbit speedruns by providing cheats, a save manager and an information display that are tailored for practice and labbing. It comes with built in practice saves for various categories.

<div align="center">
  <img src="assets/preview.png" alt="Application screenshot" width="auto" height="auto">
  <br />
  <br />
  <img src="assets/preview_2.png" alt="Application screenshot" width="auto" height="auto">
</div>
<br />

# Features

- **Practice Cheats**:
  - Developer mode gives access to the whole developer menu including flying, invulnerability and much more
  - Infinite jumpattacks removes the waiting when practicing slope boosts
  - Rendering invisible walls and triggers to truly understand your route
  - Automatically resetting signs lets you practice Riddles without having to worry about restarting your game
  - Instantly reload your save with a single shortcut
  - And more!
- **Save Manager**
  - Includes a large set of saves tailored for practice
  - Keeps your saves at the top of the "Load Game" screen for quick reloading
  - Easily expandable with your own saves 
- **Information Display**
  - Keep track of Bilbo's exact position, rotation and your set clipwarp position to take
  the guesswork out of practice.
- **In Game Display**
  - When HobbitSpeedrunTools is active you can see which cheats and saves are activated in The Hobbit itself
- **Hotkeys**
  - Customizable hotkeys for every feature - meaning you never have to leave the game
  - Shortcut to instantly restart levels

# Usage

## General usage

Start the program and The Hobbit. Tick any of the checkboxes of the cheats you might want and select any save collection and save combination you might want. Everything else is automatic. Have a look at [shortcuts](#shortcuts) for useful shortcuts that 

To use fly mode you'll have to enable developer mode and hit CTRL + F in game. When flying you can hold N to clip through objects. Unfortunately this isn't rebindable as it's all programmed to work together this way in the game itself.

**Only use HobbitSpeedrunTools for practice, make sure to close it and restart your game for real runs**

## Shortcuts

To edit the shortcuts hit the "Open Config" button. Edit the values for every shortcut in the config with the [key number](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=windowsdesktop-6.0) you wish to use. The modifier key is the key you hold while pressing the key of the feature you want to use and can be change to ctrl, alt, shift or win.

The default shortcuts are as follows:

| Shortcut        | Feature                      |
| --------------- | ---------------------------- |
| Ctrl & 1        | Toggle Dev Tools             |
| Ctrl & 2        | Toggle Infinite Jumpattack   |
| Ctrl & 3        | Toggle Render Load Triggers  |
| Ctrl & 4        | Toggle Render Other Triggers |
| Ctrl & 5        | Toggle Render Polycache      |
| Ctrl & 6        | Toggle Invincibility         |
| Ctrl & 7        | Toggle Auto Reset Signs      |
| Ctrl & 8        | Toggle Lock Clipwarp         |
| Ctrl & R        | Reload Save                  |
| Ctrl & T        | Restart Level                |
| Ctrl & G        | Trigger Clipwarp             |
| Ctrl & +        | Next Save                    |
| Ctrl & -        | Previous Save                |
| Ctrl & PageUp   | Next Save Collection         |
| Ctrl & PageDown | Previous Save Collection     |

## Adding saves

To add your own saves to the save manager follow the following steps:

1. Navigate to your `HobbitSpeedrunTools` installation and into `save-collections`
2. Create a new folder starting with the following format: `[Number]. [Collection Name]`  
   _The number should be the position that you want the collection to appear in, a lower number will appear closer to the top, but make sure every number is only used once._
3. Move your saves into your newly created folder with the following format: `[Number]. [Save Name].hobbit`  
   _The number should be the position that you want the save to appear in, a lower number will appear closer to the top, but make sure every number is only used once._

**Note**: Make sure that only the `.hobbit` files are located within your new folder. Any other files might have unexpected results.

# Download

Download the most recent release [here](../../releases). Simply open the zip file and extract the full "HobbitSpeedrunTools" folder the location where you want to keep it. Then you can run the HobbitSpeedrunTools.exe file inside the folder, and you're up and running.

# Warning

Although the HobbitSpeedrunTools save manager is written to back up your old saves and restore them after use, I can't guarantee that this will always function as expected. Make sure to back up important saves yourself.

Currently new saves that are made while the save manager is active will be discarded when the save manager is disabled or HobbitSpeedrunTools is closed. To avoid this you can disable the save manager before making saves you want to keep.

# Acknowledgements

- [Erfg12's Memory.dll](https://github.com/erfg12/memory.dll/) used for reading and writing to the game memory
- [NonInvasiveKeyboardHook by Kfirprods](https://github.com/kfirprods/NonInvasiveKeyboardHook) used for assigning global hotkeys
- [ini-parser by Rickyah](https://github.com/rickyah/ini-parser) used to read and write to the config file
- [MD_Pi](https://www.youtube.com/user/MD0111000001101001) for sharing his cheat table filled with just about every memory address in the game
- [Tasz](https://www.twitch.tv/tasz) for providing save files for the 100% category
- [Chrixiam](https://www.twitch.tv/chrixiam98) for providing save files for the All Quests category
