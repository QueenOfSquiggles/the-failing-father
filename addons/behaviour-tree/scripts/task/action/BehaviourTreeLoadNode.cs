using System.Collections.Generic;
using Godot;

public class BehaviourTreeLoadNode : BehaviourTreeAction {

    [Export] public NodePath nodePath;
    [Export] public string saveName = "";

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        var node = GetNode(nodePath);
        if (node == null) return TickResult.FAILURE;
        blackboard[saveName] = node;
        return TickResult.SUCCESS;
    }
}
