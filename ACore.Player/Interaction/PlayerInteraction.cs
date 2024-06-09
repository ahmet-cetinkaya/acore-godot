using ACore.Player.Interaction.abstraction;
using ACore.Player.Interaction.constants;
using Godot;

namespace ACore.Player;

public partial class PlayerInteraction : RayCast3D
{
    [Export]
    public CollisionObject3D[] Exceptions { get; set; }

    [Export]
    public Label Prompt { get; set; }

    [Export]
    private RayCast3D _interactionRayCast;

    public override void _Ready()
    {
        addExceptions();
    }

    public override void _PhysicsProcess(double delta)
    {
        handleInteraction();
    }

    private void addExceptions()
    {
        foreach (CollisionObject3D exception in Exceptions)
            AddException(exception);
    }

    private void handleInteraction()
    {
        if (!string.IsNullOrWhiteSpace(Prompt.Text))
            Prompt.Text = string.Empty;

        if (!IsColliding())
            return;
        GodotObject @object = GetCollider();
        if (@object is not IIntractable intractable)
            return;

        if (Prompt.Text != intractable.PromptMessage)
            Prompt.Text = intractable.PromptMessage;
        if (Input.IsActionJustPressed(InteractionInputControls.Interact))
            intractable.OnInteract(@object);
    }
}
