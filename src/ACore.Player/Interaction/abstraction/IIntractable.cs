using Godot;

namespace ACore.Player.Interaction.abstraction;

public interface IIntractable
{
    public string PromptMessage { get; protected set; }

    public void OnInteract(GodotObject @object);
}
