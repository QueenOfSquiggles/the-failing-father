using System;
using System.Collections.Generic;
using Godot;

public class BehaviourTreeLimiter : BehaviourTreeDecorator {

    [Export] public TickResult interimResult = TickResult.FAILURE;
    [Export] public int tickInterval = 5;
    private int timer = 0;

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime) {
        if (timer++ > tickInterval){
            timer = 0;
            return GetChild<BehaviourTreeNode>(0).Tick(root, blackboard, deltaTime);
        }
        return interimResult;
    }
}
