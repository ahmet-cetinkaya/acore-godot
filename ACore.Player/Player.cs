using Godot;

namespace ACore.Player;

public partial class Player : CharacterBody3D
{
    [Export]
    public AnimationTree AnimationTree { get; set; }

    public override void _UnhandledInput(InputEvent @event)
    {
        handleMovementUnhandledInputs(@event);
        handleGrabbingUnhandledInputs(@event);
    }

    public override void _Input(InputEvent @event)
    {
        handleGrabbingInputs();
    }

    public override void _PhysicsProcess(double delta)
    {
        handleMovementPhysics(delta);
        handleGrabbingPhysics();
    }
}
