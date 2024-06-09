using ACore.Player.Movement.constants;
using Godot;
using ACore.Math;

namespace ACore.Player;

public partial class Player : CharacterBody3D
{
    [Export]
    [ExportGroup("Movement")]
    private Node3D _body;

    [Export]
    [ExportGroup("Movement")]
    private Node3D _model;

    [Export]
    [ExportGroup("Movement")]
    private Node3D _leanPivotHead;

    [Export]
    [ExportGroup("Movement")]
    private Camera3D _camera;

    [Export]
    [ExportGroup("Movement")]
    public float MouseSensitivity { get; set; } = 0.1f;

    private Vector3 _direction { get; set; } = Vector3.Zero;
    private float _currentSpeed { get; set; }

    [ExportSubgroup("Speed")]
    [Export]
    public float WalkingSpeed { get; set; } = 5.0f;

    [ExportSubgroup("Speed")]
    [Export]
    public float SprintingSpeed { get; set; } = 7.5f;

    [ExportSubgroup("Speed")]
    [Export]
    public float CrouchingSpeed { get; set; } = 2.5f;

    [ExportSubgroup("Speed")]
    [Export]
    public float JumpVelocity { get; set; } = 4.5f;

    [ExportSubgroup("Speed")]
    [Export]
    public float LerpSpeed { get; set; } = 10.0f;

    [ExportSubgroup("Crouching")]
    [Export]
    private CollisionShape3D _standingCollision;

    [ExportSubgroup("Crouching")]
    [Export]
    private CollisionShape3D _crouchingCollision;

    [ExportSubgroup("Crouching")]
    [Export]
    private RayCast3D _crouchingHeadRayCast;

    [ExportSubgroup("Crouching")]
    [Export]
    public float CrouchDepth { get; set; } = -0.4f;
    private bool _isCrouching;

    public bool LookLocked { get; set; }

    private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    private void handleMovementUnhandledInputs(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            setMouseMode(mouseMotionEvent);

            if (!LookLocked)
                look(mouseMotionEvent);
        }
    }

    private void handleMovementPhysics(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y -= _gravity * (float)delta;

        if (Input.IsActionPressed(PlayerMovementInputControls.Crouch))
            crouch(delta);
        else if (Input.IsActionJustPressed(PlayerMovementInputControls.Jump) && IsOnFloor())
            velocity = jump(velocity);
        else if (Input.IsActionPressed(PlayerMovementInputControls.Sprint))
            sprint();
        else
            walk(delta);
        handleMovementLeanPhysics();

        Vector2 inputDir = Input.GetVector(
            PlayerMovementInputControls.MoveLeft,
            PlayerMovementInputControls.MoveRight,
            PlayerMovementInputControls.MoveForward,
            PlayerMovementInputControls.MoveBackward
        );

        _direction = MathfExtensions.Lerp(
            _direction,
            (_body.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized(),
            LerpSpeed * (float)delta
        );

        if (_direction != Vector3.Zero)
        {
            velocity.X = _direction.X * _currentSpeed;
            velocity.Z = _direction.Z * _currentSpeed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, _currentSpeed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _currentSpeed);
        }

        Velocity = velocity;

        _ = MoveAndSlide();
    }

    private static void setMouseMode(InputEventMouseMotion @event)
    {
        if (
            @event.IsActionPressed("ui_cancel")
            && Input.MouseMode is not Input.MouseModeEnum.Visible
        )
            Input.MouseMode = Input.MouseModeEnum.Visible;
        if (Input.MouseMode is not Input.MouseModeEnum.Captured)
            Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    private void look(InputEventMouseMotion @event)
    {
        _body.RotateY(Mathf.DegToRad(-@event.Relative.X * MouseSensitivity));
        _camera.RotateX(Mathf.DegToRad(-@event.Relative.Y * MouseSensitivity));
        _camera.Rotation = new Vector3(
            Mathf.Clamp(_camera.Rotation.X, Mathf.DegToRad(-89), Mathf.DegToRad(89)),
            _camera.Rotation.Y,
            _camera.Rotation.Z
        );
    }

    private void walk(double delta)
    {
        stand(delta);

        if (_isCrouching)
            return;

        _currentSpeed = WalkingSpeed;
    }

    private Vector3 jump(Vector3 velocity)
    {
        if (_isCrouching)
            return velocity;

        velocity.Y = JumpVelocity;
        return velocity;
    }

    private void sprint()
    {
        if (_isCrouching)
            return;

        _currentSpeed = SprintingSpeed;
    }

    private void stand(double delta)
    {
        if (_crouchingHeadRayCast.IsColliding())
            return;

        _isCrouching = false;

        _leanPivotHead.Position = MathfExtensions.Lerp(
            _leanPivotHead.Position,
            new Vector3(_leanPivotHead.Position.X, 0, _leanPivotHead.Position.Z),
            LerpSpeed * (float)delta
        );
        _standingCollision.Disabled = false;
        _crouchingCollision.Disabled = true;
    }

    private void crouch(double delta)
    {
        _isCrouching = true;

        _currentSpeed = CrouchingSpeed;

        _leanPivotHead.Position = MathfExtensions.Lerp(
            _leanPivotHead.Position,
            new Vector3(_leanPivotHead.Position.X, CrouchDepth, _leanPivotHead.Position.Z),
            LerpSpeed * (float)delta
        );
        _standingCollision.Disabled = true;
        _crouchingCollision.Disabled = false;
    }
}
