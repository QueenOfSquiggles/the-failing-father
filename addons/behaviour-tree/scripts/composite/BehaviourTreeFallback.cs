using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class BehaviourTreeFallback : BehaviourTreeCompositor {

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        return ProcessChildren(root, blackboard, deltaTime, GetChildrenAsArray());
    }

    protected override TickResult ProcessChildren(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime, Node[] children) {
        foreach (Node node in children){
            var treeNode = node as BehaviourTreeNode;
            var result = treeNode.Tick(root, blackboard, deltaTime);
            if (result != TickResult.FAILURE) return result;
        }
        return TickResult.FAILURE;

    }
}