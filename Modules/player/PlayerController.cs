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




    private Camera camera;
    private Spatial cameraRoot;
    private Spatial cameraRootX;
    private Vector3 targetMovement = new Vector3();

    private RayCast groundCheck;

    public override void _Ready() {
        camera = GetNode<Camera>("CameraRoot/CamX/Camera");
        cameraRoot = GetNode<Spatial>("CameraRoot");
        cameraRootX = GetNode<Spatial>("CameraRoot/CamX");
        groundCheck = GetNode<RayCast>("GroundCast");
        //Input.MouseMode = Input.MouseModeEnum.Captured;
        GD.Print("Testing print statements");
    }

    private float debugTimer = 0f;
    const float DEBUG_CHECK_RATE = 1.0f;
    public override void _Process(float delta) {
        debugTimer += delta;
        if(debugTimer > DEBUG_CHECK_RATE){
            debugTimer = 0f; // reset
            // // perform debug checks
            // GD.Print("Performing debugging checks!");
            // // check for stuck in position
            // if(targetMovement.Length() > 0.1f && LinearVelocity.Length() < 0.1f){
            //     // trying to move but not actual movement bug
            //     Input.MouseMode = (Input.MouseModeEnum.Visible); // unlock mouse. Doesn't get unlocked by default!
            //     GD.PrintErr("Player is likely stuck!"); // breakpoint with print so we can check the data in Engine
            // }

            // // TODO find more bugs with this method
        }

        ProcessInput();
    }



    public override void _IntegrateForces(PhysicsDirectBodyState state) {
        // alter existing vector to lean towards desired vector
        if (targetMovement.LengthSquared() > 0.1f){
            var xyAxis = new Vector3(targetMovement.x, LinearVelocity.y, targetMovement.z);
            var dotClamped = Mathf.Clamp(LinearVelocity.Dot(xyAxis) * 0.5f + 0.5f, 0.0f, 1.0f); // Clamping shouldn't be required, but just in case
            var lerpStrength = Mathf.Lerp(deacceleration, acceleration, dotClamped); // lerp between acceleration and deacceleration
            LinearVelocity = LinearVelocity.LinearInterpolate(xyAxis, lerpStrength); // lerp velocity using determined value

            if (targetMovement.y > 0.0f){
                state.ApplyCentralImpulse(new Vector3(0, targetMovement.y, 0)); // impulse jumps
            }
        }
    }

    private void ProcessInput() {
        // Convert the input keys into a desired physical vector (dir + mag)
        targetMovement = new Vector3();
        var input_dir = Input.GetVector("move_left", "move_right", "move_back", "move_forwards", 0.1f);
        var isSprinting = Input.IsActionPressed("sprint");
        var cam_xform = camera.GlobalTransform;
        targetMovement += -cam_xform.basis.z * input_dir.y;
        targetMovement += cam_xform.basis.x * input_dir.x;
        targetMovement.y = 0f;
        var speed = isSprinting? moveVelocity * sprintVelocityScale : moveVelocity;
        targetMovement = targetMovement.Normalized() * speed;
        if (Input.IsActionPressed("jump") && groundCheck.IsColliding() && targetMovement.y <= 0.0f){
            var jumpAmount = isSprinting ? jumpForce * sprintJumpForceScale : jumpForce; // fuck them kids >:D
            var up_jump = Vector3.Up * jumpAmount;
            var normal_jump = groundCheck.GetCollisionNormal() * jumpAmount;
            targetMovement += normal_jump * jumpGroundNormalContribution;
            targetMovement += up_jump * (1.0f - jumpGroundNormalContribution);
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
