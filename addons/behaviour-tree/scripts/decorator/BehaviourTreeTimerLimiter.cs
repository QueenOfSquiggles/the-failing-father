using System;
using System.Collections.Generic;
using Godot;

public class BehaviourTreeTimerLimiter : BehaviourTreeDecorator {

    [Export] public TickResult interimResult = TickResult.FAILURE;
    [Export] public float timeInterval = 1.0f;
    private float timer = 0f;

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime) {
        timer += deltaTime;
        if (timer > timeInterval){
            timer = 0f;
            return GetChild<BehaviourTreeNode>(0).Tick(root, blackboard, deltaTime);
        }
        return interimResult;
    }
}
