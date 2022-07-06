using System;
using Godot;
using System.Collections.Generic;

public class BehaviourTreeAnimTreeStateMachineTravel : BehaviourTreeAction {

    [Export] public string animTreeName = "animation_tree";
    [Export] public string animTreeStateMachineName = "parameters/playback";
    [Export] public string newStateName = "state";
    
     public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        var anim = blackboard[animTreeName] as AnimationTree;
        var stateMachine = anim.Get(animTreeStateMachineName) as AnimationNodeStateMachinePlayback;
        stateMachine?.Travel(newStateName);
        return TickResult.SUCCESS;
    }
}