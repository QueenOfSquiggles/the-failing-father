using Godot;
using System;

[Tool]
public class CommandSlot : HBoxContainer
{
    [Signal] public delegate void ButtonCallCommandPressed();
    [Signal] public delegate void ButtonMoveUpPressed();
    [Signal] public delegate void ButtonMoveDownPressed();
    [Signal] public delegate void ButtonDeletePressed();

    private Button btnCall;
    private Button btnMoveUp;
    private Button btnMoveDown;
    private Button btnDelete;

    public override void _Ready() {
        btnCall = GetNode<Button>("BtnRunCommand");
        btnMoveUp = GetNode<Button>("BtnMoveUp");
        btnMoveDown = GetNode<Button>("BtnMoveDown");
        btnDelete = GetNode<Button>("BtnDelete");
    }

    public void TriggerEvent_CallCommand(){
        EmitSignal(nameof(ButtonCallCommandPressed));
    }
    public void TriggerEvent_MoveUp(){
        EmitSignal(nameof(ButtonMoveUpPressed));
    }
    public void TriggerEvent_MoveDown(){
        EmitSignal(nameof(ButtonMoveDownPressed));
    }
    public void TriggerEvent_Delete(){
        EmitSignal(nameof(ButtonDeletePressed));
    }


    public void SetupProperties(CommandPalettePanel.Command cmd, bool canMoveUp, bool canMoveDown, Texture texture) {
        btnCall.Text = cmd.name;
        btnCall.HintTooltip = cmd.description;
        if (!canMoveUp){
            // first in list
            btnMoveUp.QueueFree();
            btnMoveUp = null;
        }
        if (!canMoveDown){
            // last in list
            btnMoveDown.QueueFree();
            btnMoveDown = null;
        }
        btnCall.Connect("pressed", this, "TriggerEvent_CallCommand");
        btnMoveUp?.Connect("pressed", this, "TriggerEvent_MoveUp");
        btnMoveDown?.Connect("pressed", this, "TriggerEvent_MoveDown");
        btnDelete.Connect("pressed", this, "TriggerEvent_Delete");
        if (texture != null) btnCall.Icon = texture;
    }
}
