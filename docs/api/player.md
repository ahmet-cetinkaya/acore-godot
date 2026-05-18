# Player API Reference

The Player system provides a comprehensive 3D first-person controller with modular movement, interaction, and grabbing capabilities.

## Player Class

**Namespace:** `ACore.Player`
**Base Class:** `CharacterBody3D`
**Scene:** `src/ACore.Player/Player.tscn`

### Properties

#### Animation

| Property        | Type            | Description                                           |
| --------------- | --------------- | ----------------------------------------------------- |
| `AnimationTree` | `AnimationTree` | Reference to the animation tree for player animations |

#### Movement

| Property                | Type               | Default | Description                             |
| ----------------------- | ------------------ | ------- | --------------------------------------- |
| `_body`                 | `Node3D`           | -       | Body pivot for rotation                 |
| `_model`                | `Node3D`           | -       | 3D model reference                      |
| `_leanPivotHead`        | `Node3D`           | -       | Head pivot for leaning/crouching        |
| `_camera`               | `Camera3D`         | -       | Player camera                           |
| `MouseSensitivity`      | `float`            | `0.1`   | Mouse look sensitivity                  |
| `WalkingSpeed`          | `float`            | `5.0`   | Movement speed when walking             |
| `SprintingSpeed`        | `float`            | `7.5`   | Movement speed when sprinting           |
| `CrouchingSpeed`        | `float`            | `2.5`   | Movement speed when crouching           |
| `JumpVelocity`          | `float`            | `4.5`   | Vertical velocity for jumping           |
| `LerpSpeed`             | `float`            | `10.0`  | Interpolation speed for smooth movement |
| `_standingCollision`    | `CollisionShape3D` | -       | Collision shape for standing            |
| `_crouchingCollision`   | `CollisionShape3D` | -       | Collision shape for crouching           |
| `_crouchingHeadRayCast` | `RayCast3D`        | -       | Raycast to detect ceiling collision     |
| `CrouchDepth`           | `float`            | `-0.4`  | Y-position offset when crouching        |
| `LookLocked`            | `bool`             | `false` | Whether camera look is locked           |

#### Movement/Lean

| Property                   | Type          | Default | Description                         |
| -------------------------- | ------------- | ------- | ----------------------------------- |
| `LeanEnabled`              | `bool`        | `true`  | Enable/disable leaning mechanic     |
| `LeaningDuration`          | `float`       | `1.0`   | Duration of lean animation          |
| `_leanPivotLeftShapeCast`  | `ShapeCast3D` | -       | Shape cast for left lean collision  |
| `_leanPivotRightShapeCast` | `ShapeCast3D` | -       | Shape cast for right lean collision |
| `_leanPivot`               | `Node3D`      | -       | Pivot node for leaning              |

#### Grabbing

| Property                      | Type                 | Default | Description                             |
| ----------------------------- | -------------------- | ------- | --------------------------------------- |
| `GrabbingEnabled`             | `bool`               | `true`  | Enable/disable grabbing mechanic        |
| `GrabbingJoint`               | `Generic6DofJoint3D` | -       | Physics joint for grabbed objects       |
| `_grabbingTwoHandMarker`      | `Marker3D`           | -       | Marker for grab position                |
| `_grabbingTwoHandStaticBody`  | `StaticBody3D`       | -       | Static body for rotation                |
| `_grabbingPullPower`          | `float`              | `4`     | Strength of grab pull                   |
| `_grabbingRotationPower`      | `float`              | `0.05`  | Speed of grab rotation                  |
| `_grabbingInteractionRayCast` | `RayCast3D`          | -       | Raycast for detecting grabbable objects |
| `_grabbedObject`              | `RigidBody3D`        | `null`  | Currently grabbed object                |

### Methods

