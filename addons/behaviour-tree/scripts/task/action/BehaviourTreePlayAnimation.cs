using System;
using Godot;
using System.Collections.Generic;

public class BehaviourTreePlayAnimation : BehaviourTreeAction {

    [Export] public string animPlayerName = "animation_player";
    [Export] public string animName = "anim";
    [Export] public bool skipIfActive = true;
    [Export] public bool onlyIfNoCurrentAnimation = false;
    [Export] public float customBlend = -1f;
    [Export] public bool playBackwards = false;
    [Export] public float customSpeed = 1.0f;
    [Export] public bool fromEnd = false;
    
     public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        var anim = blackboard[animPlayerName] as AnimationPlayer;
        if (skipIfActive && anim.CurrentAnimation == animName) return TickResult.FAILURE;
        if (onlyIfNoCurrentAnimation && !anim.CurrentAnimation.Empty()) return TickResult.FAILURE;

        if (playBackwards) anim.PlayBackwards(animName, customBlend);
        else anim.Play(animName, customBlend, customSpeed, fromEnd);

        return TickResult.SUCCESS;
    }
}