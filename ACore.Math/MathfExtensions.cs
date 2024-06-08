using Godot;

namespace ACore.Game.Math;

public class MathfExtensions
{
    /// <summary>
    /// Linear interpolation between <c>First</c> and <c>Second</c> by <c>Amount</c>
    /// </summary>
    public static Vector3 Lerp(Vector3 from, Vector3 to, float weight)
    {
        float retX = Mathf.Lerp(from.X, to.X, weight);
        float retY = Mathf.Lerp(from.Y, to.Y, weight);
        float retZ = Mathf.Lerp(from.Z, to.Z, weight);
        return new Vector3(retX, retY, retZ);
    }

    /// <summary>
    /// Linear interpolation between <c>First</c> and <c>Second</c> by <c>Amount</c>
    /// </summary>
    public static Vector2 Lerp(Vector2 from, Vector2 to, float weight)
    {
        float retX = Mathf.Lerp(from.X, to.X, weight);
        float retY = Mathf.Lerp(from.Y, to.Y, weight);
        return new Vector2(retX, retY);
    }
}
