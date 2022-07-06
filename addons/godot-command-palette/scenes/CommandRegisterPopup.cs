using Godot;
using System;

public class CommandRegisterPopup : WindowDialog
{
    private LineEdit lineName;
    private LineEdit lineDescription;
    private LineEdit lineRunnable;
    private Button btnAccept;
    private Button btnCancel;

    public override void _Ready() {
        lineName = GetNode<LineEdit>("MarginContainer/VBoxContainer/HBoxContainer/LineName");
        lineDescription = GetNode<LineEdit>("MarginContainer/VBoxContainer/HBoxContainer2/LineDescription");
        lineRunnable = GetNode<LineEdit>("MarginContainer/VBoxContainer/HBoxContainer3/LineRunnable");
        btnAccept = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer4/BtnAccept");
        btnCancel = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer4/BtnCancel");

        btnAccept.Connect("pressed", this, "RegisterCommand");
    }

    private void RegisterCommand(){
        
    }

}