| Method                        | Description                                       |
| ----------------------------- | ------------------------------------------------- |
| `_UnhandledInput(InputEvent)` | Handles unhandled input for movement and grabbing |
| `_Input(InputEvent)`          | Handles input for grabbing                        |
| `_PhysicsProcess(double)`     | Handles physics updates for movement and grabbing |

---

## PlayerInteraction Class

**Namespace:** `ACore.Player`
**Base Class:** `RayCast3D`

### Properties

| Property              | Type                  | Description                                 |
| --------------------- | --------------------- | ------------------------------------------- |
| `Exceptions`          | `CollisionObject3D[]` | Objects to exclude from interaction raycast |
| `Prompt`              | `Label`               | UI label for interaction prompt             |
| `_interactionRayCast` | `RayCast3D`           | Internal raycast for interaction detection  |

### Methods

| Method                    | Description                                       |
| ------------------------- | ------------------------------------------------- |
| `_Ready()`                | Initializes exception list                        |
| `_PhysicsProcess(double)` | Processes interaction detection each frame        |
| `addExceptions()`         | Adds collision objects to raycast exceptions      |
| `handleInteraction()`     | Checks for interactable objects and handles input |

### Usage

To make an object interactable, implement the `IInteractable` interface:

```csharp
using ACore.Player.Interaction.abstraction;

public partial class MyInteractable : Node3D, IIntractable
{
    public string PromptMessage { get; protected set; } = "Press E to interact";

    public void OnInteract(GodotObject @object)
    {
        GD.Print("Interacted by: " + @object.Name);
    }
}
```

---

## IInteractable Interface

**Namespace:** `ACore.Player.Interaction.abstraction`

### Members

| Member                    | Type     | Description                                        |
| ------------------------- | -------- | -------------------------------------------------- |
| `PromptMessage`           | `string` | Message displayed to player when looking at object |
| `OnInteract(GodotObject)` | `void`   | Called when player interacts with object           |

---

## Constants

### PlayerMovementInputControls

**Namespace:** `ACore.Player.Movement.constants`

| Constant       | Value             | Description                        |
| -------------- | ----------------- | ---------------------------------- |
| `MoveForward`  | `"move_forward"`  | Input action for forward movement  |
| `MoveBackward` | `"move_backward"` | Input action for backward movement |
| `MoveLeft`     | `"move_left"`     | Input action for left movement     |
| `MoveRight`    | `"move_right"`    | Input action for right movement    |
| `Sprint`       | `"sprint"`        | Input action to sprint             |
| `Crouch`       | `"crouch"`        | Input action to crouch             |
| `Jump`         | `"jump"`          | Input action to jump               |
| `LeanLeft`     | `"lean_left"`     | Input action to lean left          |
| `LeanRight`    | `"lean_right"`    | Input action to lean right         |

### InteractionInputControls

**Namespace:** `ACore.Player.Interaction.constants`

| Constant   | Value        | Description                           |
| ---------- | ------------ | ------------------------------------- |
| `Interact` | `"interact"` | Input action to interact with objects |

### PlayerGrabbingInput

**Namespace:** `ACore.Player.Grabbing.constants`

| Constant             | Value            | Description                            |
| -------------------- | ---------------- | -------------------------------------- |
| `INPUT_PICKUP`       | `"grab"`         | Input action to pickup/drop objects    |
| `INPUT_ROTATE_CLICK` | `"rotate_click"` | Input action to rotate grabbed objects |

---

## Input Map Configuration

Add these input actions to your Godot project's Input Map:

### Movement Actions

- `move_forward`: W / Arrow Up
- `move_backward`: S / Arrow Down
- `move_left`: A / Arrow Left
- `move_right`: D / Arrow Right
- `sprint`: Shift
- `crouch`: Ctrl
- `jump`: Space
- `lean_left`: Q
- `lean_right`: E

### Interaction Actions

- `interact`: E (when not grabbing)
- `grab`: Left Mouse Button (when near physics object)
- `rotate_click`: Right Mouse Button (to rotate grabbed object)
