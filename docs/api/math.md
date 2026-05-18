# Math API Reference

Utility classes for mathematical operations in Godot.

## MathfExtensions Class

**Namespace:** `ACore.Math`
**Type:** `static class`

Extension methods for Godot's `Mathf` class, providing vector interpolation utilities.

### Methods

#### Vector3 Lerp

```csharp
public static Vector3 Lerp(Vector3 from, Vector3 to, float weight)
```

Linearly interpolates between two `Vector3` values.

**Parameters:**

- `from` (`Vector3`): Starting vector
- `to` (`Vector3`): Target vector
- `weight` (`float`): Interpolation weight (0.0 = from, 1.0 = to)

**Returns:** `Vector3` - Interpolated vector

**Example:**

```csharp
Vector3 a = new Vector3(0, 0, 0);
Vector3 b = new Vector3(10, 10, 10);
Vector3 mid = MathfExtensions.Lerp(a, b, 0.5f); // (5, 5, 5)
```

---

#### Vector2 Lerp

```csharp
public static Vector2 Lerp(Vector2 from, Vector2 to, float weight)
```

Linearly interpolates between two `Vector2` values.

**Parameters:**

- `from` (`Vector2`): Starting vector
- `to` (`Vector2`): Target vector
- `weight` (`float`): Interpolation weight (0.0 = from, 1.0 = to)

**Returns:** `Vector2` - Interpolated vector

**Example:**

```csharp
Vector2 a = new Vector2(0, 0);
Vector2 b = new Vector2(100, 100);
Vector2 mid = MathfExtensions.Lerp(a, b, 0.5f); // (50, 50)
```

### Usage in Player System

The `MathfExtensions` class is used throughout the Player system for smooth transitions:

```csharp
// Smooth movement direction changes
_direction = MathfExtensions.Lerp(
    _direction,
    targetDirection,
    LerpSpeed * (float)delta
);

// Smooth crouch position changes
_leanPivotHead.Position = MathfExtensions.Lerp(
    _leanPivotHead.Position,
    new Vector3(_leanPivotHead.Position.X, CrouchDepth, _leanPivotHead.Position.Z),
    LerpSpeed * (float)delta
);
```
