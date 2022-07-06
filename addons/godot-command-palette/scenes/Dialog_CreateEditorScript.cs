using Godot;
using System;

[Tool]
public class Dialog_CreateEditorScript : WindowDialog {
    private CommandPalettePanel cmdPalette;
    private string filePath;

    private Label lblFile;
    private LineEdit lineName;
    private LineEdit lineDescription;
    private Button btnAccept;
    private Button btnCancel;

    public override void _Ready() {
        lblFile = GetNode<Label>("MarginContainer/VBoxContainer/LabelFileRef");
        lineName = GetNode<LineEdit>("MarginContainer/VBoxContainer/HBoxContainer/LineName");
        lineDescription = GetNode<LineEdit>("MarginContainer/VBoxContainer/HBoxContainer2/LineDescription");
        btnAccept = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer4/BtnAccept");
        btnCancel = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer4/BtnCancel");
        btnAccept.Connect("pressed", this, "OnBtnAccept");
        btnCancel.Connect("pressed", this, "OnBtnCancel");
    }

    public void StartCreate(string file ,CommandPalettePanel cmdPalette){
        this.cmdPalette = cmdPalette;
        filePath = file;
        lblFile.Text = "File: " + file;
        lineName.Text = GetDefaultName();
        lineDescription.Text = GetDefaultDescription();
        btnAccept.GrabFocus();
        GD.Print("Opening CreateEditorScript popup");
        PopupCentered();
    }

    private void OnBtnAccept(){
        if(cmdPalette == null) return;
        var name = lineName.Text;
        var description = lineDescription.Text;            
        cmdPalette.RegisterCommand(name, description, filePath, CommandPalettePanel.CommandType.EDITOR_SCRIPT);
        OnBtnCancel();
    }

    private string GetDefaultName(){
        var parts = filePath.Split('/');
        return parts[parts.Length-1];
    }

    private string GetDefaultDescription(){
        return "Run editor script " + GetDefaultName();
    }

    private void OnBtnCancel(){
        this.Hide();
        cmdPalette = null;
    }


}
