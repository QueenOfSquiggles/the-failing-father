using System;
using Godot;
using System.Collections.Generic;

public class BehaviourTreeAnimTreeParamFloat : BehaviourTreeAction {

    [Export] public string animTreeName = "animation_tree";
    [Export] public string paramName = "anim";
    [Export] public float paramValue = 0.0f;
    
     public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        var anim = blackboard[animTreeName] as AnimationTree;
        anim.Set(paramName, paramValue);
        return TickResult.SUCCESS;
    }
}