using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

[Tool]
public class CommandPalettePanel : Control
{
    public enum CommandType{
        EDITOR_SCRIPT,
        TOOL_SCRIPT_COMMAND,
        MACRO
    }
    public struct Command {
        public string name;
        public string description;
        public string runnable;
        public CommandType type;
    }

    private const string SAVE_DATA_PATH = "res://addons/godot-command-palette/scenes/data/commands.json";

    public List<Command> registeredCommands = new List<Command>();
    public static CommandPalettePanel instance = null;

    /*
    |
    |   Separation
    |
    |
    */

    [Export] public Texture textureEditorScript;
    [Export] public Texture textureToolScript;
    [Export] public Texture textureMacro;

    
    private Control commandsListRoot;
    private PackedScene commandSlotScene;
    private Button btnRegisterNew;
    private Button btnSave;
    private PopupMenu registerMenu;
    private FileDialog fileDialog;
    private WindowDialog registerPopupEditorScript;
    private QuickRunner quickRunner;
    private string dataPathPrefix = "./addons/godot-command-palette/scenes/data/";

    public override void _Ready() {
        // basically a singleton.
        if (instance == null) instance = this;
        else QueueFree();

        // handle setup
        btnRegisterNew = GetNode<Button>("PanelContainer/VBoxContainer/PanelContainer/HBoxContainer/BtnRegisterNew");
        btnRegisterNew.Connect("pressed", this, "OnButtonRegisterPressed");
        btnSave = GetNode<Button>("PanelContainer/VBoxContainer/PanelContainer/HBoxContainer/BtnSave");
        btnSave.Connect("pressed", this, "SaveData");

        quickRunner = GetNode<WindowDialog>("QuickRunner") as QuickRunner;

        commandSlotScene = GD.Load<PackedScene>("res://addons/godot-command-palette/scenes/CommandSlot.tscn");
        commandsListRoot = GetNode<Control>("%GroupCommands");

        // popups
        registerMenu = GetNode<PopupMenu>("RegisterCommandMenu");
        while(registerMenu.Items.Count > 0) registerMenu.RemoveItem(0); // force clear items. Just in case we get overlap. should never occur unless you are editing this plugin
        registerMenu.AddIconItem(textureEditorScript, "Editor Script Command", (int)CommandType.EDITOR_SCRIPT);
        registerMenu.AddIconItem(textureToolScript, "Tool Script Command", (int)CommandType.EDITOR_SCRIPT);
        registerMenu.AddIconItem(textureMacro, "Macro Command", (int)CommandType.EDITOR_SCRIPT);
        registerMenu.Connect("index_pressed", this, "OnMenuItemSelected");

        fileDialog = GetNode<FileDialog>("FileDialog");
        registerPopupEditorScript = GetNode<WindowDialog>("Dialog_CreateEditorScript");
        if (registerPopupEditorScript == null){
            GD.PrintErr("PopupEditorScript was found null!");
        }

        // Prep systems
        LoadData();
        RebuildList();
    }

    public void OnButtonRegisterPressed(){
        var rect = registerMenu.GetRect();
        rect.Position = btnRegisterNew.RectGlobalPosition;
        registerMenu.Popup_(rect);
    }

    public void OnMenuItemSelected(int index){
        switch((CommandType)index){
            case CommandType.EDITOR_SCRIPT:
                fileDialog.Connect("popup_hide", this, "RegisterEditorCommand", new Godot.Collections.Array(){}, (uint)((int)ConnectFlags.Oneshot));
                fileDialog.PopupCenteredRatio(0.75f);
                GD.Print("File dialog opened");
                return;
        }
        GD.PushError("Command Type '" + ((CommandType)index).ToString() + "' is not currently supported");
    }

    private void RegisterEditorCommand() {
        var filePath = fileDialog.CurrentPath;
        GD.Print("file selected: " + filePath);
        var dialog = registerPopupEditorScript as Dialog_CreateEditorScript;
        if (dialog != null){
            dialog.StartCreate(filePath, this);
        }else{
            GD.PrintErr("Failed to cast registerPopupEditorScript to Dialog_CreateEditorScript");
        }
    }


    public override void _ExitTree() {
        SaveData();
        instance = null;
    }

    public void RegisterCommand(string m_name, string m_description, string m_runnable, CommandType m_type){
        registeredCommands.Add(new Command(){
            // creates a new Command and adds to the end of the list.
            name = m_name,
            description = m_description,
            runnable = m_runnable,
            type = m_type
        });
        CallDeferred("RebuildList");
    }

    public static void RegisterCommandExternal(string name, string description, string runnable, CommandType m_type){
        instance?.RegisterCommand(name, description, runnable, m_type);
    }

