using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.UI.Info;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;
using ImGuiScene;

namespace BlacklistPlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private PartyListUpdater partyListUpdater;
    private ToastGui toastGui;
    public MainWindow(PartyListUpdater partyListUpdater, ToastGui toastGui) : base(
        "My Amazing Window", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.partyListUpdater = partyListUpdater;
        this.toastGui = toastGui;
    }

    public void Dispose() {}

    public override void Draw()
    {
        //ImGui.Text($"The random config bool is {this.Plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");

        if (ImGui.Button("Enable"))
        {

            this.toastGui.ShowQuest(new SeString());
        }

        //if (ImGui.Button("Disable"))
        //{
        //    this.partyListUpdater.Disable();
        //}

        //ImGui.Spacing();

        //ImGui.Text("Have a goat:");
        //ImGui.Indent(55);
        //ImGui.Image(this.GoatImage.ImGuiHandle, new Vector2(this.GoatImage.Width, this.GoatImage.Height));
        //ImGui.Unindent(55);
    }
}
