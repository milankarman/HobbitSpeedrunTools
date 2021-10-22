<br />
<p align="center">
  <img src="hobbit-save-manager/img/icon.png" alt="Logo" width="60" height="60">

  <h2 align="center">Hobbit Save Manager</h2>

  <p align="center">
    <i>A tool for quickly swapping practice saves for speedrunners of The Hobbit</i>
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

# About

This is a tool made to quickly swap saves in the 2003 PC game "The Hobbit". This is beneficial to speedrunners as they manage a lot of saves for practice and swapping between them is time consuming and tedious.

<div align="center">
  <img src="assets/preview.png" alt="Triangulation view" width="auto" height="auto">
</div>
<br />

# Features

- Allows super fast swapping of saves
- Only keeps 1 save at all times, always making it the first option under "Load Game"
- Easily expandable with your own saves
- Backs up existing saves on startup and restores them on close

# Usage

## General usage

Start the program, this will automatically back up and clear your saves folder. From there you can select a save collection that suits your needs. Then all that's left is to pick a save, this will then automatically become the only save in your game.

## Adding saves

To add your own saves to the save manager follow the following steps:

1. Navigate to your `hobbit-save-manager` installation and into `save-collections`
2. Create a new folder starting with the following format: `[Number]. [Collection Name]`  
*The number should be the position that you want the collection to appear in, a lower number will appear closer to the top, but make sure every number is only used once.*
3. Move your saves into your newly created folder with the following format: `[Number]. [Save Name].hobbit`  
*The number should be the position that you want the save to appear in, a lower number will appear closer to the top, but make sure every number is only used once.*

**Note**: Make sure that only the `.hobbit` files are located within your new folder. Any other files might have unexpected results.

# Download

Download the most recent release [here](../../releases). Simply open the zip file and extract the full "hobbit-save-manager" folder the location where you want to keep it. Then you can run the hobbit-save-manager.exe file inside the folder, and you're up and running!

(You might be prompted to install the .NET 6.0 runtime if you don't already have it)

# Warning

Although Hobbit Save Manager is written to back up your old saves and restore them after use, I can't guarantee that this will always function as expected. Make sure to back up important saves yourself.
