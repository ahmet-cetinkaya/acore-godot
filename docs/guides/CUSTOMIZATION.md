# Customization Guide

This guide covers how to customize and extend the acore-godot player system.

## Extending the Player

### Adding New Player Features

The Player class uses C# partial classes, allowing you to add functionality in separate files.

1. Create a new file `src/ACore.Player/Player.YourFeature.cs`:

```csharp
using Godot;

namespace ACore.Player;

public partial class Player : CharacterBody3D
{
    [Export]
    [ExportGroup("Your Feature")]
    private bool YourFeatureEnabled { get; set; } = true;

    private void handleYourFeaturePhysics()
    {
        if (!YourFeatureEnabled)
            return;

        // Your feature logic here
    }
}
```

2. Hook into the main update loop by modifying `Player.cs`:

```csharp
public override void _PhysicsProcess(double delta)
{
    handleMovementPhysics(delta);
    handleGrabbingPhysics();
    handleYourFeaturePhysics(); // Add your handler
}
```

### Adding New Input Actions

1. Create a constants file:

```csharp
// src/ACore.Player/YourFeature/constants/YourFeatureInputControls.cs
namespace ACore.Player.YourFeature.constants;

public static class YourFeatureInputControls
{
    public const string YourAction = "your_action";
}
```

2. Add the input action to Godot's Input Map
3. Use the constant in your code:

```csharp
if (Input.IsActionJustPressed(YourFeatureInputControls.YourAction))
{
    // Handle action
}
```

## Custom Movement

### Modifying Movement Speeds

Adjust the exported properties on the Player node:

```
Movement/
├── Speed/
│   ├── Walking Speed: 5.0
│   ├── Sprinting Speed: 7.5
│   ├── Crouching Speed: 2.5
│   ├── Jump Velocity: 4.5
│   └── Lerp Speed: 10.0
```

### Changing Movement Feel

The movement uses linear interpolation for smooth transitions:

```csharp
_direction = MathfExtensions.Lerp(
    _direction,
    targetDirection,
    LerpSpeed * (float)delta
);
```

To make movement more responsive:
- Increase `LerpSpeed` (default: 10.0)
- Set closer to 20.0 for snappy movement

To make movement smoother:
- Decrease `LerpSpeed`
- Set closer to 5.0 for floaty movement

### Adding Stamina System

```csharp
// src/ACore.Player/Player.Stamina.cs
using Godot;

namespace ACore.Player;

public partial class Player : CharacterBody3D
{
    [Export]
    [ExportGroup("Stamina")]
    public float MaxStamina { get; set; } = 100.0f;

    [Export]
    [ExportGroup("Stamina")]
    public float StaminaDrainRate { get; set; } = 10.0f;

    [Export]
    [ExportGroup("Stamina")]
    public float StaminaRegenRate { get; set; } = 5.0f;

    public float CurrentStamina { get; private set; }

    public override void _Ready()
    {
        CurrentStamina = MaxStamina;
    }

    private void handleStamina(double delta)
    {
        bool isSprinting = Input.IsActionPressed(PlayerMovementInputControls.Sprint);

        if (isSprinting && CurrentStamina > 0)
        {
            CurrentStamina = Mathf.Max(0, CurrentStamina - StaminaDrainRate * (float)delta);
        }
        else if (!isSprinting && CurrentStamina < MaxStamina)
        {
            CurrentStamina = Mathf.Min(MaxStamina, CurrentStamina + StaminaRegenRate * (float)delta);
        }
    }

    // Modify sprint to check stamina
    private void sprint()
    {
        if (_isCrouching || CurrentStamina <= 0)
            return;

        _currentSpeed = SprintingSpeed;
    }
}
```

## Custom Interaction

### Custom Interaction Prompts

The `IInteractable` interface allows full control over interaction prompts:

```csharp
public partial class Door : Node3D, IIntractable
{
    public string PromptMessage { get; protected set; }

    public override void _Ready()
    {
        PromptMessage = IsLocked ? "[E] Unlock Door" : "[E] Open Door";
    }

    public void OnInteract(GodotObject @object)
    {
        // Handle interaction
    }
}
```

### Multiple Interaction Types

Create specialized interfaces:

```csharp
public interface IPickable
{
    void Pickup(Node3D picker);
}

public interface IDamageable
{
    void Damage(float amount);
}
```

