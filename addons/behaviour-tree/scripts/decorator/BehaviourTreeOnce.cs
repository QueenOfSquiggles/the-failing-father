using System;
using System.Collections.Generic;
using Godot;

public class BehaviourTreeOnce : BehaviourTreeDecorator {

    private bool hasRun = false;

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime) {
        if (hasRun) return TickResult.SUCCESS;
        hasRun = true;
        return GetChild<BehaviourTreeNode>(0).Tick(root, blackboard, deltaTime);
    }
}
