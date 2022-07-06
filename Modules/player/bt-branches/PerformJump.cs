using Godot;
using System;
using System.Collections.Generic;

public class PerformJump : BehaviourTreeAction {
    // FIXME this node is currently bloated with logic! Need to smooth out those wrinkles on the brain

    [Export] public string playerControllerName = "player_controller";
    [Export] public string jumpForceName = "jump_force";

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        if(blackboard[jumpForceName] == null) return TickResult.FAILURE;
        
        var player = blackboard[playerControllerName] as PlayerController;
        float jumpForce = (float)blackboard[jumpForceName];

        if(player.groundCheck.IsColliding()) { // TODO these checks should be a separate node
            player.targetMovement.y = jumpForce;
        }
        return TickResult.SUCCESS;
    }
}
