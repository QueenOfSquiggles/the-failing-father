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

    // private void ProcessInput() {
    //     // Convert the input keys into a desired physical vector (dir + mag)
    //     targetMovement = new Vector3();
    //     var input_dir = Input.GetVector("move_left", "move_right", "move_back", "move_forwards", 0.1f);
    //     var isSprinting = Input.IsActionPressed("sprint");
    //     var cam_xform = camera.GlobalTransform;
    //     targetMovement += -cam_xform.basis.z * input_dir.y;
    //     targetMovement += cam_xform.basis.x * input_dir.x;
    //     targetMovement.y = 0f;
    //     var speed = isSprinting? moveVelocity * sprintVelocityScale : moveVelocity;
    //     targetMovement = targetMovement.Normalized() * speed;
    //     if (Input.IsActionPressed("jump") && groundCheck.IsColliding() && targetMovement.y <= 0.0f){
    //         var jumpAmount = isSprinting ? jumpForce * sprintJumpForceScale : jumpForce; // fuck them kids >:D
    //         var up_jump = Vector3.Up * jumpAmount;
    //         var normal_jump = groundCheck.GetCollisionNormal() * jumpAmount;
    //         targetMovement += normal_jump * jumpGroundNormalContribution;
    //         targetMovement += up_jump * (1.0f - jumpGroundNormalContribution);
    //     }
    // }

}
