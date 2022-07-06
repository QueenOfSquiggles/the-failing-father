using System.Collections.Generic;
using System.Linq;
using Godot;

public class BehaviourTreePrintDebugState : BehaviourTreeAction {

    private const int FLAG_PRINT_TREE_ROOT_NAME = 1;
    private const int FLAG_PRINT_TREE_CHECKPOINT = 2;
    private const int FLAG_PRINT_BLACKBOARD_CONTENTS = 4;
    private const int FLAG_PRINT_FLAG_8 = 8;
    private const int FLAG_PRINT_FLAG_16 = 16;

    private const int FLAGS_ALL = 31;
    // hint keys : 1 | 2 | 4 | 8
    [Export] public string additionalMessage = "";
    [Export(PropertyHint.Flags, "Root Name,Checkpoint Name,Blackboard Contents")] public int flags = FLAGS_ALL;

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        string msg = "<BT DEBUG>";
        if (!additionalMessage.Empty()) msg += "\n\tMSG: " + additionalMessage;
        if (IsFlag(FLAG_PRINT_TREE_ROOT_NAME)) msg += $"\n\tROOT: '{ root.Name }'";
        if (IsFlag(FLAG_PRINT_TREE_CHECKPOINT)) msg += $"\n\tCHECKPOINT: '{ ((root.checkpoint == null)? "null" : root.checkpoint.Name )}'";
        if (IsFlag(FLAG_PRINT_BLACKBOARD_CONTENTS)){
            if(blackboard.Count == 0){
                msg += "\n\tBLACKBOARD: { empty }";
            }else{
                msg += "\n\tBLACKBOARD: {";
                foreach(string key in blackboard.Keys){
                    var value = blackboard[key];
                    string strValue = "null";
                    if (value != null) strValue = value.ToString();
                    msg += $"\n\t\t'{key}' : '{strValue}'";
                }
                msg += "\n\t}";
            }
        } 
        
        msg += "\n</BT DEBUG>";
        GD.Print(msg);
        return TickResult.SUCCESS;
    }

    private bool IsFlag(int flagBit){
        return (flags & flagBit) != 0;
    }

    
}