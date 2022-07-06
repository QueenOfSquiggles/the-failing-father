using System;
using System.Collections.Generic;

public class BehaviourTreeInverter : BehaviourTreeDecorator {
    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime) {
        var result = GetChild<BehaviourTreeNode>(0).Tick(root, blackboard, deltaTime);
        if (result == TickResult.SUCCESS){
            return TickResult.FAILURE;
        } else if (result == TickResult.FAILURE){
            return TickResult.SUCCESS;
        } else {
            return result;
        }
    }
}
