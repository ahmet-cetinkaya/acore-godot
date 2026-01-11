# Project Structure

This document describes the organization and structure of the acore-godot project.

## Directory Layout

```
acore-godot/
├── src/                          # Source code
│   ├── ACore.Player/             # Player controller system
│   │   ├── Player.cs             # Main Player class
│   │   ├── Player.tscn           # Player scene
│   │   ├── assets/               # Player-specific assets
│   │   │   └── images/
│   │   │       └── dot-crosshair.png
│   │   ├── Grabbing/             # Object grabbing mechanics
│   │   │   ├── Player.Grabbing.cs
│   │   │   └── constants/
│   │   │       └── PlayerGrabbingInput.cs
│   │   ├── Interaction/          # Interaction system
│   │   │   ├── PlayerInteraction.cs
│   │   │   ├── abstraction/
│   │   │   │   └── IIntractable.cs
│   │   │   └── constants/
│   │   │       └── InteractionInputControls.cs
│   │   └── Movement/             # Movement mechanics
│   │       ├── Player.Movement.cs
│   │       ├── Player.Movement.Lean.cs
│   │       └── constants/
│   │           └── PlayerMovementInputControls.cs
│   ├── ACore.Math/               # Math utilities
│   │   └── MathfExtensions.cs
│   ├── ACore.Dev/                # Development assets
│   │   └── Textures/
│   │       └── Grids/
│   │           ├── Dark/
│   │           └── Orange/
│   └── ACore.Environment/        # Environment assets
│       └── Sky/
│           └── hdris/
├── docs/                         # Documentation
│   ├── api/                      # API reference
│   ├── guides/                   # User guides
│   └── assets/
│       └── godot-icon.png
├── .gitignore
├── LICENSE
└── README.md
```

## Module Overview

### ACore.Player

The player controller system provides a modular 3D first-person controller with:

- **Movement** - WASD movement with walking, sprinting, crouching, and jumping
- **Lean** - Left/right leaning mechanic with collision detection
- **Interaction** - Raycast-based interaction system using the `IInteractable` interface
- **Grabbing** - Physics-based object pickup, carry, and throwing

**Key Files:**
- `Player.cs` - Main player class orchestrating all systems
- `Player.Movement.cs` - Core movement physics and input handling
- `Player.Movement.Lean.cs` - Leaning mechanic with AnimationTree integration
- `PlayerInteraction.cs` - Interaction raycast and prompt system
- `Player.Grabbing.cs` - Physics-based grabbing with rotation

### ACore.Math

Mathematical utility extensions for Godot.

**Key Files:**
- `MathfExtensions.cs` - Vector2 and Vector3 linear interpolation methods

### ACore.Dev

Development assets for prototyping and debugging.

**Contents:**
- Grid textures (Dark and Orange variants)

### ACore.Environment

Environment assets for scene lighting and atmosphere.

**Contents:**
- HDR skybox textures (kloofendal cloudy sky)

## Scene Hierarchy

### Player Scene (`Player.tscn`)

```
Player (CharacterBody3D)
├── Body (Node3D)
│   └── Camera (Camera3D)
├── Model (Node3D)
│   └── LeanPivot (Node3D)
│       └── LeanPivotHead (Node3D)
│           ├── LeanPivotLeftShapeCast (ShapeCast3D)
│           ├── LeanPivotRightShapeCast (ShapeCast3D)
│           ├── StandingCollision (CollisionShape3D)
│           ├── CrouchingCollision (CollisionShape3D)
│           ├── CrouchingHeadRayCast (RayCast3D)
│           ├── PlayerInteraction (RayCast3D)
│           └── GrabbingTwoHandMarker (Marker3D)
│               └── GrabbingTwoHandStaticBody (StaticBody3D)
├── GrabbingJoint (Generic6DOfJoint3D)
└── GrabbingInteractionRayCast (RayCast3D)
```

## Namespace Structure

```
ACore
├── Math
│   └── MathfExtensions
└── Player
    ├── Movement
    │   └── constants
    │       └── PlayerMovementInputControls
    ├── Interaction
    │   ├── abstraction
    │   │   └── IIntractable
    │   └── constants
    │       └── InteractionInputControls
    └── Grabbing
        └── constants
            └── PlayerGrabbingInput
```

## File Organization Patterns

### Partial Classes

The `Player` class is split across multiple files using C# partial classes:

- `Player.cs` - Core lifecycle and orchestration
- `Player.Movement.cs` - Movement functionality
- `Player.Movement.Lean.cs` - Lean mechanics
- `Player.Grabbing.cs` - Grabbing functionality

This pattern keeps related functionality grouped while maintaining a single unified class.

### Constants

Input action constants are organized in `constants/` subdirectories within each feature module.

### Abstractions

Interfaces are placed in `abstraction/` subdirectories (e.g., `IInteractable`).

## Adding New Features

When extending the player system with new features:

1. Create a new partial class file: `src/ACore.Player/Player.{FeatureName}.cs`
2. Add constants in: `src/ACore.Player/{FeatureName}/constants/{FeatureName}InputControls.cs`
3. Add any abstractions in: `src/ACore.Player/{FeatureName}/abstraction/`
4. Update the scene hierarchy in `Player.tscn` with any required nodes
5. Add input actions to the project's Input Map
