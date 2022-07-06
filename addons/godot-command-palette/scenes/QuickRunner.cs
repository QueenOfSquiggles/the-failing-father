using Godot;
using System;
using System.Data;

[Tool]
public class QuickRunner : WindowDialog
{
    [Export(PropertyHint.Range, "0.0,1.0")]
    public float minSimilarity = 0.5f;

    private Tree commandsTree;
    private LineEdit searchLine;

    private string lastSearch = "";

    private const string SETTING_TOLERANCE = "quick_runner/search_term_tolerance";

    public override void _Ready() {
        commandsTree = GetNode<Tree>("MarginContainer/VBoxContainer/ScrollContainer/Tree");
        searchLine = GetNode<LineEdit>("MarginContainer/VBoxContainer/Search");
        searchLine.Connect("text_changed", this, "UpdateSearchTerm");
        commandsTree.Connect("button_pressed", this, "OnButtonPressed");
        if (!ProjectSettings.HasSetting(SETTING_TOLERANCE)){
            ProjectSettings.SetSetting(SETTING_TOLERANCE, minSimilarity);
            ProjectSettings.SetInitialValue(SETTING_TOLERANCE, minSimilarity);
        }
    }

    public void UpdateSearchTerm(string n_text){
        lastSearch = n_text;
        if (Visible){
            RebuildTree(true);
        }
    }

    public void UseQuickRunner(){
        searchLine.Text = lastSearch;
        float temp = 0.0f;
        if(float.TryParse(ProjectSettings.GetSetting(SETTING_TOLERANCE).ToString(), out temp)) minSimilarity = temp;
        RebuildTree(true);
        PopupCenteredRatio(0.4f);
        searchLine.GrabFocus();
    }

    public void OnButtonPressed(TreeItem item, int column, int id){
        // the id should be the index of the command in the registry
        Hide();
        CommandPalettePanel.instance.RunCommand(id);
    }

    private void RebuildTree(bool limitBySearch = false){
        commandsTree.Clear();
        commandsTree.CreateItem(); // create root
        var searchTerm = searchLine.Text;
        lastSearch = searchLine.Text;
        var commandArray = CommandPalettePanel.instance.registeredCommands.ToArray();
        Array.Sort(commandArray, (CommandPalettePanel.Command cmd1,CommandPalettePanel.Command cmd2) => {
            return cmd1.name.CasecmpTo(cmd2.name); // should sort alphabetically, meaning searches are closest matching?
        });

        TreeItem[] headers = new TreeItem[3];
        for(int i = 0; i < commandArray.Length; i++){
            var cmd = commandArray[i];
            if(limitBySearch && !searchTerm.Empty()){
                var similarity = StringSimilarity.PercentageSimilar(searchTerm.ToLowerInvariant(), cmd.name.ToLowerInvariant());
                if (similarity < minSimilarity){
                    continue; // skip this command
                } 
            }
            TreeItem owner = null;
            CreateTreeHeader(cmd.type, headers, out owner);
            var cmdItem = commandsTree.CreateItem(owner);
            //cmdItem.SetIcon(0, owner.GetIcon(0));
            cmdItem.AddButton(0, owner.GetIcon(0), i, false, cmd.description);
            cmdItem.SetText(0, cmd.name);
        }
    }

    private void CreateTreeHeader(CommandPalettePanel.CommandType type, TreeItem[] existingOwners, out TreeItem owner){
        owner = existingOwners[(int)type];
        if (owner == null){
            owner = commandsTree.CreateItem();
            existingOwners[(int)type] = owner;
            owner.SetText(0, type.ToString());
            var texture = CommandPalettePanel.instance.textureEditorScript;
            switch(type){
                case CommandPalettePanel.CommandType.EDITOR_SCRIPT:
                    texture = CommandPalettePanel.instance.textureEditorScript;
                    break;
                case CommandPalettePanel.CommandType.TOOL_SCRIPT_COMMAND:
                    texture = CommandPalettePanel.instance.textureToolScript;
                    break;
                case CommandPalettePanel.CommandType.MACRO:
                    texture = CommandPalettePanel.instance.textureMacro;
                    break;
            }
            owner.SetIcon(0, texture);
        }
    }

    public override void _Input(InputEvent inputEvent) {
        if (Visible && inputEvent.IsActionPressed("ui_accept")){
            var item = commandsTree.GetSelected();
            if(item == null){
                //  Root -> Header -> Topmost Command (search delimited)
                item = commandsTree.GetRoot().GetChildren().GetChildren();
            }
            var id = item.GetButtonId(0, 0);
            CommandPalettePanel.instance.RunCommand(id);
            this.Hide();
        }
    }

}
