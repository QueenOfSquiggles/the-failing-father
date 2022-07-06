using Godot;
using System;
using System.Collections.Generic;

public class BehaviourTreeNode : Node {
    public enum TickResult {
        SUCCESS, FAILURE, RUNNING
    }

    private readonly int maxNumChildren;
    private readonly int minNumChildren;
    public BehaviourTreeNode(int maxChildren, int minChildren = -1){
        maxNumChildren = maxChildren;
        minNumChildren = minChildren;
    }
    public override void _Ready() {
        if(maxNumChildren != -1 && GetChildCount() > maxNumChildren){
            GD.PushError($"Invalid Behaviour Tree Node! Too many children: { GetChildCount() }! Max allowed on {Name} are {maxNumChildren} children");
        }
        if(GetChildCount() < minNumChildren){
            GD.PushError($"Invalid Behaviour Tree Node! Too few children: { GetChildCount() }! Min required on {Name} are {maxNumChildren} children");
        }
    }

    /// <summary>
    /// The main processing of the BehaviourTree. Each node will handle its unique function.
    /// </summary>
    public virtual TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        throw new NotImplementedException("This is an effectively abstract class! Do not use it!");
    }

    protected Node[] GetChildrenAsArray(){
        var children = GetChildren();
        var arr = new Node[children.Count];
        children.CopyTo(arr, 0);
        return arr;
    }

}
