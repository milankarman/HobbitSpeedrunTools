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

# Usage

## General usage

Start the program, tick the checkbox next to "Manage Saves" to enable the save manager and tick any of the cheat checkboxes to enable them. Note that The Hobbit must be running for the cheats to be activated.

## Adding saves

To add your own saves to the save manager follow the following steps:

1. Navigate to your `HobbitSpeedrunTools` installation and into `save-collections`
2. Create a new folder starting with the following format: `[Number]. [Collection Name]`  
*The number should be the position that you want the collection to appear in, a lower number will appear closer to the top, but make sure every number is only used once.*
3. Move your saves into your newly created folder with the following format: `[Number]. [Save Name].hobbit`  
*The number should be the position that you want the save to appear in, a lower number will appear closer to the top, but make sure every number is only used once.*

**Note**: Make sure that only the `.hobbit` files are located within your new folder. Any other files might have unexpected results.

# Download

Download the most recent release [here](../../releases). Simply open the zip file and extract the full "HobbitSpeedrunTools" folder the location where you want to keep it. Then you can run the hobbit-save-manager.exe file inside the folder, and you're up and running.

Note that you will be prompted to install the .NET 5.0 runtime if you don't already have it.

# Warning

Although the HobbitSpeedrunTools save manager is written to back up your old saves and restore them after use, I can't guarantee that this will always function as expected. Make sure to back up important saves yourself.

# Acknowledgements
- [Erfg12's Memory.dll](https://github.com/erfg12/memory.dll/) used for reading and writing to the game memory
- [MD_Pi](https://www.youtube.com/user/MD0111000001101001) for sharing his cheat table filled with just about every memory address in the game
- [Tasz](https://www.twitch.tv/tasz) for providing save files for the 100% category