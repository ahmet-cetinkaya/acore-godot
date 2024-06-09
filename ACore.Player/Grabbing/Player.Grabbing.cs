using ACore.Player.Grabbing.constants;
using Godot;

namespace ACore.Player;

public partial class Player : CharacterBody3D
{
    [Export]
    [ExportGroup("Grabbing")]
    protected bool GrabbingEnabled { get; set; } = true;

    [Export]
    [ExportGroup("Grabbing")]
    protected Generic6DofJoint3D GrabbingJoint { get; set; }

    [Export]
    [ExportGroup("Grabbing")]
    protected Marker3D _grabbingTwoHandMarker;

    [Export]
    [ExportGroup("Grabbing")]
    protected StaticBody3D _grabbingTwoHandStaticBody;

    [Export]
    [ExportGroup("Grabbing")]
    protected float _grabbingPullPower = 4;

    [Export]
    [ExportGroup("Grabbing")]
    protected float _grabbingRotationPower = 0.05f;

    [Export]
    [ExportGroup("Grabbing")]
    protected RayCast3D _grabbingInteractionRayCast;

    protected RigidBody3D _grabbedObject;

    private void handleGrabbingUnhandledInputs(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            if (Input.IsActionPressed(PlayerGrabbingInput.INPUT_ROTATE_CLICK))
                rotateObject(mouseMotionEvent);

            if (Input.IsActionPressed(PlayerGrabbingInput.INPUT_ROTATE_CLICK))
                rotateObject(mouseMotionEvent);
        }
    }

    private void handleGrabbingInputs()
    {
        if (!GrabbingEnabled)
        {
            dropObjectFromGrabbing();
            return;
        }

        if (Input.IsActionJustPressed(PlayerGrabbingInput.INPUT_PICKUP) && _grabbedObject == null)
            pickupObjectForGrabbing();

        if (Input.IsActionJustReleased(PlayerGrabbingInput.INPUT_PICKUP) && _grabbedObject != null)
            dropObjectFromGrabbing();

        if (Input.IsActionJustPressed(PlayerGrabbingInput.INPUT_ROTATE_CLICK) && !LookLocked)
            LookLocked = true;
        if (Input.IsActionJustReleased(PlayerGrabbingInput.INPUT_ROTATE_CLICK) && LookLocked)
            LookLocked = false;
    }

    private void handleGrabbingPhysics()
    {
        if (!GrabbingEnabled)
            return;

        if (_grabbedObject != null)
        {
            Vector3 a = _grabbedObject.GlobalTransform.Origin;
            Vector3 b = _grabbingTwoHandMarker.GlobalTransform.Origin;
            _grabbedObject.LinearVelocity = (b - a) * _grabbingPullPower;
        }
    }

    private void pickupObjectForGrabbing()
    {
        GodotObject collider = _grabbingInteractionRayCast.GetCollider();
        if (collider == null)
            return;
        if (collider is not RigidBody3D objectRigidBody)
            return;

        _grabbedObject = objectRigidBody;
        GrabbingJoint.NodeB = _grabbedObject.GetPath();
    }

    private void dropObjectFromGrabbing()
    {
        if (_grabbedObject == null)
            return;

        _grabbedObject = null;
        GrabbingJoint.NodeB = GrabbingJoint.GetPath();
    }

    private void rotateObject(InputEventMouseMotion @event)
    {
        if (_grabbedObject == null)
            return;

        _grabbingTwoHandStaticBody.RotateX(
            Mathf.DegToRad(@event.Relative.Y * _grabbingRotationPower)
        );
        _grabbingTwoHandStaticBody.RotateY(
            Mathf.DegToRad(@event.Relative.X * _grabbingRotationPower)
        );
    }
}
