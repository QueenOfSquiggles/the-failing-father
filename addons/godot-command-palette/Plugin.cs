using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class Plugin : EditorPlugin {
    
    const string SCENE_PALETTE_SCENE = "res://addons/godot-command-palette/scenes/CommandPalettePanel.tscn";

    private Control commandPalettePanel = null;

    public override void _EnterTree() {
        var scene = GD.Load<PackedScene>(SCENE_PALETTE_SCENE);
        commandPalettePanel = scene.Instance<Control>();
        this.AddControlToDock(DockSlot.RightUl, commandPalettePanel);
    }

    public override void _ExitTree() {
        if (!IsInstanceValid(commandPalettePanel)) return;
        this.RemoveControlFromDocks(commandPalettePanel);
        commandPalettePanel.QueueFree();
    }

    // public override void _Input(InputEvent inputEvent) {
    //     if (inputEvent is InputEventKey && inputEvent.IsPressed()){
    //         var keyEvent = inputEvent as InputEventKey;
    //         if (keyEvent.Control && keyEvent.Shift){
    //             if (keyEvent.Scancode == (uint)(int)KeyList.P){
    //                 GD.Print("Open quick command palatte!");
    //             }
    //         }
    //     }
    // }

}
