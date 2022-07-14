using Godot;
using System;

public class StepCheck : Position3D {

    [Export] public float stepHeight = 0.5f;
    [Export] public float stepMinDepth = 0.125f;
    [Export] public float stepCheckDistance = 0.5f;

    public bool isStepValid = false;

    private RayCast castTop;
    private RayCast castBottom;

    public override void _Ready(){
        castTop = GetNode<RayCast>("RayCastTop");
        castBottom = GetNode<RayCast>("RayCastBottom");
        var trans_copy = castTop.Transform; 
        trans_copy.origin.y = stepHeight;
        castTop.Transform = trans_copy; // You said this copy was trans???
        castTop.CastTo = castBottom.CastTo = new Vector3(0f,0f,-stepCheckDistance);
    }

    public override void _PhysicsProcess(float delta){
        var temp = isStepValid;
        isStepValid = (castBottom.IsColliding() && !castTop.IsColliding());
        if (temp != isStepValid){
            GD.Print($"isStepValid={isStepValid}");
        }
    }

}
