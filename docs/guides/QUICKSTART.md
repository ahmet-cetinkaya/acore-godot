# Quick Start Guide

This guide will help you get started with acore-godot in your project.

## Installation

### As a Submodule

1. Navigate to your Godot project:

    ```bash
    cd your-godot-project
    ```

2. Add the submodule:

    ```bash
    git submodule add https://github.com/ahmet-cetinkaya/acore-godot.git addons/ACore
    ```

3. Initialize and update:

    ```bash
    git submodule update --init --recursive
    ```

### Manual Installation

1. Clone or download the repository
2. Copy the `src/` folder to your project's `addons/` directory
3. Rename `addons/src/` to `addons/ACore/`

## Project Setup

### Enable C# in Godot

1. Open your project in Godot
2. Go to **Project → Project Settings → Application → Config**
3. Set **Run → Application Type** to `C#`
4. Click **Close**

### Configure Input Actions

Add the following input actions in **Project → Project Settings → Input Map**:

#### Movement

| Action Name     | Key/Button     |
| --------------- | -------------- |
| `move_forward`  | W, Arrow Up    |
| `move_backward` | S, Arrow Down  |
| `move_left`     | A, Arrow Left  |
| `move_right`    | D, Arrow Right |
| `sprint`        | Shift          |
| `crouch`        | Ctrl           |
| `jump`          | Space          |
| `lean_left`     | Q              |
| `lean_right`    | E              |

#### Interaction

| Action Name    | Key/Button         |
| -------------- | ------------------ |
| `interact`     | E                  |
| `grab`         | Left Mouse Button  |
| `rotate_click` | Right Mouse Button |

## Using the Player

### Basic Setup

1. Open your main scene
2. Drag `addons/ACore/ACore.Player/Player.tscn` into your scene
3. Add a `WorldEnvironment` with lighting/HDRI (you can use the HDRIs from `ACore.Environment/Sky/hdris/`)
4. Add some collision geometry (StaticBody3D with CollisionShape3D)
5. Press **F5** to play

### Customizing Player Settings

Select the Player node in your scene and adjust properties in the Inspector:

**Movement:**

- `Walking Speed`: 5.0 (default)
- `Sprinting Speed`: 7.5 (default)
- `Crouching Speed`: 2.5 (default)
- `Jump Velocity`: 4.5 (default)
- `Mouse Sensitivity`: 0.1 (default)

**Lean:**

- `Lean Enabled`: true (default)
- `Leaning Duration`: 1.0 (default)

**Grabbing:**

- `Grabbing Enabled`: true (default)
- `Grabbing Pull Power`: 4.0 (default)
- `Grabbing Rotation Power`: 0.05 (default)

## Creating Interactable Objects

1. Create a new script for your object:

    ```csharp
    using ACore.Player.Interaction.abstraction;
    using Godot;

    public partial class MyInteractable : Node3D, IIntractable
    {
        public string PromptMessage { get; protected set; } = "Press E to use";

        public void OnInteract(GodotObject @object)
        {
            GD.Print("Interacted!");
            // Your interaction logic here
        }
    }
    ```

2. Attach the script to any `Node3D` in your scene
3. Add a `CollisionShape3D` to make it detectable

## Creating Grabbable Objects

1. Create a `RigidBody3D`
2. Add a `CollisionShape3D` with an appropriate shape
3. Add a mesh (MeshInstance3D) for visuals
4. The Player will automatically be able to grab it when looking at it and pressing the grab button

## Environment Assets

### HDR Skyboxes

Use the included HDR skyboxes for realistic lighting:

1. Add a `WorldEnvironment` node to your scene
2. Create a new `Environment` resource
3. Set **Sky → Sky Resource** to a new `PanoramaSkyMaterial`
4. Set **Panorama** to one of the HDRIs from `addons/ACore/ACore.Environment/Sky/hdris/`
5. Adjust **Background Energy** as needed (try 0.5 - 1.0)

## Development Assets

### Grid Textures

Use the included grid textures for prototyping and alignment:

1. Create a `MeshInstance3D`
2. Add a plane mesh or grid mesh
3. Create a new `StandardMaterial3D`
4. Set **Albedo Texture** to one of:
    - `addons/ACore/ACore.Dev/Textures/Grids/Dark/texture_07.png`
    - `addons/ACore/ACore.Dev/Textures/Grids/Orange/texture_05.png`

## Common Issues

### Player falls through floor

- Ensure your floor has a `StaticBody3D` with a `CollisionShape3D`
- Check that the collision shape is sized correctly

### Mouse not captured

- Click in the game window to capture the mouse
- Press **Escape** to release the mouse

### Cannot interact with objects

- Ensure the object has a `CollisionShape3D`
- Check that the object implements `IInteractable`
- Verify the interaction action is set to `interact`

### Cannot grab objects

- Ensure the object is a `RigidBody3D`
- Check that `Grabbing Enabled` is true on the Player
- Verify the grab action is set to `grab`

## Next Steps

- Read the [API Reference](../api/player.md) for detailed documentation
- See [Project Structure](../PROJECT_STRUCTURE.md) to understand the codebase organization
- Check the examples in the `src/ACore.Player/` directory for reference implementations
