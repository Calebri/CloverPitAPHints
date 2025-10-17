# CloverPit AP Hints
A BepInEx plugin designed to implement CloverPit as a hint game for the Archipelago Multiworld.

I originally wanted to make a full APWorld for this game, but I realized that I didn't know how to use BepInEx or mod games in general, so this is the best I can do right now.

## How does it work?
Doing certain objectives sends a hint to the multiworld. A hint will inform you of the item that is in an unchecked location on the slot that the plugin is connected to. Right now, you can get hints by completing deadlines. By default, the first hint is sent when you reach deadline 3, but this is configurable. In the future, I plan to add custom single-use charms which you can buy to get hints.

## Usage
1. Download and install BepInEx.
2. Download and install the CloverPit AP Hints plugin. Make sure the `.dll` files are in a folder within `CloverPit/BepInEx/plugins`.
3. Run the game once to generate the config file.
4. The config file is accessible in `CloverPit/BepInEx/config/github.calebri.cloverpitaphints.cfg`. There you can set the IP, port, and password of the Archipelago server you want to connect to, as well as the name of the slot you want to hint for. You can also configure the first deadline that will send a hint.
5. Running the game after configuring will automatically attempt to connect to the server specified in the config file. There is currrently no in-game way of telling if you are connected. The Archipelago server will log a confirmation message stating that you have connected as a hint game.
6. Playing the game as normal will issue hints when reaching a deadline, starting at deadline 3 by default.

## Planned Features
- Custom Charms for more hint opportunities
- In-game configuration menu

### Speculative Features (Don't count on it.)
- In-game connection indicator
- In-game connection GUI

## Building From Source

### Instructions

1. Download [Archipelago.MultiClient.Net](https://www.nuget.org/packages/Archipelago.MultiClient.Net).
1. Clone this repo.
1. Run `dotnet build` in the project folder.
1. Move `CloverPitAPHints.dll` from `/bin/Debug/netstandard2.1` to a new folder in `CloverPit/BepInEx/plugins`.
1. Add `Archipelago.MultiClient.Net.dll` and `Newtonsoft.Json.dll` to the new plugin folder. These libaries are required for the plugin to run properly and can be found in `C:\Users\<user>\.nuget\packages\archipelago.multiclient.net\<version>\lib\netstandard2.0` on Windows.