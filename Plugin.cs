using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using BepInEx.Configuration;
using System.Collections.ObjectModel;

namespace CloverPitAPHints;

[BepInPlugin("github.calebri.cloverpitaphints", "CloverPitAPHints", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private GameObject dm = null;
    private TextMeshProUGUI tmpro = null;

    private float timer = 0;
    private int deadnumOld = 0;

    private ConfigEntry<int> hintThresh;

    // AP Variables
    private ConfigEntry<string> ip;
    private ConfigEntry<int> port;
    private ConfigEntry<string> username;
    private ConfigEntry<string> pass;
    private ArchipelagoSession session;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        SceneManager.sceneLoaded += OnSceneLoaded;

        // Config Initialization
        hintThresh = Config.Bind<int>("Settings", "hintThresh", 3, "The first deadline that will give you a hint upon reaching it.");

        ip = Config.Bind<string>("Archipelago", "ip", "localhost", "Server IP");
        port = Config.Bind<int>("Archipelago", "port", 38281, "Server port.");
        username = Config.Bind<string>("Archipelago", "name", "Player1", "Slot name.");
        pass = Config.Bind<string>("Archipelago", "password", "", "Server password.");

        session = ArchipelagoSessionFactory.CreateSession(ip.Value, port.Value);

        // Server Connection
        Logger.LogInfo("Attempting to connect to AP server...");

        APConnect(username.Value, pass.Value);
    }

    private void APConnect(string name, string pass)
    {
        LoginResult result;

        try
        {
            result = session.TryConnectAndLogin("", name, ItemsHandlingFlags.NoItems, null, ["HintGame"]); // Consider adding tags either TextOnly or AP (possibly HintGame) to bypass game name requirement
        }
        catch (Exception e)
        {
            result = new LoginFailure(e.GetBaseException().Message);
        }

        if (!result.Successful)
        {
            Logger.LogError("Failed to connect to Archipelago server. Make sure config is valid and restart the game.");
            return;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode lsm) // Recording exsistance / absence of TMP UI elements in scene
    {
        if (scene.name == "03GameScene") // 03GameScene is the room scene
        {
            Logger.LogInfo("🎲 Game scene loaded.");
            dm = GameObject.Find("Deadline Monitor");
            if (dm != null) // Record TMPUGUI component for use elsewhere
            {
                Logger.LogInfo("🎲 TMProUGUI component found in game scene.");
                tmpro = dm.GetComponentInChildren<TextMeshProUGUI>();
            }
            else // Record absence of dm and tmpro
            {
                tmpro = null;
            }
        }
        else
        {
            dm = null;
            tmpro = null;
        }
    }

    private void Update() // Runs every frame
    {
        timer += Time.deltaTime;

        if (timer >= 1)
        { // 1 sec timer to avoid log spam
            timer = 0;
            if (tmpro != null)
            {
                int deadnum = GetDeadline(); // Parse deadline monitor text
                // Logger.LogInfo($"🎲 Current deadline: {deadnum}");

                if (deadnum > deadnumOld && deadnumOld != 0 && deadnum >= hintThresh.Value)
                {
                    Logger.LogInfo($"🎲 Deadline increased to {deadnum}");

                    SendHint();
                }

                deadnumOld = deadnum;
            }
        }
    }

    private int GetDeadline()
    {
        if (tmpro == null)
        {
            return 0;
        }
        else
        {
            return int.Parse(tmpro.text.Split(' ')[1].Substring(1));
        }
    }
    
    private void SendHint()
    {
        ReadOnlyCollection<long> missing = session.Locations.AllMissingLocations;
        var random = new System.Random();
        long item = missing[random.Next(missing.Count)];

        try
        {
            session.Locations.ScoutLocationsAsync(HintCreationPolicy.CreateAndAnnounce, item);
            Logger.LogInfo($"Sent hint for AP item with ID: {item}");
        }
        catch
        {
            Logger.LogError("Hint failed to send due to the Archipelago websocket being closed.");
        }
    }
}
