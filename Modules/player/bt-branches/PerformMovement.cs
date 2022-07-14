using Godot;
using System;
using System.Collections.Generic;

public class PerformMovement : BehaviourTreeAction {

    [Export] public string playerControllerName = "player_controller";
    [Export] public string movementVectorName = "move_vector";
    [Export] public string speedName = "move_speed";

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        if (blackboard[movementVectorName] == null) return TickResult.FAILURE;
        if (blackboard[speedName] == null) return TickResult.FAILURE;

        var player = blackboard[playerControllerName] as PlayerController;
        Vector2 moveVector = (Vector2)blackboard[movementVectorName];
        float speed = (float)blackboard[speedName];
        
        if(moveVector.Length() > 1.0f) moveVector = moveVector.Normalized();
        
        var move = new Vector3();
        var basisZ = -player.camera.GlobalTransform.basis.z;
        var basisX = player.camera.GlobalTransform.basis.x;
        {// fix y problems shortening the normalized vector
            basisZ.y = 0;
            basisZ = basisZ.Normalized();
            basisX.y = 0;
            basisX = basisX.Normalized();
        }
        move += basisZ * moveVector.y;
        move += basisX * moveVector.x;
        move.y = 0f;
        player.targetMovement = move * speed;
        return TickResult.SUCCESS;
    }

}
