using Godot;
using System;
using System.Collections.Generic;

public class IsHeadSpaceRayColliding : BehaviourTreeQuery {

    [Export] public string playerControllerName = "player_controller";

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        var player = blackboard[playerControllerName] as PlayerController;
        return player.headCheck.IsColliding() ? TickResult.SUCCESS : TickResult.FAILURE;
    }
    
}