Check for multiple interfaces:

```csharp
public partial class MultiInteractable : Node3D, IIntractable, IPickable, IDamageable
{
    public string PromptMessage { get; protected set; } = "[E] Interact | [Click] Attack";

    public void OnInteract(GodotObject @object)
    {
        // Interaction logic
    }

    public void Pickup(Node3D picker)
    {
        // Pickup logic
    }

    public void Damage(float amount)
    {
        // Damage logic
    }
}
```

## Custom Grabbing

### Throwing Mechanics

Add throwing velocity when dropping objects:

```csharp
// Add to Player.Grabbing.cs
[Export]
[ExportGroup("Grabbing")]
private float _throwForce = 10.0f;

private void dropObjectFromGrabbing()
{
    if (_grabbedObject == null)
        return;

    // Add forward velocity for throwing
    if (Input.IsActionPressed(PlayerMovementInputControls.Sprint))
    {
        Vector3 throwDirection = _camera.GlobalTransform.Basis.Z;
        _grabbedObject.LinearVelocity = throwDirection * _throwForce;
    }

    _grabbedObject = null;
    GrabbingJoint.NodeB = GrabbingJoint.GetPath();
}
```

### Carry Limit

Limit the number or weight of grabbable objects:

```csharp
[Export]
[ExportGroup("Grabbing")]
public float MaxCarryWeight { get; set; } = 50.0f;

private float getCurrentCarryWeight()
{
    // Calculate total mass of grabbed object
    return _grabbedObject?.Mass ?? 0;
}

private bool canGrabObject(RigidBody3D obj)
{
    return getCurrentCarryWeight() + obj.Mass <= MaxCarryWeight;
}
```

## Animation Integration

### Custom Animation Tree

The Player system integrates with Godot's AnimationTree. Set up your AnimationTree:

1. Create an AnimationTree node as a child of Player
2. Add these blend spaces:
   - `LeanBlend` (-1: left, 0: center, 1: right)
   - `LeanLeftCollisionBlend` (0: no collision, 1: collision)
   - `LeanRightCollisionBlend` (0: no collision, 1: collision)

3. Reference it in the Player's `AnimationTree` property

### Adding Movement Animations

```csharp
// In Player.Movement.cs
private void updateAnimationTree()
{
    if (AnimationTree == null)
        return;

    Vector2 inputDir = Input.GetVector(
        PlayerMovementInputControls.MoveLeft,
        PlayerMovementInputControls.MoveRight,
        PlayerMovementInputControls.MoveForward,
        PlayerMovementInputControls.MoveBackward
    );

    // Set movement blend space
    AnimationTree.Set("parameters/MovementBlend/blend_position", inputDir);
}
```

## UI Integration

### Custom HUD

Create a custom HUD scene and reference it from the Player:

```csharp
[Export]
public CustomHud Hud { get; set; }

private void updateHud()
{
    if (Hud != null)
    {
        Hud.UpdateStamina(CurrentStamina, MaxStamina);
        Hud.UpdateHealth(CurrentHealth, MaxHealth);
    }
}
```

## Physics Settings

### Adjusting Gravity

The system uses Godot's default gravity. To customize:

```csharp
private float _gravity = 20.0f; // Override default
```

Or modify project-wide:
- **Project → Project Settings → Physics → 3D → Default Gravity**: 9.8

### Collision Layers

Configure collision layers for Player interactions:

- Layer 1: Player
- Layer 2: Environment
- Layer 3: Interactable
- Layer 4: Grabbable

## Debugging

### Visual Debug Shapes

Add debug visualization:

```csharp
public override void _Draw()
{
    // Draw interaction ray
    if (_interactionRayCast.IsColliding())
    {
        Vector3 start = _interactionRayCast.GlobalPosition;
        Vector3 end = _interactionRayCast.GetCollisionPoint();
        DrawLine(ToLocal(start), ToLocal(end), Colors.Yellow);
    }
}

// Enable in _Ready():
public override void _Ready()
{
    SetPhysicsProcess(true);
}
```

### Debug Output

Add debug prints:

```csharp
if (OS.IsDebugBuild())
{
    GD.Print($"Speed: {_currentSpeed}, Crouching: {_isCrouching}");
}
```
