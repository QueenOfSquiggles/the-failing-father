using System;
using System.Collections.Generic;
using Godot;

public class BehaviourTreeLogic_XOR : BehaviourTreeCompositor {

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        return ProcessChildren(root, blackboard, deltaTime, GetChildrenAsArray());
    }

    protected override TickResult ProcessChildren(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime, Node[] children){
        int countSucceeded = 0;
        foreach(Node c in children){
            var btNode = c as BehaviourTreeNode; // assume true
            var result = btNode.Tick(root, blackboard, deltaTime);
            if (result == TickResult.RUNNING) return result; // RUNNING
            if (result == TickResult.SUCCESS) countSucceeded++;
        }
        // XOR means if only one child succeeded, succeed; otherwise, fail
        return (countSucceeded == 1) ? TickResult.SUCCESS : TickResult.FAILURE;
    }
    
}