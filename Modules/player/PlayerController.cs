using Godot;
using System;

public class PlayerController : RigidBody
{

    [Export] public float moveVelocity = 10.0f;
    [Export] public float sprintVelocityScale = 1.5f;
    [Export] public float sprintJumpForceScale = 0.8f; // kinda evil >:D
    [Export(PropertyHint.Range, "0.0,1.0")] public float acceleration = 0.3f;
    [Export(PropertyHint.Range, "0.0,1.0")] public float deacceleration = 0.1f;
    [Export] public float jumpForce = 50.0f;
    [Export(PropertyHint.Range, "0.0,1.0")] public float jumpGroundNormalContribution = 0.75f;
    [Export] public float mouseSpeed = 0.3f;




    public Camera camera;
    public Spatial cameraRoot;
    public Spatial cameraRootX;
    public Vector3 targetMovement = new Vector3();
    public RayCast groundCheck;
    public RayCast headCheck;

    public override void _Ready() {
        camera = GetNode<Camera>("CameraRigOffset/CameraRoot/CamX/Camera");
        cameraRoot = GetNode<Spatial>("CameraRigOffset/CameraRoot");
        cameraRootX = GetNode<Spatial>("CameraRigOffset/CameraRoot/CamX");
        groundCheck = GetNode<RayCast>("GroundCast");
        headCheck = GetNode<RayCast>("CameraRigOffset/HeadCast");
    }



    public override void _IntegrateForces(PhysicsDirectBodyState state) {
        // alter existing vector to lean towards desired vector
        // this can't happen in the behaviour tree because it has to happen in the IntregrateForces method.
        // so we use targetMovement as a proxy for the velocity values. Packing the jump data in as well.
        if (targetMovement.LengthSquared() > 0.1f){
            var xyAxis = new Vector3(targetMovement.x, LinearVelocity.y, targetMovement.z);
            var dotClamped = Mathf.Clamp(LinearVelocity.Dot(xyAxis) * 0.5f + 0.5f, 0.0f, 1.0f); // Clamping shouldn't be required, but just in case
            var lerpStrength = Mathf.Lerp(deacceleration, acceleration, dotClamped); // lerp between acceleration and deacceleration
            LinearVelocity = LinearVelocity.LinearInterpolate(xyAxis, lerpStrength); // lerp velocity using determined value

            if (targetMovement.y > 0.0f && LinearVelocity.y <= 0.0f){
                state.ApplyCentralImpulse(new Vector3(0, targetMovement.y, 0)); // impulse jumps
            }
        }
    }


    public override void _Input(InputEvent _event) {
        if (_event.IsActionPressed("ui_cancel")){
            // toggle mouse lock
            if (Input.MouseMode == Input.MouseModeEnum.Captured){
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }else{
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
        }
        if(_event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured){
            var mouseEvent = _event as InputEventMouseMotion;
            cameraRoot.RotateY(Mathf.Deg2Rad(mouseEvent.Relative.x * mouseSpeed * -1f));


            cameraRootX.RotateX(Mathf.Deg2Rad(mouseEvent.Relative.y * mouseSpeed * -1f));
            var camRot = cameraRootX.RotationDegrees;
            camRot.x = Mathf.Clamp(camRot.x, -70, 70);
            cameraRootX.RotationDegrees = camRot;
        }
    }

}
