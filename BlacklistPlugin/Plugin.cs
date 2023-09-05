using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using BlacklistPlugin.Windows;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game;
using Dalamud.Logging;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Collections.Generic;

namespace BlacklistPlugin
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Blacklist Plugin";
        private const string CommandName = "/bl"; //xdd

        private DalamudPluginInterface PluginInterface { get; init; }
        private PartyListUpdater partyListUpdater { get; set; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("BlacklistPlugin");

        private ConfigWindow ConfigWindow { get; init; }
        private MainWindow MainWindow { get; init; }

        public Plugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
            pluginInterface.Create<Service>();

            //PluginLog.Verbose("Adding OnUpdate to Framework update");
            //Service.Framework.Update += OnUpdate;

            this.partyListUpdater = new PartyListUpdater(Service.PartyList);
            this.partyListUpdater.Enable();

            // TODO: handle the Event when the list is already initialized and a new player is blacklisted. Either reinit the list or manually add it afterwards.
            getAllBlacklistedPlayers();




            // SAMPLE CODE, REMOVE WHEN NOT NEEDED ANYMORE
            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(partyListUpdater, Service.ToastGui);

            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);

            Service.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public unsafe List<Player> getAllBlacklistedPlayers()
        {
            var data = (BlackListPlayerData*)AtkStage.GetSingleton()->AtkArrayDataHolder->StringArrays[14]->StringArray;

            List<Player> players = new();
            // 400 is the size of the (String)AtkArrayData from Dalamud. The indeces correspond to: 0-199 Name; 200-399 World.
            // 0 and 200 are the Name and World corresponding to the same player.
            for (var i = 0; i < 400; i++)
            {
                if (data[i].pointerToValue != null)
                {
                    PluginLog.Verbose($"Index: {i}");
                    if (i < 200)
                    {
                        players.Add(new Player { Name = data[i].Value });
                    }
                    else
                    {
                        players[i - 200].World = data[i].Value;
                    }
                }
            }

            return players;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();
            MainWindow.Dispose();
            this.partyListUpdater.Dispose();

            Service.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            PluginLog.Verbose("opening MainWindow");
            MainWindow.IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
