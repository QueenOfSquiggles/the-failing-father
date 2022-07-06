using System;
using System.Collections.Generic;
using Godot;

public class BehaviourTreeLogic_AND : BehaviourTreeCompositor {

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        return ProcessChildren(root, blackboard, deltaTime, GetChildrenAsArray());
    }

    protected override TickResult ProcessChildren(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime, Node[] children){
        foreach(Node c in children){
            var btNode = c as BehaviourTreeNode; // assume true
            var result = btNode.Tick(root, blackboard, deltaTime);
            if (result != TickResult.SUCCESS) return result; // FAILURE or RUNNING
        }
        return TickResult.SUCCESS;
    }
    
}