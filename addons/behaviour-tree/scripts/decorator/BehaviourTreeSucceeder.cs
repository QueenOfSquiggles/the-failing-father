using System;
using Godot;
using System.Collections.Generic;

public class BehaviourTreeSucceeder : BehaviourTreeDecorator {

    [Export] public TickResult forceResult = TickResult.SUCCESS;
    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime) {
        GetChild<BehaviourTreeNode>(0).Tick(root, blackboard, deltaTime);
        return forceResult;
    }
}