    private void RebuildList(){
        foreach( Node child in commandsListRoot.GetChildren()){
            child.QueueFree();
        }
        for (int i = 0; i < registeredCommands.Count;i++){
            var cmdSlot = commandSlotScene.Instance<CommandSlot>();
            commandsListRoot.AddChild(cmdSlot);
            cmdSlot.Owner = commandsListRoot;
            var paramz = new Godot.Collections.Array();
            var cmd = registeredCommands[i];
            paramz.Add(i);
            cmdSlot.Connect("ButtonCallCommandPressed", this, "RunCommand", paramz);
            cmdSlot.Connect("ButtonMoveUpPressed", this, "MoveCommandUp", paramz);
            cmdSlot.Connect("ButtonMoveDownPressed", this, "MoveCommandDown", paramz);
            cmdSlot.Connect("ButtonDeletePressed", this, "DeleteCommand", paramz);
            cmdSlot.SetupProperties(cmd, (i!=0), (i!=registeredCommands.Count-1), GetTextureForCommand(cmd.type));
        }
        SaveData();
    }

    private Texture GetTextureForCommand(CommandType type){
        switch(type){
            case CommandType.EDITOR_SCRIPT: return textureEditorScript;
            case CommandType.TOOL_SCRIPT_COMMAND: return textureToolScript;
            case CommandType.MACRO: return textureMacro;
        }
        return null;// shouldn't ever happen
    }

    public void MoveCommandUp(int index){
        var cmd = registeredCommands[index];
        if (index == 0) return;
        var temp = registeredCommands[index - 1];
        registeredCommands[index] = temp;
        registeredCommands[index-1] = cmd;
        RebuildList();
    }
    public void MoveCommandDown(int index){
        var cmd = registeredCommands[index];
        if (index == GetChildCount()-1) return;
        var temp = registeredCommands[index + 1];
        registeredCommands[index] = temp;
        registeredCommands[index+1] = cmd;
        RebuildList();
    }
    public void DeleteCommand(int index){
        registeredCommands.RemoveAt(index);
        RebuildList();
    }

    public void RunCommand(int index){
        var cmd = registeredCommands[index];
        bool processed = false;
        switch(cmd.type){
            case CommandType.EDITOR_SCRIPT:
               RunCommandEditorScript(cmd);
               processed = true;
               break;               
        }
        if (!processed) GD.PushError("Command Type " + cmd.type + " is currently unsupported");
    }

    private void RunCommandEditorScript(Command cmd){
        GD.Print("Running Editor Script : " + cmd.name);
        var script = GD.Load<Script>(cmd.runnable);
        if (!script.IsTool()){
            GD.PushError("Editor script is not a tool script! Please fix this in your code!");
            return;
        }
        
        if(script.CanInstance()){
            var editorScript = GD.InstanceFromId(script.GetInstanceId());
            var inst = editorScript.Call("new") as EditorScript;
            // I am assuming that this will cover all bases.
            // May not work in cases of other language bindings?
            if (inst.HasMethod("_Run")){
                inst.Call("_Run"); // should cover C# cases
            } else if (inst.HasMethod("_run")){
                inst.Call("_run"); // should cover GDScript cases
            }
        }        
    }

    private void DebugListMethods(Godot.Object obj){
        var methodNames = new List<string>();
        var methodDicts = obj.GetMethodList();
        GD.Print("Searching for a 'run' method...");
        foreach(Dictionary methodData in methodDicts){
            if(methodData.Contains("name")){
                var name = methodData["name"] as string;
                if(name.ToLower().Contains("run")){
                    GD.Print("Found run method! :'" + name + "'\n\t", methodData);
                }
                methodNames.Add(name);
            }
        }
        GD.Print("Methods Found: ", new Godot.Collections.Array(methodNames));
    }

    private void LoadData(){
        GD.Print("Loading command palette");
        var serializer = new XmlSerializer(typeof(Command[]));
        var reader = new StreamReader(dataPathPrefix + "metadata.xml");
        var loadedCommands = serializer.Deserialize(reader) as Command[];
        registeredCommands.AddRange(loadedCommands);
        reader.Close();
    }

    private void SaveData(){
        GD.Print("Saving command palette");
        var serializer = new XmlSerializer(typeof(Command[]));
        var writer = new StreamWriter(dataPathPrefix + "metadata.xml");
        serializer.Serialize(writer, registeredCommands.ToArray());
        writer.Close();
    }

    public override void _Input(InputEvent inputEvent) {
        if (inputEvent is InputEventKey && inputEvent.IsPressed()){
            var keyEvent = inputEvent as InputEventKey;
            if (keyEvent.Control && keyEvent.Shift){
                // CTRL+SHIFT+P -> Command Palette
                if (keyEvent.Scancode == (uint)(int)KeyList.P) quickRunner.UseQuickRunner();
            }
        }
    }

}
