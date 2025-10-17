# CloverPit AP Hints
A BepInEx plugin designed to implement CloverPit as a hint game for the Archipelago Multiworld.

## How does it work?
Doing certain objectives sends a hint to the multiworld. A hint will inform you of the item that is in an unchecked location on the slot that the plugin is connected to. Right now, you can get hints by completing deadlines. By default, the first hint is sent when you reach deadline 3, but this is configurable. In the future, I plan to add custom single-use charms which you can buy to get hints.

## Usage
1. Download and install BepInEx.
2. Download and install the CloverPit AP Hints plugin. Make sure the `.dll` files are in a folder within `CloverPit/BepInEx/plugins`.
3. Run the game once to generate the config file.
4. The config file is accessible in `CloverPit/BepInEx/config/CloverPitAPHints.cfg`. There you can set the IP, port, and password of the Archipelago server you want to connect to, as well as the name of the slot you want to hint for. You can also configure the first deadline that will send a hint.
5. Running the game after configuring will automatically attempt to connect to the server specified in the config file. There is currrently no in-game way of telling if you are connected. The Archipelago server will log a confirmation message stating that you have connected as a hint game.
6. Playing the game as normal will issue hints when reaching a deadline, starting at deadline 3 by default.